using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class AttackAction : AbstractAttackAction
{
    protected bool _isShootEnd = false;
    protected bool _isDogFight = false;
    public ShipPersonalInfo Target;
    private float _nextRecalTime;
    private float _minAttackDist;
    private float _minAttackDistToStart;
    private float _minAttackDistToEnd;

    public AttackAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target,
        ActionType actionType = ActionType.attack)
        : base(owner, actionType)
    {
        _isDogFight = false;
        Target = target;
        _minAttackDist = Single.MaxValue;
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd += OnShootEnd;
            if (weapon.AimRadius < _minAttackDist)
            {
                _minAttackDist = weapon.AimRadius;
            }
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
        if (_isDogFight)
        {
            ShallEndDogFight();
            _owner.WeaponsController.CheckWeaponFire(Target);
            _owner.SetTargetSpeed(1f);
            if (Target.ShipLink.CurSpeed < 0.01f)
            {
                var dir = Target.ShipLink.Position - _owner.Position;
                _owner.MoveToDirection(dir);
            }
            else
            {
                var dir = Target.ShipLink.PredictionPosAim() - _owner.Position;
                _owner.MoveToDirection(dir);
            }
        }
        else
        {
            if (Target.Dist < _minAttackDistToStart && Target.ShipLink.CurSpeed < 0.01f)
            {
            }

            ShallStartDogFight();
            //            bool tooClose = false;
            if (Target.ShipLink.CurSpeed <= 0.1f)
            {
                var maxTurn = _owner.MaxTurnRadius * 2;
                if (Target.Dist < maxTurn)
                {
                    var pp = AIUtility.GetByWayDirection(_owner.Position, Target.ShipLink.Position,
                        _owner.LookDirection,
                        _owner.MaxTurnRadius);
                    if (pp != null)
                    {
                        var trg = _owner.CellController.FindCell(pp.Right);
                        if (trg.IsFree())
                        {
                            //                            _owner.SetTargetSpeed(tooClose ? 0.3f : 1f);
                            _owner.MoveByWay(pp.Right);
                            return;
                        }
                    }

                    //Делаем по прямой
                }
            }

            var pp1 = AIUtility.GetByWayDirection(_owner.Position, Target.ShipLink.Position, _owner.LookDirection,
                _owner.MaxTurnRadius);
            if (pp1 != null)
            {
                _owner.MoveByWay(pp1.Right);
            }
            else
            {
                _owner.MoveByWay(Target.ShipLink);
            }
        }
    }

    private void ShallStartDogFight()
    {
        var isInFront = _owner.IsInFromt(Target.ShipLink);
        if (isInFront)
        {
            if (Target.Dist < _minAttackDistToStart)
            {
                _isDogFight = true;
            }
        }
    }

    private void ShallEndDogFight()
    {
        if (Target.Dist > _minAttackDistToEnd)
        {
            _isDogFight = false;
        }
    }

    protected override void Dispose()
    {
        foreach (var weapon in _owner.WeaponsController.GelAllWeapons())
        {
            weapon.OnShootEnd -= OnShootEnd;
        }
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
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
                if (besstEnemy != Target)
                {
                    return true;
                }
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