using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ReturnActionToBattlefield : BaseAction
{
    private Vector3 posToReturn;

    public ReturnActionToBattlefield([NotNull] ShipBase owner) 
        : base(owner,ActionType.returnToBattle)
    {
        var cell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Free);
        posToReturn = cell.Center;
        FindWay();
    }

    public override void ManualUpdate()
    {
        // _owner.SetTargetSpeed(1f);
        if (_targetPoint != null)
            _owner.MoveByWay(_targetPoint.Value);
    }

    private void FindWay()
    {
        _targetPoint = (posToReturn);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => _owner.InBattlefield),
            new CauseAction("no target", () => !_targetPoint.HasValue),
            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)),
        };
        return c;
    }
    
    public override void DrawGizmos()
    {

    }
}

