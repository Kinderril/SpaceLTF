using System;
using System.Collections.Generic;
using UnityEngine;


public class ShipBoostRam : ShipBoostAbstract
{
//    private const float START_PERIOD = 0.2f;
    private const float WORK_PERIOD = 0.8f;
//    private const float END_PERIOD = 0.3f;
    private const float COEF_SPEED = 2.3f;
    private const float c1 = 0.8f;
    private const float c2 = 1f - c1;

    private const float MIN_BLOCK_DIST = 3f;
    private const float MAX_BLOCK_DIST = 10f;

    private const float THROW_POWER = 10f;
    private const float SEC_DELAY_POWER = 2f;
    private const float DAMAGE_HEALTH_COEF = 0.11f;


    private Vector3 _lookDirOnStart;
    private float _speedOnStart;
    private IShipData _target;
    private float _period;
//    private float _endStartPeriod;
//    private float _startLastPeriod;
//    private float _endLastPeriod;
    // private Vector3 _dirToMoveToSide;
    // private float _minDistToAttack;
    private float _lastMoveSpeedCoef;
    private float _maxSpeed;
    private float _damagePower;
    private HashSet<ShipPersonalInfo> _hitted = new HashSet<ShipPersonalInfo>();

    public ShipBoostRam(ShipBase owner, float turnSpeed, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(owner, activateCallback, endCallback, setAddMoveCallback)
    {
        // _minDistToAttack = owner.WeaponsController.MaxAttackRadius * 1.3f;
    }
    public void Activate(IShipData target)
    {
        if (!CanUse)
        {
            return;
        }
        Debug.LogError($"Ram activated {_owner.Id}");
        _damagePower = _owner.ShipParameters.MaxHealth * DAMAGE_HEALTH_COEF;
        _hitted.Clear();
        _maxSpeed = _owner.MaxSpeed();
        _target = target;
        _speedOnStart = _owner.CurSpeed;
        _lookDirOnStart = _owner.LookDirection;
        IsActive = true;
        _owner.ExternalForce.Init(20, WORK_PERIOD, _owner.LookDirection);
        ActivateTime();
    }

    private void ActivateTime()
    {
        _period = Time.time + WORK_PERIOD;
        // _period = 0f;
        //        _endLastPeriod = Time.time + WORK_PERIOD + START_PERIOD + END_PERIOD;
        //        _startLastPeriod = Time.time + WORK_PERIOD + START_PERIOD;
        //        _endStartPeriod = Time.time + START_PERIOD;
    }

    public bool ShallStartUse(IShipData target)
    {
        if (!CanUse)
        {
            return false;
        }
        if (target == null)
        {
            return false;
        }
        var dist = target.Dist;
        if (dist > MAX_BLOCK_DIST || dist < MIN_BLOCK_DIST)
        {
            return false;
        }
        if (!target.IsInFrontSector())
            return false;

        return true;
    }

    public void ManualUpdate()
    {
        if (!IsActive)
        {
            return;
        }

        if (_target.IsDead || _target == null)
        {
            Stop();
            return;
        }

        float nextSpeed;
        if (Time.time > _period)
        {
            Stop();
            return;
        }
        CheckEnemies();
//
//        nextSpeed = nextSpeed * COEF_SPEED;
//        _lastMoveSpeedCoef = _lastMoveSpeedCoef * c1 + nextSpeed * c2;
//        var ownerCurSpeed = _lookDirOnStart * _owner.CurSpeed * Time.deltaTime * _lastMoveSpeedCoef;
//        SetAddMoveCallback(ownerCurSpeed);
    }

    private void CheckEnemies()
    {
        if (Time.frameCount % 3 == 0)
        {
            foreach (var shipPersonalInfo in _owner.Enemies)
            {
                if (!_hitted.Contains(shipPersonalInfo.Value) && shipPersonalInfo.Value.Dist < 2)
                {
                    Hit(shipPersonalInfo);
                }
            }
        }

    }

    private void Hit(KeyValuePair<ShipBase, ShipPersonalInfo> shipPersonalInfo)
    {
        DamageDoneDelegate damaged = (healthdelta, shielddelta, damageAppliyer) =>
        {
            GlobalEventDispatcher.ShipDamage(_owner, healthdelta, shielddelta, WeaponType.ramStrike);
            var coef = damageAppliyer != null ? damageAppliyer.ExpCoef : 0f;
            _owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta, coef);
            if (damageAppliyer != null)
            {
                if (damageAppliyer.IsDead)
                {
                    GlobalEventDispatcher.ShipDeath(damageAppliyer, _owner);
                    _owner.ShipInventory.LastBattleData.AddKill();
                }
            }
        };
        _hitted.Add(shipPersonalInfo.Value);
        shipPersonalInfo.Key.ShipParameters.Damage(_damagePower, _damagePower, damaged, _owner);
        shipPersonalInfo.Key.ExternalForce.Init(THROW_POWER, SEC_DELAY_POWER, shipPersonalInfo.Value.DirNorm);
    }



    private void Stop()
    {
        IsActive = false;
        Debug.LogError($"Ram stop {_owner.Id}");
        SetAddMoveCallback(Vector3.zero);
    }
}

