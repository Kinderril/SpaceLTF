using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataAttack : ShipDesicionDataBase
{

    public ShipDesicionDataAttack(ShipBase owner, PilotTactic tactic)
        :base(owner,tactic)
    {

    }
    

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        var attackOptional = AttackOrAttackSide(ship);
        return DoOrWait(attackOptional, ship);
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        ship = null;
        return ActionType.waitEnemySec;
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        if (_owner.Enemies.Count > 0)
        {
            var enemy = CalcBestEnemy(out var enemyRating, _owner.Enemies);
            ship = enemy.ShipLink;
            return true;
        }
        ship = null;
        return false;
    }

    public override string GetName()
    {
        return "Attack";
    }

}

