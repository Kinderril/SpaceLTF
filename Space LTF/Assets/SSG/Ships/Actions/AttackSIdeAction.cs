using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class AttackSideAction : AttackAction
{
    private Vector3 TargetPos
    {
        get { return Target.ShipLink.Position; }
    }
    
    private bool _controlReached = false;
    private Vector3 _controlPoint;

    public AttackSideAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target,Vector3 controlPoint) 
        : base(owner,target, ActionType.attackSide)
    {
        _controlPoint = controlPoint;
    }
    
    public override void ManualUpdate()
    {
        _owner.WeaponsController.CheckWeaponFire(Target);
        if (Target.Dist < 15 || _owner.PathController.Complete(_controlPoint, 1f))
        {
            _controlReached = true;
        }
        if (_controlReached)
        {
            MoveToTarget();
        }
        else
        {
            _owner.SetTargetSpeed(1f);
            _owner.MoveByWay(_controlPoint);
        }
    }
    
    public override void DrawGizmos()
    {
        if (TargetPos != null)
        {
            if (_controlReached)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_owner.Position, Target.ShipLink.Position);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_owner.Position, _controlPoint);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_controlPoint, Target.ShipLink.Position);
            }
        }
    }

    public static Vector3 FindControlPoint(Vector3 start, Vector3 end, Battlefield battlefield)
    {
        var sCell = battlefield.CellController.FindCell(start);
        var eCell = battlefield.CellController.FindCell(end);
        int xI;
        int zI;
        if (MyExtensions.IsTrue01(.5f))
        {
            xI = sCell.Xindex;
            zI = eCell.Zindex;
        }
        else
        {
            xI = eCell.Xindex;
            zI = sCell.Zindex;
        }
        var controlCell = battlefield.CellController.Data.GetCell(xI, zI);
        if (controlCell.OutOfField || controlCell.CellType != CellType.Free)
        {
            controlCell = battlefield.CellController.Data.FindClosestCellByType(controlCell, CellType.Free);
            return controlCell.Center;
        }
        return controlCell.Center;
    }
}

