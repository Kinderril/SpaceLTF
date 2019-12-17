using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public  class ReadyToAttackAction : AbstractAttackAction
{
//    protected bool _isShootEnd = false;
//    protected bool _isDogFight = false;
    public ShipPersonalInfo Target;
    private float _nextRecalTime;
//    private float _minAttackDist;
//    private float _minAttackDistToStart;
//    private float _minAttackDistToEnd;

    public ReadyToAttackAction([NotNull] ShipBase owner, [NotNull] ShipPersonalInfo target)
        : base(owner, ActionType.readyToAttack)
    {
        Target = target;
    }


    
    public override void ManualUpdate()
    {
        MoveToTarget();
    }

    protected void MoveToTarget()
    {
        var isInFront = Target.IsInFrontSector();
        if (isInFront && Target.ShipLink.CurSpeed < _owner.CurSpeed && Target.Dist < 10)
        {
            //slow little bit;
            _owner.SetTargetSpeed(0.2f);                                                                        
        }
        else
        {
            _owner.SetTargetSpeed(1f);
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

   

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("invisible", () => !Target.Visible),
            new CauseAction("target null", () => Target == null),
            new CauseAction("another target close", AnotherTargetBetter),
            new CauseAction("weapon load", () => _owner.WeaponsController.AnyWeaponIsLoaded()),
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

        }
    }
}

