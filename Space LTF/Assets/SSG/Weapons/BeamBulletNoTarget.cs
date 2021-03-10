using System;
using System.Collections.Generic;
using UnityEngine;

struct BeamTarget
{
    public BeamTarget(Dictionary<AIAsteroidPredata, Vector3> AsteroidPredatas
        , Dictionary<ShipBase, Vector3> Ships)
    {
        this.AsteroidPredatas = AsteroidPredatas;
        this.Ships = Ships;
    }

    public Dictionary<AIAsteroidPredata,Vector3> AsteroidPredatas;
    public Dictionary<ShipBase, Vector3> Ships;
}

public class BeamBulletNoTarget : Bullet
{
    private bool _canActivate;
    protected float _deathTime;
    protected float _startTime;
    private const float SHIP_SIZE_SQRT = 2f;
    private const float _startPeriod = 0.4f;
    private const float _endPeriod = 1.6f;
    private const float _offPeriod = 0.1f;
    private const float _offStartTime = _endPeriod- _offPeriod;
    private const float _damagePeriod = 0.5f;
    private float _nextDamagePeriod = 1.5f;
    private float _lastTargetPeriod = 0f;
    private float _startOff = 1.5f;
    private string _roadName;
    private Vector3? _lastHitTarget = null;
    private Vector3 _lastTarget;

    public float coefWidth = 1f;

//    private string _undersideName;
    private Color _crolor;
    public GameObject EdgeObject;
    private bool haveEdgeObject = false;
    private Vector3 _startScaleEdgeObject = Vector3.one;
    private Vector3? _selectedTrg = null;

    public CurveLineAbsorber ProcessEvent;
    void Awake()
    {
        haveEdgeObject = EdgeObject != null;
        if (haveEdgeObject)
        {
            _startScaleEdgeObject = EdgeObject.transform.localScale;
        }
        _roadName = NcCurveAnimation.Ng_GetMaterialColorName(ProcessEvent.ParticleAttractor.roadMaterial);
//        _undersideName = NcCurveAnimation.Ng_GetMaterialColorName(ProcessEvent.ParticleAttractor.undersideMaterial);
        ProcessEvent.ParticleAttractor.roadMaterial = Utils.CopyMaterial(ProcessEvent.ParticleAttractor.roadMaterial);
//        ProcessEvent.ParticleAttractor.undersideMaterial = Utils.CopyMaterial(ProcessEvent.ParticleAttractor.undersideMaterial);
        _crolor = ProcessEvent.ParticleAttractor.roadMaterial.GetColor(_roadName);
    }


    public override BulletType GetType => BulletType.beamNoTarget;

    public override void Init()
    {
        _selectedTrg = null;
        _canActivate = false;
        ProcessEvent.gameObject.SetActive(false);
        _selectedTrg = null;
        base.Init();
    }


    public override void LateInit()
    {
        _deathTime = Time.time + _endPeriod;
        _crolor.a = 1f;
        _startTime = Time.time + _startPeriod;
        _nextDamagePeriod = Time.time + _damagePeriod;
        _startOff = Time.time + _offStartTime;
        _lastHitTarget = null;
        base.LateInit();
        _canActivate = true;
    }

    public void SetDeathTime(float dTime)
    {
        // Debug.LogError($"Ser dt:{dTime}");
        _deathTime = dTime;
        _startOff = Time.time + _offStartTime;
    }

