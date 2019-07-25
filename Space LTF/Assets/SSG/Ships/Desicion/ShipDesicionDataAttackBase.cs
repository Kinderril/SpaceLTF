using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataAttackBase : ShipDesicionDataBase
{
    public const float distToAttack = 10f;
    private ShipBase _mainEnemyShip;

    public ShipDesicionDataAttackBase(ShipBase owner,ShipBase enemyCommanderMainShip)
        :base(owner)
    {
        _mainEnemyShip = enemyCommanderMainShip;
    }

    public override PilotTcatic GetTacticType()
    {
        return PilotTcatic.attackBase;
    }

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        return DoOrWait(AttackOrAttackSide(ship), ship);
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        ship = _mainEnemyShip;
        return DoOrWait(AttackOrAttackSide(ship), ship);
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        var bestEnemy = CalcBestEnemy(_owner.Enemies);
        if (bestEnemy != null)
        {
            ship = bestEnemy.ShipLink;
            return true;
        }
        ship = null;
        return false;
    }

    protected override float ShipValue(ShipPersonalInfo info)
    {
        float cRating = 0;
        if (info.ShipLink.ShipParameters.StartParams.ShipType == ShipType.Base)
                {
                    cRating += 40;
                }
//        var dot = Utils.FastDot(info.DirNorm, _owner.LookDirection);
//        var isFront = dot > 0;
//        if (info.Dist < 15)
//        {
//            if (isFront)
//            {
//                cRating = 100 + info.Dist;
//            }
//            else
//            {
//                cRating = 30 + info.Dist;
//            }
//        }
//        else
//        {
//            cRating = info.Dist;
//        }
//
//        if (info.ShipLink.ShipParameters.StartParams.ShipType == ShipType.Base)
//        {
//            cRating += 40;
//        }
//        if (!info.Visible)
//        {
//            cRating -= 99999;
//        }

        return cRating;
    }

    public override string GetName()
    {
        return "Base attack";
    }
}

