using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public  class GoToCurrentPointAction : BaseAction
{
    private Vector3 Target;
    private bool _isCome = false;

    public GoToCurrentPointAction([NotNull] ShipBase owner, [NotNull] Vector3 target)
        : base(owner,ActionType.goToCurrentPointAction)
    {
        Target = target;
    }

    public override void ManualUpdate()
    {
        _isCome = _owner.PathController.Complete(Target);
        if (!_isCome)
        {
            _owner.MoveByWay(Target);
        }
        _owner.SetTargetSpeed((!_isCome)?1f:0f);
    }

    protected override void Dispose()
    {

    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("" +
                            "", () => false),
        };
        return c;
    }
    
    public override void DrawGizmos()
    {
        Gizmos.DrawLine(Target,_owner.Position);
    }
}

