﻿using JetBrains.Annotations;
using UnityEngine;


public class AfterAttackAction : BaseAction
{
    private bool _canDoBackflip;
    private float _nextCheckCanSupport;
    protected float _nextCheckTwist;
    public AfterAttackAction([NotNull] ShipBase owner)
        : base(owner, ActionType.afterAttack)
    {
        FindWay();
        GlobalEventDispatcher.OnShipShootDelegate += OnShipShootDelegate;
        _nextCheckCanSupport = Time.time + 2f;
        _canDoBackflip = MyExtensions.IsTrue01(.45f);
    }

    private void OnShipShootDelegate(ShipBase shooter, ShipBase target)
    {
        if (target == _owner && shooter.TeamIndex != _owner.TeamIndex)
        {
            CheckBoostTwist(shooter);
        }
    }

    protected override void Dispose()
    {
        GlobalEventDispatcher.OnShipShootDelegate -= OnShipShootDelegate;
        base.Dispose();
    }

    public override void ManualUpdate()
    {
        _owner.ShipModuls.Update();
        // _owner.SetTargetSpeed(1f);
        if (_targetPoint != null)
            _owner.MoveByWay(_targetPoint.Value);
        _owner.AttackersData.UpdateData();
        if (_canDoBackflip)
        {
            CheckBackflip();
        }
    }
    private void CheckBoostTwist(ShipBase shooter)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTwist < Time.time)
        {
            _nextCheckTwist = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Enemies.TryGetValue(shooter, out var data))
            {
                _owner.Boost.BoostTwist.Activate(data);
            }
        }
    }

    private void CheckBackflip()
    {
        if (_owner.AttackersData.CurAttacker != null && !_owner.Boost.BoostLoop.IsActive)
        {
            if (_owner.Boost.IsReady && _owner.Boost.BoostLoop.CanUse)
            {
                _owner.Boost.BoostLoop.Activate();
            }
        }
    }

    private void FindWay()
    {
        var point = _owner.CellController.Data.FreePoints.RandomElement();
        _targetPoint = point;
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("can support", () => CanSupport()),
            new CauseAction("_targetPoint null", () => _targetPoint == null && !_owner.Boost.BoostLoop.IsActive),
            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)  && !_owner.Boost.BoostLoop.IsActive),
            new CauseAction("weapon load", () => _owner.WeaponsController.AnyDamagedWeaponIsLoaded()  && !_owner.Boost.BoostLoop.IsActive)
        };
        return c;
    }

    private bool CanSupport()
    {
        if (_owner.WeaponsController.SupportWeaponsBuffPosibilities.HaveAny)
        {
            if (_nextCheckCanSupport < Time.time)
            {
                _nextCheckCanSupport = Time.time + 2f;

                if (_owner.WeaponsController.AnySupportWeaponIsLoaded(0f, out var fullLoadSupport))
                {
                    if (_owner.HaveClosestDamagedFriend(out var ship))
                    {
                        return true;
                    }
                }
            }
        }


        return false;
    }

    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint;
    }

    public override void DrawGizmos()
    {
        //        if (_targetPoint != null)
        //        {
        //            _targetPoint.DrawGizmos(_owner.Position);
        //        }
        //        var d = 0.2f;
        //        Gizmos.DrawSphere(Target(),d);
        //        Gizmos.DrawSphere(Target()+Vector3.up*d,d);
    }
}

