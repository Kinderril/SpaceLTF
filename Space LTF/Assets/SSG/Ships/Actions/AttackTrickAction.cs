using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AttackTrickAction : AbstractAttackAction
{
    // protected bool _isDogFight;
    // protected bool _isShootEnd;
    // private readonly float _minAttackDist;
    // private readonly float _minAttackDistToEnd;
    // private readonly float _minAttackDistToStart;
    // private float _nextRecalTime;
    public ShipPersonalInfo Target;

    private ShipBoostHalfLoop _trick;

    public AttackTrickAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target,
        ActionType actionType = ActionType.attackHalfLoop)
        : base(owner, actionType)
    {
        _trick = _owner.Boost.BoostHalfLoop;
        Target = target;
        Target.ShipLink.AttackersData.ShipStartsAttack(owner);
        _trick.Start(TargetGetter);

    }

    private Vector3? TargetGetter()
    {
        if (Target.ShipLink.IsDead)
        {
            return null;
        }

        return Target.ShipLink.Position;
    }


    public override void ManualUpdate()
    {
        if (!Target.ShipLink.IsDead)
        {
            _owner.WeaponsController.CheckWeaponFire(Target);
        }
        MoveToTarget();
    }

    protected void MoveToTarget()
    {
        _owner.Boost.BoostHalfLoop.ManualUpdate();
    }



    protected override void Dispose()
    {
        Target.ShipLink.AttackersData.ShipEndsAttack(_owner);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new[]
        {
            new CauseAction("trick ends", () => !_trick.IsActive),
        };
        return c;
    }

    public override Vector3? GetTargetToArrow()
    {
        return Target.ShipLink.Position;
    }

    public override void DrawGizmos()
    {
        if (Target != null)
        {
            var all = _owner.WeaponsController.GelAllWeapons();
            if (all.Count > 0)
            {
                var weapon = all.FirstOrDefault();
                weapon?.GizmosDraw();
            }
        }
    }
}