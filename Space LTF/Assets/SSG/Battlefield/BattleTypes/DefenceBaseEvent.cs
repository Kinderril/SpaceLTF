using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DefenceBaseEvent : BattleTypeEvent
{
    public const float POWER_ICN = 0.35f;
    public override bool HaveActiveTime => false;

    public DefenceBaseEvent()
        : base(Namings.Tag("defendBaseWithTurrets"))
    {

    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, 
        List<StartShipPilotData> paramsOfShips, Player player)
    {
        if (teamIndex == TeamIndex.green)
        {
            var power = player.Army.GetPower();
            var coef = POWER_ICN * 0.7f;
            var toTurrent = power * coef;
            var tuuretsLogs = new ArmyCreatorLogs();
            var turrets = ArmyCreator.CreateTurrets(toTurrent, player, player.Army.BaseShipConfig, tuuretsLogs,out var point);

            foreach (var startShipPilotData in turrets)
            {
                paramsOfShips.Add(startShipPilotData);
            }

        }
        return paramsOfShips;
    }


    public override EndBattleType WinCondition(EndBattleType prevResult)
    {
        if (prevResult == EndBattleType.win)
        {
            return EndBattleType.winFull;
        }
        return prevResult;
    }

}
