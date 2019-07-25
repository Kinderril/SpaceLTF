using System;
using JetBrains.Annotations;
using UnityEngine;

public class WaitEnemyTime : BaseAction
{
    private float _endTime = 0f;

    public WaitEnemyTime([NotNull] ShipBase owner,float delta) 
        : base(owner, ActionType.waitEnemySec)
    {
        _endTime = Time.time + delta;
    }

    public override void ManualUpdate()
    {
        _owner.SetTargetSpeed(1f);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => _owner.InBattlefield),
            new CauseAction("time is out", () => _endTime < Time.time),
        };
        return c;
    }
}