    protected override void ManualUpdate()
    {
        if (Time.timeScale <= 0.0001f)
        {
            return;
        }
        var hittedTarget = DoDamagePeriod();
        if (!TimeEndCheck())
        {
            if (_canActivate)
            {
                ProcessEvent.Play();
                _canActivate = false;
                ProcessEvent.gameObject.SetActive(true);
            }

            var period = (_startTime - Time.time);
            Vector3 trg;
            if (hittedTarget.HasValue)
            {
                trg = hittedTarget.Value;
            }   
            else if (_selectedTrg.HasValue)
            {
                trg = _selectedTrg.Value;
            }
            else
            {
                if (period > 0)
                {
                    var p = 1f - period / _startPeriod;

                    var dir = _endPos - Weapon.CurPosition;
                    dir.y = 0;
                    dir = Utils.NormalizeFastSelf(dir);
                    trg = Weapon.CurPosition + dir * p * _distanceShoot;
                }
                else
                {
                    trg = _endPos;
                }
            }

            Updatetickness();
            UpdateOffPeriod();
            _lastTarget = Vector3.Lerp(_lastTarget, trg,0.3f);  
            MoveTo(_lastTarget, Weapon.CurPosition);
        }
    }

    private Vector3? DoDamagePeriod()
    {
        if (_lastTargetPeriod < Time.time)
        {
            _lastTargetPeriod = Time.time + 0.1f;
            var targets = FindAllTargets();
            var distAsteroid = Single.MaxValue;
            Vector3? closestPrPoint = null;
            AIAsteroidPredata closetsAsteroid = null;
            ShipBase closetsShip = null;
            foreach (var targetsAsteroidPredata in targets.AsteroidPredatas)
            {
                if (targetsAsteroidPredata.Key.IsDead)
                {
                    continue;
                }
                var sDist = (targetsAsteroidPredata.Value - Weapon.CurPosition).sqrMagnitude;
                if (sDist < distAsteroid)
                {
                    distAsteroid = sDist;
                    closetsAsteroid = targetsAsteroidPredata.Key;
                    closestPrPoint = targetsAsteroidPredata.Value;
                }
            }
            var distShip = Single.MaxValue;
            foreach (var shipBase in targets.Ships)
            {
                var sDist = (shipBase.Value - Weapon.CurPosition).sqrMagnitude;
                if (sDist < distAsteroid)
                {
                    distShip = sDist;
                    closetsShip = shipBase.Key;
                    closestPrPoint = shipBase.Value;
                }
            }

            bool haveTrg = distShip < 99999 || distAsteroid < 99999;
            _lastHitTarget = null;
            if (haveTrg)
            {
                if (closestPrPoint != null)
                {
                    _lastHitTarget = closestPrPoint;
                }
            }

            if (_nextDamagePeriod < Time.time)
            {
                // Debug.LogError($"_nextDamagePeriod:haveTrg:{haveTrg}");
                _nextDamagePeriod = Time.time + _damagePeriod;
                if (haveTrg)
                {
                    if (distShip < distAsteroid)
                    {
                        if (closetsShip != null)
                        {
                            // Debug.LogError($"closetsShip:haveTrg:");
                            closetsShip.GetHit(Weapon, this);
                        }
                    }
                    else
                    {
                        if (closetsAsteroid != null)
                        {
                            // Debug.LogError($"closetsAsteroid:haveTrg:{closetsAsteroid.Position}  id:{closetsAsteroid.Id}");
                            closetsAsteroid.Death();
                        }
                    }
                }
            }
        }                

        return _lastHitTarget;

    }

    private void Updatetickness()
    {
        var period = (_deathTime - Time.time);
        var p = Mathf.Clamp01(1f - period / _endPeriod);
        if (haveEdgeObject)
        {
            EdgeObject.transform.localScale = _startScaleEdgeObject * p;
        }
        ProcessEvent.ParticleAttractor.roadWidth = p * .7f * coefWidth;
    } 
    private void UpdateOffPeriod()
    {
        if (Time.time > _startOff)
        {
            var p = Mathf.Clamp01(1f -(Time.time - _startOff) / _offPeriod);
            _crolor.a = p;
        }
        else
        {
            _crolor.a = 1f;
        }
        ProcessEvent.ParticleAttractor.roadMaterial.SetColor(_roadName, _crolor);
//        ProcessEvent.ParticleAttractor.undersideMaterial.SetColor(_undersideName, _crolor);
    }

