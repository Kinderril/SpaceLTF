using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StayAttackAction : AbstractAttackAction
{
    // protected bool _isDogFight;
    protected bool _isShootEnd;
    private readonly float _minAttackDist;
    // private readonly float _minAttackDistToEnd;
    // private readonly float _minAttackDistToStart;
    private float _nextRecalTime;
    public ShipPersonalInfo Target;

    public StayAttackAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target,
        ActionType actionType = ActionType.shootFromPlace)
        : base(owner, actionType)
    {
        Target = target;
        // Target.ShipLink.AttackersData.ShipStartsAttack(owner);
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd += OnShootEnd;
            if (weapon.AimRadius < _minAttackDist)
                _minAttackDist = weapon.AimRadius;
        }

    }

    private void OnShootEnd(WeaponInGame obj)
    {
        _isShootEnd = true;
    }

    public override void ManualUpdate()
    {
        CheckTarget();
        if (Target != null)
        {
            var aimPos = _owner.WeaponsController.CheckWeaponFire(Target);
            var dir = GetDirToAimPos(aimPos);
            TurnToTarget(dir);
        }
    }
    protected Vector3 GetDirToAimPos(Vector3? aimPos)
    {
        Vector3 targetAimPos;
        if (aimPos.HasValue)
        {
            targetAimPos = aimPos.Value;
        }
        else
        {
            targetAimPos = Target.ShipLink.PredictionPosAim();
        }
        var dir = targetAimPos - _owner.Position;
        return dir;
    }
    private void CheckTarget()
    {
        if (_nextRecalTime < Time.time)
        {
            _nextRecalTime = Time.time + 3f;
            var trg = BestByParams(out var rating, _owner.Enemies);
            if (trg != null)
                Target = trg;
        }
    }
    protected void TurnToTarget(Vector3 dir)
    {
        if (Target != null)
        {
            _owner.ApplyRotation(Utils.NormalizeFastSelf(dir), true);
        }
    }

    private ShipPersonalInfo BestByParams(out float rating, Dictionary<ShipBase, ShipPersonalInfo> posibleTargets)
    {
        rating = Single.MinValue;
        ShipPersonalInfo bestEnemy = null;
        foreach (var shipInfo in posibleTargets) //_owner.Enemies)
        {
            var ship = shipInfo.Value;
            // if (ship.CommanderShipEnemy.IsPriority)
            // {
            //     return ship;
            // }
            var cRating = ShipValue(ship);
            if (cRating > rating)
            {
                rating = cRating;
                bestEnemy = ship;
            }
        }
        return bestEnemy;
    }
    protected virtual float ShipValue(ShipPersonalInfo info)
    {
        float cRating = 0;
        var dot = Utils.FastDot(info.DirNorm, _owner.LookDirection);
        var isFront = dot > 0;
        if (info.Dist < 15)
        {
            if (isFront)
            {
                cRating = 1000 + info.Dist;
            }
            else
            {
                cRating = 300 + info.Dist;
            }
        }
        else
        {
            cRating = info.Dist;
        }

        if (info.ShipLink.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            cRating -= 400;
        }
        if (!info.Visible)
        {
            cRating -= 999999;
        }

        return cRating;
    }

    protected override void Dispose()
    {
        //        Target.ShipLink.AttackersData.ShipEndsAttack(_owner);
        foreach (var weapon in _owner.WeaponsController.GelAllWeapons())
            weapon.OnShootEnd -= OnShootEnd;
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new[]
        {
            // new CauseAction("out bf", () => !_owner.InBattlefield),
            // new CauseAction("is Shoot End", () => _isShootEnd),
            // new CauseAction("invisible", () => !Target.Visible),
            new CauseAction("target null", () => Target == null),
            // new CauseAction("another target close", AnotherTargetBetter),
            // new CauseAction("weapon not load", () => _owner.WeaponsController.AllWeaponNotLoad()),
            // new CauseAction("target is dead", () => Target.ShipLink.IsDead)
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