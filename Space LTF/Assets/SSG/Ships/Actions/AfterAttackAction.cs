using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class AfterAttackAction : BaseAction
{
    public AfterAttackAction([NotNull] ShipBase owner) 
        : base(owner,ActionType.afterAttack)
    {
        FindWay();
    }

    public override void ManualUpdate()
    {
        _owner.ShipModuls.Update();
        // _owner.SetTargetSpeed(1f);
        if (_targetPoint != null)
            _owner.MoveByWay(_targetPoint.Value);
        _owner.AttackersData.UpdateData();
    }

    private void FindWay()
    {
        var point = _owner.CellController.Data.FreePoints.RandomElement();
        _targetPoint = point;
        return;

        var trgCell = _owner.CellController.GetCellByDir(_owner.Cell, _owner.LookDirection);
        if (trgCell.IsFree())
        {
            _targetPoint = trgCell.Center;
            return;
        }
        Vector3 d1, d2;
        if (MyExtensions.IsTrue01(.5f))
        {
            d1 = _owner.LookLeft;
            d2 = _owner.LookRight;
        }
        else
        {
            d1 = _owner.LookRight;
            d2 = _owner.LookLeft;
        }
        trgCell = _owner.CellController.GetCellByDir(_owner.Cell, d1);
        if (trgCell.IsFree())
        {
            _targetPoint = trgCell.Center;
            return;
        }
        trgCell = _owner.CellController.GetCellByDir(_owner.Cell,d2);
        if (trgCell.IsFree())
        {
            _targetPoint = trgCell.Center;
            return;
        }
        trgCell = _owner.CellController.GetCellByDir(_owner.Cell,-_owner.LookDirection);
        if (trgCell.IsFree())
        {
            _targetPoint = trgCell.Center;
            return;
        }
        var freeCell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Free);
        _targetPoint = freeCell.Center;

    }
    
    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("_targetPoint null", () => _targetPoint == null),
            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)),
            new CauseAction("weapon load", () => _owner.WeaponsController.AnyWeaponIsLoaded())
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

