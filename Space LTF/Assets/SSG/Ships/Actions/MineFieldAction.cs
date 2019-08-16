using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class MineFieldAction : BaseAction
{
    private MineAbstractModul _mineModul;
    private AICell _cellToProtect;
    private Vector3 _cellDanger;

    public MineFieldAction([NotNull] ShipBase owner,MineAbstractModul mineModul, AICell cellToProtect, Vector3 dangerCell) 
        : base(owner,ActionType.mineField)
    {
        _cellToProtect = cellToProtect;
        _cellDanger = dangerCell;
        _mineModul = mineModul;
        FindWay();
    }

    public override void ManualUpdate()
    {
        var dir = _cellToProtect.Center - _owner.Position;
        var sDist = dir.sqrMagnitude;
        if (sDist < 3*3 && sDist > 1)
        {
            SetMine();
        }
        _owner.SetTargetSpeed(1f);
        if (_targetPoint != null)
            _owner.MoveByWay(_targetPoint.Value);
    }

    private void SetMine()
    {
        _mineModul.SetMine();
    }

    private void FindWay()
    {
        var dir = Utils.NormalizeFastSelf(_cellDanger - _cellToProtect.Center);
        var pos = _cellToProtect.Center + dir*2f;

        _targetPoint = (pos);
    }


    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("target is dead", () => !_targetPoint.HasValue),
            new CauseAction("path complete", () => _owner.PathController.Complete(_targetPoint.Value)),
        };
        return c;
    }

    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint;
    }

    public override void DrawGizmos()
    {
    }
}

