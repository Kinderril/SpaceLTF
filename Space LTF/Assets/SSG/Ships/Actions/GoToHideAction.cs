using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class GoToHideAction : BaseAction
{
    private AICell _targetCell;
    private float _endTime;

    public GoToHideAction([NotNull] ShipBase owner) 
        : base(owner,ActionType.goToHide)
    {
        _endTime = Time.time + 4f;
        FindWay();
    }

    public override void ManualUpdate()
    {
        _owner.SetTargetSpeed(1f);
        if (_targetCell != null)
            _owner.MoveByWay(_targetCell.Center);
    }

    private void FindWay()
    {
        var cell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell,CellType.Clouds);
        if (cell != null && cell != _owner.Cell)
        {
            _targetCell = cell;
        }
        else
        {
            int offset = 1;
            var ix = MyExtensions.IsTrue01(.5f) ? _owner.CellController.Data.MaxIx - offset : offset;
            var iz = MyExtensions.IsTrue01(.5f) ? _owner.CellController.Data.MaxIz - offset : offset;
            var cellTmp = _owner.CellController.Data.GetCell(ix, iz);
            _targetCell = _owner.CellController.Data.FindClosestCellByType(cellTmp, CellType.Free);
        }
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("path complete go to hide", () => _owner.Cell == (_targetCell)),
            new CauseAction("_endTime go to hide", () => _endTime < Time.time),
//            new CauseAction("path complete go to hide", () => _owner.PathController.Complete(_targetPoint.Value)),
        };
        return c;
    }

    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint.Value;
    }

    public override void DrawGizmos()
    {

    }
}

