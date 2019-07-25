using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataAttack : ShipDesicionDataBase
{

    public ShipDesicionDataAttack(ShipBase owner)
        :base(owner)
    {

    }
    
    public override PilotTcatic GetTacticType()
    {
        return PilotTcatic.attack;
    }

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        return DoOrWait(AttackOrAttackSide(ship), ship);
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        ship = null;
        return ActionType.waitEnemySec;
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        float enemyRating;
        if (_owner.Enemies.Count > 0)
        {
            var enemy = CalcBestEnemy(out enemyRating, _owner.Enemies);
            ship = enemy.ShipLink;
            return true;
        }
        else
        {
            ship = null;
            return false;
        }
    }

    public override string GetName()
    {
        return "Attack";
    }

}

