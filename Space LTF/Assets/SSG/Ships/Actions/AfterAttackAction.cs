using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class AfterAttackAction : BaseAction
{
    private bool _canDoBackflip;
    public AfterAttackAction([NotNull] ShipBase owner) 
        : base(owner,ActionType.afterAttack)
    {
        FindWay();
        _canDoBackflip = MyExtensions.IsTrue01(.45f);
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

    private void CheckBackflip()
    {
        if (_owner.AttackersData.CurAttacker != null && !_owner.Boost.BoostBackflip.IsActive)
        {
            if (_owner.Boost.CanUse)
            {
                _owner.Boost.ActivateBackflip();
//                if (Time.time - _owner.AttackersData.CurAttacker.ShipLink.WeaponsController.LastShootTime <
//                    Time.deltaTime * 4f)
//                {
//                    _owner.Boost.ActivateBackflip();
//                }
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
            new CauseAction("_targetPoint null", () => _targetPoint == null && !_owner.Boost.BoostBackflip.IsActive),
            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)  && !_owner.Boost.BoostBackflip.IsActive),
            new CauseAction("weapon load", () => _owner.WeaponsController.AnyWeaponIsLoaded()  && !_owner.Boost.BoostBackflip.IsActive)
        };
        return c;
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