    private void MoveTo(Vector3 target, Vector3 @from)
    {
        if (haveEdgeObject)
        {
            EdgeObject.transform.position = target;
        }
        ProcessEvent.UpdatePositions(from, target);
    }

    private bool TimeEndCheck()
    {
        if (Time.time > _deathTime)
        {
            // Debug.LogError("TimeEndCheck");
            Death();
            return true;
        }
        return false;
    }

    private BeamTarget FindAllTargets()
    {
        var bc = BattleController.Instance;
        Commander commander;
        if (AffectTypeHit == BulletAffectType.damage)
        {
            commander =  Weapon.TeamIndex == TeamIndex.green ? bc.RedCommander : bc.GreenCommander;
        }
        else
        {
            commander = Weapon.TeamIndex == TeamIndex.green ? bc.GreenCommander : bc.RedCommander;
        }
        var sDistShip =SHIP_SIZE_SQRT * coefWidth * coefWidth;
        var ships = new Dictionary<ShipBase,Vector3>();
        foreach (var shipBase in commander.Ships)
        {
            if (shipBase.Value.Id != Weapon.Owner.Id)
            {
                var p = shipBase.Value.Position;
                var projectDist = CalcDistSqrDist(p, out var prPoint);
                if (projectDist < sDistShip)
                {
                    ships.Add(shipBase.Value,prPoint);
                    // shipBase.Value.GetHit(Weapon, this);
                }
            }
        }

        var asteroids = bc.CellController.Data.Asteroids;

        var asteroidsToAffect = new Dictionary<AIAsteroidPredata,Vector3>();
        foreach (var aiAsteroidPredata in asteroids)
        {
            if (aiAsteroidPredata.IsDead || asteroidsToAffect.ContainsKey(aiAsteroidPredata))
            {
                continue;
            }
            var p = aiAsteroidPredata.Position;
            var projectDist = CalcDistSqrDist(p,out var prPoint);
            if (projectDist < 1f)
            {
                asteroidsToAffect.Add(aiAsteroidPredata,prPoint);
            }
        }

        // foreach (var aiAsteroidPredata in asteroidsToAffect)
        // {
        //     aiAsteroidPredata.Death();
        // }
        return new BeamTarget(asteroidsToAffect,ships);

    }

    public void MoveTargetTo(Vector3 trg)
    {
        _selectedTrg = trg;
    }

    private float CalcDistSqrDist(Vector3 p,out Vector3 projectionPoint)
    {
        Vector3 trg;
        if (_selectedTrg.HasValue)
        {
            trg = _selectedTrg.Value;
        }
        else
        {
            trg = _endPos;
        }

        var wPos = Weapon.CurPosition;
        projectionPoint = AIUtility.GetProjectionPoint(p, trg, wPos);
        // Debug.DrawLine(wPos, trg, Color.magenta, 5f);
        var s1 = new SegmentPoints(trg, wPos);
         var sPoint = 2 * projectionPoint - p;
        var s2 = new SegmentPoints(sPoint, p);
        var crossPoint = AIUtility.GetCrossPoint(s1, s2);
        if (crossPoint.HasValue)
        {
            var projectDir = projectionPoint - p;
            projectDir.y = 0;
            var projectDist = projectDir.sqrMagnitude;
            // Debug.DrawLine(p, projection,Color.yellow,5f);
            return projectDist;
        }
        else
        {
            // Debug.DrawLine(p, projection, Color.red, 5f);
        }
        return Single.MaxValue;
    }

    protected override void DrawGizmosSelected()
    {
        Gizmos.DrawLine(Position, _endPos);
        base.DrawGizmosSelected();
    }

    protected override bool IsCatch(ShipHitCatcher s)
    {
        if (s.CatcherType == HitCatcherType.shield)
        {
            return false;
        }
        return base.IsCatch(s);
    }

    protected override void OnDestroyAction()
    {

    }

    public override void Death()
    {
        ProcessEvent.gameObject.SetActive(false);
        _canActivate = false;
        // Debug.LogError($"Beam dead {Time.time}");
        base.Death();
    }
}

