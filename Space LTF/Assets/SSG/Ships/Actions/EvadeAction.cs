using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class EvadeAction : BaseAction
{
    private AICell _clouds = null;

    public EvadeAction([NotNull] ShipBase owner) 
        : base(owner,ActionType.evade)
    {
        int dist;
        var cloudsCell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Clouds,false,out dist);
        if (dist < 3)
        {
            if (cloudsCell != null)
            {
                _clouds = cloudsCell;
            }
            else
            {
//                cloudsCell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Free, out dist);
            }

        }
    }

    public override void ManualUpdate()
    {
        if (_clouds != null)
        {
            // _owner.SetTargetSpeed(1f);
            var pos = _clouds.Center;
            _targetPoint = pos;
            _owner.MoveByWay(pos);
            return;
        }

        var danger = _owner.Locator.DangerEnemy;
        Vector3 target;
        bool acceleration = false;
        if (_owner.ShipParameters.MaxSpeed > danger.ShipLink.ShipParameters.MaxSpeed)
        {
            //GO STRAIGHT
            // _owner.SetTargetSpeed(1f);
            var pos = _owner.Position + _owner.LookDirection * 10;
            var cell = _owner.CellController.FindCell(pos);
            if (cell.IsFree())
            {
                _targetPoint = pos;
                _owner.MoveByWay(pos);
                return;
            }
        }
        if (_owner.ShipParameters.TurnSpeed > danger.ShipLink.ShipParameters.TurnSpeed)
        {
            //DO TURN
            Vector3 side = GetSideDir(danger);
            // _owner.SetTargetSpeed(1f);
            var pos = _owner.Position + side * 7;
            var cell = _owner.CellController.FindCell(pos);
            if (cell.IsFree())
            {
                _targetPoint = pos;
                _owner.MoveByWay(pos);
                return;
            }
        }
        else
        {
            var side = GetSideDir(danger);
            // if (_owner.IsInFromt(danger.ShipLink.Position))
            // {
            //     // _owner.SetTargetSpeed(1f);
            // }
            // else
            // {
            //     _owner.SetTargetSpeed(0.1f);
            // }

            var pos = _owner.Position + side * 7;
            _targetPoint = pos;
            _owner.MoveByWay(pos);
        }
        
    }
    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint;
    }
    private Vector3 GetSideDir(ShipPersonalInfo danger)
    {
        Vector3 side;
        var sideDot = Vector3.Dot(_owner.LookLeft, danger.DirNorm) > 0;
        side = sideDot ? _owner.LookLeft : _owner.LookRight;
        return side;
    }
    

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
//            new CauseAction("_owner dead", () => !_owner.InBattlefield),
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("no danger", () => _owner.Locator.DangerEnemy == null),
            new CauseAction("no danger ship link", () => _owner.Locator.DangerEnemy.ShipLink == null),
            new CauseAction("weapon load", () => _owner.WeaponsController.AnyWeaponIsLoaded())
//            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)),
        };
        return c;
    }
    
    public override void DrawGizmos()
    {

    }
}

