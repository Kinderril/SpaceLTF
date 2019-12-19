using System;
using JetBrains.Annotations;
using UnityEngine;

public class WaitEnemy : BaseAction
{
    private bool _shallEnd = false;
    private Vector3 _point;

    public WaitEnemy([NotNull] ShipBase owner,Vector3 point) 
        : base(owner, ActionType.waitEnemy)
    {
        _point = point;
    }

    public override void ManualUpdate()
    {
        ShipBase closest = null;
        float dist = Single.MaxValue;
        foreach (var info in _owner.Enemies.Values)
        {
            if (info.Dist < dist)
            {
                dist = info.Dist;
                closest = info.ShipLink;
            }
            if (info.Dist < ShipDesicionDataDefenceBase._defDist * 0.9f)
            {
                _shallEnd = true;
                return;
            }
        }

        if (closest != null)
        {
            var dir = Utils.NormalizeFastSelf(closest.Position - _owner.Position);
            var goodDir = Utils.IsAngLessNormazied(dir, _owner.LookDirection, UtilsCos.COS_8_RAD);
            if (!goodDir)
            {
                _owner.ApplyRotation(dir, true);
            }
        }
        // _owner.SetTargetSpeed(0f);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => _owner.InBattlefield),
            new CauseAction("enemy come close", () => _shallEnd),
        };
        return c;
    }
}