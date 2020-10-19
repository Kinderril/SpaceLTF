using System.Collections.Generic;
using UnityEngine;


public class BeamBulletNoTarget : Bullet
{
    private bool _canActivate;
    protected float _deathTime;
    protected float _startTime;
    private const float _startPeriod = 0.4f;
    private const float _endPeriod = 1.6f;
    private const float _offPeriod = 0.1f;
    private const float _offStartTime = _endPeriod- _offPeriod;
    private const float _damagePeriod = 0.5f;
    private float _nextDamagePeriod = 1.5f;
    private float _startOff = 1.5f;
    private string _roadName;

    public float coefWidth = 1f;

//    private string _undersideName;
    private Color _crolor;
    public GameObject EdgeObject;
    private bool haveEdgeObject = false;
    private Vector3 _startScaleEdgeObject = Vector3.one;

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
        _canActivate = false;
        ProcessEvent.gameObject.SetActive(false);
        base.Init();
    }


    public override void LateInit()
    {
        _deathTime = Time.time + _endPeriod;
        _crolor.a = 1f;
        _startTime = Time.time + _startPeriod;
        _nextDamagePeriod = Time.time + _damagePeriod;
        _startOff = Time.time + _offStartTime;
        base.LateInit();
        _canActivate = true;
    }

    protected override void ManualUpdate()
    {
        if (Time.timeScale <= 0.0001f)
        {
            return;
        }
        DoDamagePeriod();
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

            Updatetickness();
            UpdateOffPeriod();

            MoveTo(trg, Weapon.CurPosition);
        }
    }

    private void DoDamagePeriod()
    {
        if (_nextDamagePeriod < Time.time)
        {
            _nextDamagePeriod = Time.time + _damagePeriod;
            FindAllTargets();
        }
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
            Death();
            return true;
        }
        return false;
    }

    private void FindAllTargets()
    {
        var bc = BattleController.Instance;
        var commander = Weapon.TeamIndex == TeamIndex.green ? bc.RedCommander : bc.GreenCommander;

        foreach (var shipBase in commander.Ships)
        {
            var p = shipBase.Value.Position;
            var projectDist = CalcDist(p);
            if (projectDist < 2f * coefWidth)
            {
                shipBase.Value.GetHit(Weapon, this);
            }
        }

        var asteroids = bc.CellController.Data.Asteroids;

        List<AIAsteroidPredata> toDelete = new List<AIAsteroidPredata>();
        foreach (var aiAsteroidPredata in asteroids)
        {
            var p = aiAsteroidPredata.Position;
            var projectDist = CalcDist(p);
            if (projectDist < 1f)
            {
                toDelete.Add(aiAsteroidPredata);
            }
        }

        foreach (var aiAsteroidPredata in toDelete)
        {
            aiAsteroidPredata.Death();
        }

    }

    private float CalcDist(Vector3 p)
    {
        var projection = AIUtility.GetProjectionPoint(p, _endPos, Weapon.CurPosition);
        var projectDir = projection - p;
        projectDir.y = 0;
        var projectDist = projectDir.magnitude;
        return projectDist;
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
        //        Debug.LogError($"Beam dead {Time.time}");
        base.Death();
    }
}

