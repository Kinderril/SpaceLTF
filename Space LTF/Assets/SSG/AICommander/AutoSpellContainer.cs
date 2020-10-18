using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoSpellContainer
{
    public ShipBase Target = null;
    private float _delay = 1f;

    private float _nextCheck = 1f;

    //    protected Commander _commander;
    protected SpellInGame _spellData;
    protected ShipControlCenter _owner;
//    protected BulleStartParameters _bulletStartParams;
//    protected Bullet _bulletOrigin;
    protected float ShootDistSqrt;
    protected float _maxDist;

    protected TeamIndex _teamIndex;

//    protected TestTargetPosition _testTargetPosition;
    private List<AIAsteroidPredata> _allAsteroids = new List<AIAsteroidPredata>();
    private bool _isActive = false;
//    private bool _isActive = false;
    private bool _withAsteroids = false;
    private bool _withSpeed = false;
    public bool IsActive => _isActive;
    public event Action<AutoSpellContainer> OnActive;
    private Action<int,bool> ActiveCallback;

    public AutoSpellContainer(ShipControlCenter commander, SpellInGame spellData,Action<int,bool> activeCallback)
    {
        this.ActiveCallback = activeCallback;
//        _bulletOrigin = spellData.bu
//        _bulletStartParams = spellData.BulletStartParams;
        _isActive = false;
        _allAsteroids = BattleController.Instance.CellController.Data.Asteroids;
        //        _testTargetPosition = new TestTargetPosition();
        _spellData = spellData;
        _owner = commander;
        _teamIndex = commander.TeamIndex;
        _delay = MyExtensions.Random(1f, 4f);
        var spellRad = spellData.AimRadius; //. .r.BulleStartParameters.radiusShoot;
        _maxDist = spellRad;
        ShootDistSqrt = spellRad * spellRad;
        _withAsteroids = true;
        _withSpeed = false;

        switch (spellData.SpellType)
        {
            case SpellType.shildDamage:
            case SpellType.engineLock:
            case SpellType.distShot:
            case SpellType.rechargeShield:
            case SpellType.hookShot:
                _withAsteroids = false;
                break;
        }  

        switch (spellData.SpellType)
        {
            case SpellType.lineShot:
            case SpellType.throwAround:
            case SpellType.distShot:
            case SpellType.artilleryPeriod:
            case SpellType.machineGun:
                _withSpeed = true;
                break;
        }
    }

    protected void PeriodInnerUpdate()
    {
        Vector3 trg;
        if (CanCast())
        {
            if (IsEnemyClose(out trg))
            {
                _nextCheck = Time.time + 2f;
                Cast(trg);
                _owner.CoinController.UseCoins(_spellData.CostCount, _spellData.CostPeriod);
            }
        }
    }

    private bool IsEnemyClose(out Vector3 trg)
    {
       
        if (_spellData.AffectAction.TargetType == TargetType.Ally)
        {
            if (_spellData.ShallCastToTaregtAIAction(null, Target))
            {
                trg = Target.Position;
                return true;
            }
            else
            {
                trg = Vector3.zero;
                return false;
            }
        }
        else
        {
            if (_owner.Enemies.TryGetValue(Target, out var targInfo))
            {
                var findPositionBySpeed = Target.Position;
                if (targInfo.Dist < _maxDist)
                {
                    var sDistToTarget = targInfo.Dist * targInfo.Dist;

                    findPositionBySpeed = _withSpeed ? FindPosBySpeed(_owner.Position,Target,
                        _spellData.BulletSpeed):Target.Position;

                    if (_withAsteroids)
                    {
                        if (!CheckAsteroids(sDistToTarget, findPositionBySpeed))
                        {
                            trg = Vector3.zero;
                            return false;
                        }
                    }

                }


                if (_spellData.ShallCastToTaregtAIAction(targInfo, Target))
                {
                    trg = findPositionBySpeed;
                    return true;
                }
                else
                {
                    trg = Vector3.zero;
                    return false;
                }
            }
        }
        SetActive(false, null);
        trg = Vector3.zero;
        return false;

    }

    private Vector3 FindPosBySpeed(Vector3 ownerPosition, ShipBase target, float bulletSpeed)
    {
        var dir = ownerPosition - target.Position;
        var sDistBase = dir.sqrMagnitude;
        var maxSpeed = target.MaxSpeed();
        if (!target.DamageData.IsEngineWorks  || maxSpeed > bulletSpeed || maxSpeed < 0.001f)
        {
//            Debug.LogError($"FindPosBySpeed: target.Position:{target.Position}");
            return target.Position;
        }
        var aCoef = target.MaxSpeed() / bulletSpeed;  //aCoef < 1
        var dCoef = Mathf.Sqrt(sDistBase);
//        x1Coef = Mathf.Sqrt(sDistBase / (1 - aCoef * aCoef));

        var v1_x = target.LookDirection.x;
        var v1_z = target.LookDirection.z;

        var v2_x = dir.x;
        var v2_z = dir.z;

        var abSk = v1_x * v2_x + v1_z * v2_z;
        var sum1 = Mathf.Sqrt(v1_x * v1_x + v1_z * v1_z);
        var sum2 = Mathf.Sqrt(v2_x * v2_x + v2_z * v2_z);

        var cosA = abSk / (sum1 * sum2);

        var kB = 2 * aCoef * dCoef * cosA;
        var kA = (1 - aCoef * aCoef);
        var kC = -sDistBase;

        float xCoef;

        var dtr = Mathf.Sqrt(kB * kB - 4 * kA * kC);
        var bot = 2 * kA;

        var x_1 = (-kB + dtr) / bot;
        var x_2 = (-kB - dtr) / bot;

        if (x_1 > 0)
        {
            xCoef = x_1 * aCoef;
            var newTargetPos = target.Position + target.LookDirection * xCoef;
//            Debug.LogError($"FindPosBySpeed: target.Position:{target.Position}  x1Coef:{x_1}  newTargetPos:{newTargetPos}   aCoef:{aCoef}  target.MaxSpeed():{target.MaxSpeed()}    bulletSpeed:{bulletSpeed}");

            return newTargetPos;
        }
        xCoef = x_2 * aCoef;
        var newTargetPos2 = target.Position + target.LookDirection * xCoef;
//        Debug.LogError($"FindPosBySpeed: target.Position:{target.Position}  x1Coef:{x_2}  newTargetPos2:{newTargetPos2}   aCoef:{aCoef}  target.MaxSpeed():{target.MaxSpeed()}    bulletSpeed:{bulletSpeed}");

        return newTargetPos2;

//        Debug.DrawRay(newTargetPos,Vector3.up*4,Color.yellow,3);
//        Debug.DrawRay(newTargetPos2,Vector3.up*4,Color.green,3);

       
//        return newTargetPos;
//return target.Position;
    }

    private bool CheckAsteroids(float sDistToTarget,Vector3 targetPosition)
    {
        foreach (var aiAsteroidPredata in _allAsteroids)
        {
            var sDist = (aiAsteroidPredata.Position - _owner.Position).sqrMagnitude;
            if (sDist < sDistToTarget)
            {
                var pPoint =
                    AIUtility.GetProjectionPoint(aiAsteroidPredata.Position, _owner.Position,
                        targetPosition);
                var dirToPp = pPoint - aiAsteroidPredata.Position;
                var sDistPP = dirToPp.sqrMagnitude;
                if (sDistPP < aiAsteroidPredata.Rad)
                {
                    return false;
                }

            }
        }

        return true;
    }

    public void SetActive(bool val,ShipBase target)
    {
//        if (val)
//            Debug.LogError($"ActivateAiSpell {this._spellData.Name}");
//        else
//        {
//
//            Debug.LogError($"DISABLE {this._spellData.Name}");
//        }

        if (val && Target != null && target != Target)
        {
            ActiveCallback(Target.Id,false);
        }

        if (!val && Target != null && target == null)
        {
            ActiveCallback(Target.Id, false);
        }
        
        var prevTarget = target;
        Target = target;
        _isActive = val;
        OnActive?.Invoke(this);
        if (!val)
            DisposeTarget();

        if (_isActive && Target != null)
        {
            ActiveCallback(Target.Id, _isActive);
        }
        else if (prevTarget != null)
        {
            ActiveCallback(prevTarget.Id, _isActive);
        }
    }

    private void OnDeathTarget(ShipBase obj)
    {
        SetActive(false, null);
    }

    private void DisposeTarget()
    {
        if (Target != null)
        {
            Target.OnDeath -= OnDeathTarget;
            Target = null;
        }
    }


    private void Cast(Vector3 target)
    {
//        Debug.LogError($"Auto spell cast");
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetCastSpell(_spellData.SpellType));
        var startPos = _modulPos();
        Debug.DrawRay(startPos,Vector3.up * 8,Color.yellow,4);
        Debug.DrawRay(target, Vector3.up * 8,Color.blue,4);
        var dir = Utils.NormalizeFastSelf(target - startPos);
        var distToTarget = (startPos - target).magnitude;
        if (distToTarget > _maxDist)
        {
            var maxDistPos = startPos + dir * _maxDist;
            target = maxDistPos;
        }

        _spellData.CastSpell(new BulletTarget(target), _spellData.BulletOrigin, _spellData, startPos, 
            _spellData.BulletStartParams);
    }

    protected Vector3 _modulPos()
    {
        return _owner.Position;
    }

    public void PeriodlUpdate()
    {
        if (_nextCheck < Time.time)
        {
            _nextCheck = Time.time + 0.6f;
            PeriodInnerUpdate();
        }
    }

    protected bool CanCast()
    {
        return _owner.CoinController.CanUseCoins(_spellData.CostCount);
    }

    public void Dispose()
    {
        DisposeTarget();
    }
}