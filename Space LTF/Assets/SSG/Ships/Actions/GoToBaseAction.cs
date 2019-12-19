using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class GoToBaseAction : BaseAction
{
    public const float CloseToMainShipSQRT = 1f;
    public const float CloseToMainShipSQRT2 = CloseToMainShipSQRT*1.2f;
    private ShipBase Target;
    private bool _withRunAway;
    public GoToBaseAction([NotNull] ShipBase owner, ShipBase target,bool withRunAway) 
        : base(owner,ActionType.moveToBase)
    {
        _withRunAway = withRunAway;
        Target = target;
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
        _targetPoint = Target.Position;
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("target is dead", () => Target.IsDead),
            new CauseAction("come close", () =>
            {
                var sDist = (Target.Position - _owner.Position).sqrMagnitude;
                if (sDist < CloseToMainShipSQRT)
                {
                    if (_withRunAway)
                    {
                        _owner.Commander.ShipRunAway(_owner);
                    }
                    return true;
                }
                return false;
            }),
        };
        return c;
    }

    public override Vector3? GetTargetToArrow()
    {
        return _targetPoint;
    }

    public override void DrawGizmos()
    {
        var d = 0.2f;
        Gizmos.DrawSphere(Target.Position, d);
        Gizmos.DrawSphere(Target.Position+Vector3.up*d,d);
    }
}

