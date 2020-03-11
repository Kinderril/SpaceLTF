using System;
using JetBrains.Annotations;
using UnityEngine;

public class WaitEnemy : BaseAction
{
    private bool _shallEnd = false;
    private Vector3 _point;
    private float _nextRefresh;

    public WaitEnemy([NotNull] ShipBase owner) 
        : base(owner, ActionType.waitEnemy)
    {
        _point = _owner.Commander.GetWaitPosition(_owner);
    }

    public override void ManualUpdate()
    {
        if (_owner.DesicionData.HaveEnemyInDangerZoneDefenceBase(out var closest))
        {
            _shallEnd = true;
              return;
        }

        if (_nextRefresh < Time.time)
        {
            _nextRefresh = Time.time + 3f;
            _point = _owner.Commander.GetWaitPosition(_owner);
        }
        var dir = Utils.NormalizeFastSelf(_point - _owner.Position);
        var goodDir = Utils.IsAngLessNormazied(dir, _owner.LookDirection, UtilsCos.COS_8_RAD);
        if (!goodDir)
        {
            _owner.ApplyRotation(dir, true);
        }
    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(new Ray(_point, Vector3.up * 10));
        Gizmos.color = Color.white;
        base.DrawGizmos();
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("enemy come close", () => _shallEnd),
        };
        return c;
    }
}