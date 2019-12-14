using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataBalanced : ShipDesicionDataBase
{

    public ShipDesicionDataBalanced(ShipBase owner, PilotTactic tactic)
        : base(owner, tactic)
    {

    }
    

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        throw new NotImplementedException();
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        throw new NotImplementedException();
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        throw new NotImplementedException();
    }


    public override string GetName()
    {
        return "Balance";
    }
}

