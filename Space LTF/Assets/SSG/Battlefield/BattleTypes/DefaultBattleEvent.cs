using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DefaultBattleEvent : BattleTypeEvent
{
    public override bool HaveActiveTime => false;

    public override void Init(BattleController battle)
    {
        base.Init(battle);

    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, 
        List<StartShipPilotData> paramsOfShips, Player player)
    {
        return paramsOfShips;
    }

}
