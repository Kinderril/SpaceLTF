using JetBrains.Annotations;
using UnityEngine;

public class AttackAction : AbstractAttackAction
{
    protected bool _isDogFight;
    protected bool _isShootEnd;
    private readonly float _minAttackDist;
    private readonly float _minAttackDistToEnd;
    private readonly float _minAttackDistToStart;
    private float _nextRecalTime;
    public ShipPersonalInfo Target;

    public AttackAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target,
        ActionType actionType = ActionType.attack)
        : base(owner, actionType)
    {
        _isDogFight = false;
        Target = target;
        Target.ShipLink.AttackersData.ShipStartsAttack(owner);
        _minAttackDist = float.MaxValue;
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd += OnShootEnd;
            if (weapon.AimRadius < _minAttackDist) _minAttackDist = weapon.AimRadius;
        }

        _minAttackDistToStart = _minAttackDist * 1.3f;
        _minAttackDistToEnd = _minAttackDist * 1.7f;
        _isShootEnd = false;
    }

    private void OnShootEnd(WeaponInGame obj)
    {
        _isShootEnd = true;
    }

    public override void ManualUpdate()
    {
        MoveToTarget();
    }

    protected void MoveToTarget()
    {
        if (Target.Dist < _minAttackDistToStart)
        {
            if (_owner.Boost.CanUse) _owner.Boost.ActivateTurn(Target.DirNorm);
        }
        else if (Target.IsInBack() && Target.Dist < 14)
        {
            if (_owner.Boost.CanUse) _owner.Boost.ActivateBack();
        }

        if (_isDogFight)
        {
            ShallEndDogFight();
            _owner.WeaponsController.CheckWeaponFire(Target);
            // _owner.SetTargetSpeed(1f);
            if (Target.ShipLink.CurSpeed < 0.01f)
            {
                _owner.MoveToDirection(Target.DirNorm);
            }
            else
            {
                var dir = Target.ShipLink.PredictionPosAim() - _owner.Position;
                _owner.MoveToDirection(dir);
            }
        }
        else
        {
            ShallStartDogFight();
            //            bool tooClose = false;
            if (Target.ShipLink.CurSpeed <= 0.1f && Target.Dist < _owner.MaxTurnRadius * 2)
            {
                var pp = AIUtility.GetByWayDirection(_owner.Position, Target.ShipLink.Position,
                    _owner.LookDirection,
                    _owner.MaxTurnRadius);
                if (pp != null)
                {
                    var trg = _owner.CellController.FindCell(pp.Right);
                    if (trg.IsFree())
                    {
                        _owner.MoveByWay(pp.Right);
                        return;
                    }
                }
            }

            var pp1 = AIUtility.GetByWayDirection(_owner.Position, Target.ShipLink.Position, _owner.LookDirection,
                _owner.MaxTurnRadius);
            if (pp1 != null)
                _owner.MoveByWay(pp1.Right);
            else
                _owner.MoveByWay(Target.ShipLink);
        }
    }

    private void ShallStartDogFight()
    {
        var isInFront = Target.IsInFrontSector();
        if (isInFront)
            if (Target.Dist < _minAttackDistToStart)
                _isDogFight = true;
    }

    private void ShallEndDogFight()
    {
        if (Target.Dist > _minAttackDistToEnd) _isDogFight = false;
    }

    protected override void Dispose()
    {
        Target.ShipLink.AttackersData.ShipEndsAttack(_owner);
        foreach (var weapon in _owner.WeaponsController.GelAllWeapons()) weapon.OnShootEnd -= OnShootEnd;
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("is Shoot End", () => _isShootEnd),
            new CauseAction("invisible", () => !Target.Visible),
            new CauseAction("target null", () => Target == null),
            new CauseAction("another target close", AnotherTargetBetter),
            new CauseAction("weapon not load", () => _owner.WeaponsController.AllWeaponNotLoad()),
            new CauseAction("target is dead", () => Target.ShipLink.IsDead)
        };
        return c;
    }

    private bool AnotherTargetBetter()
    {
        if (_nextRecalTime < Time.time)
        {
            _nextRecalTime = Time.time + MyExtensions.Random(0.8f, 1.2f);

            if (Target.Dist > 20)
            {
                var besstEnemy = _shipDesicionDataBase.CalcBestEnemy(_owner.Enemies);
                if (besstEnemy != Target) return true;
            }
        }

        return false;
    }

    public override Vector3? GetTargetToArrow()
    {
        return Target.ShipLink.Position;
    }

    public override void DrawGizmos()
    {
        if (Target != null)
        {
//            Gizmos.color = Color.blue;
//            Gizmos.DrawRay(_owner.Position, Vector3.up * 5);
//            Gizmos.color = Color.green;
//            if (_isClose)
//            {
//                Gizmos.color = Color.blue;
//            }
//            Gizmos.DrawLine(_owner.Position, Target.ShipLink.Position);
        }
    }
}