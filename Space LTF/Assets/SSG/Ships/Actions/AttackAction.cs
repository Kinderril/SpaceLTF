using JetBrains.Annotations;
using System.Linq;
using UnityEngine;

public class AttackAction : AbstractAttackAction
{

    protected bool _isDogFight;
    protected bool _isShootEnd;
    private readonly float _minAttackDist;
    private readonly float _minAttackDistToEnd;
    private readonly float _minAttackDistToStart;
    protected float _nextRecalTime;
    protected float _nextCheckTwist;
    protected float _nextCheckRam;
    protected float _nextCheckTurn;
    public IShipData Target;

    public AttackAction([NotNull] ShipBase owner, [NotNull] IShipData target,
        ActionType actionType = ActionType.attack)
        : base(owner, actionType)
    {
        GlobalEventDispatcher.OnShipShootDelegate += OnShipShootDelegate;
        _isDogFight = false;
        Target = target;
        Target.ShipLink.AttackersData.ShipStartsAttack(owner);
        _minAttackDist = float.MaxValue;
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd += OnShootEnd;
            if (weapon.AimRadius < _minAttackDist)
                _minAttackDist = weapon.AimRadius;
        }

        _minAttackDistToStart = _minAttackDist * 1.3f;
        _minAttackDistToEnd = _minAttackDist * 1.7f;
        _isShootEnd = false;
    }

    protected void OnShootEnd(WeaponInGame obj)
    {
        _isShootEnd = true;
    }

    public override void ManualUpdate()
    {
        MoveToTarget();
    }
    private void OnShipShootDelegate(ShipBase shooter1, ShipBase target)
    {
        if (target == _owner && shooter1.TeamIndex != _owner.TeamIndex)
        {
            CheckBoostTwistByShoot(shooter1);
        }
    }
    private void CheckBoostTwistByShoot(ShipBase shooter)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTwist < Time.time)
        {
            _nextCheckTwist = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Enemies.TryGetValue(shooter, out var data))
            {
                _owner.Boost.BoostTwist.Activate(data);
            }
        }
    }

    protected void MoveToTarget()
    {
        CheckBoostTurn();
        CheckBoostTwist();
        CheckBoostRam();
        if (_isDogFight)
        {
            ShallEndDogFight();
            var aimPos = _owner.WeaponsController.CheckWeaponFire(Target);
            // _owner.SetTargetSpeed(1f);
            if (Target.ShipLink.CurSpeed < 0.01f)
            {
                _owner.MoveToDirection(Target.DirNorm);
            }
            else
            {
                var dir = GetDirToAimPos(aimPos);
                _owner.MoveToDirection(dir);
            }
        }
        else
        {
            ShallStartDogFight();
            if (Target.ShipLink.CurSpeed <= 0.1f && Target.Dist < _owner.MaxTurnRadius * 2)
            {
                var pp = AIUtility.GetByWayDirection(_owner.Position, Target.ShipLink.Position,
                    _owner.LookDirection,
                    _owner.MaxTurnRadius);
                if (pp != null)
                {
                    var trg = _owner.CellController.GetCell(pp.Right);
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
                _owner.MoveByWay(Target);
        }
    }

    private void CheckBoostTwist()
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTwist < Time.time)
        {
            _nextCheckTwist = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD); ;
            if (_owner.Boost.BoostTwist.ShallStartUse(Target))
            {
                _owner.Boost.BoostTwist.Activate(Target);
            }
        }
    }
    private void CheckBoostRam()
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckRam < Time.time)
        {
            _nextCheckRam = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Boost.BoostRam.ShallStartUse(Target))
            {
                _owner.Boost.BoostRam.Activate();
            }
        }
    }

    private void CheckBoostTurn()
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTurn < Time.time)
        {
            _nextCheckTurn = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Boost.BoostTurn.ShallStartUse(Target))
            {
                _owner.Boost.BoostTurn.Activate(Target.DirNorm);
            }
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
        GlobalEventDispatcher.OnShipShootDelegate -= OnShipShootDelegate;
        Target.ShipLink.AttackersData.ShipEndsAttack(_owner);
        foreach (var weapon in _owner.WeaponsController.GelAllWeapons())
            weapon.OnShootEnd -= OnShootEnd;
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
            new CauseAction("weapon not load", () => _owner.WeaponsController.AllDamageWeaponNotLoad(ShipDesicionDataBase.POSIBLE_UNLOAD_WEAPONS)),
            new CauseAction("target is dead", () => Target.ShipLink.IsDead)
        };
        return c;
    }

    protected virtual bool AnotherTargetBetter()
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
            var all = _owner.WeaponsController.GelAllWeapons();
            if (all.Count > 0)
            {
                var weapon = all.FirstOrDefault();
                weapon?.GizmosDraw();
            }
        }
    }
}