using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MassiveBattle : BattleTypeEvent
{
    private BattleTypeData _eBattleType;
    public override bool HaveActiveTime => false;
    public MassiveBattle(BattleTypeData eBattleType)
        : base(Namings.Tag("massiveBattle"))
    {
        _eBattleType = eBattleType;
    }     
    public override void Init(BattleController battle)
    {
        base.Init(battle);

    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, List<StartShipPilotData> paramsOfShips, Player player)
    {
        if (teamIndex == TeamIndex.green)
        {
            for (int i = 0; i < _eBattleType.AddToPlayerCount; i++)
            {
                var config = _eBattleType._addSelfConfig;
                var data = ArmyCreatorLibrary.GetArmy(config);
                var ship = ArmyCreator.CreateShipWithWeapons(new ArmyRemainPoints(12), data, player, new ArmyCreatorLogs());
                paramsOfShips.Add(ship);
            }
        }   
        if (teamIndex == TeamIndex.red)
        {
            for (int i = 0; i < _eBattleType.AddToEnemyCount; i++)
            {
                var config = _eBattleType._addEnemyConfig;
                var data = ArmyCreatorLibrary.GetArmy(config);
                var ship = ArmyCreator.CreateShipWithWeapons(new ArmyRemainPoints(12),data, player, new ArmyCreatorLogs());
                paramsOfShips.Add(ship);
            }
        }
        return paramsOfShips;
    }




    public override EndBattleType WinCondition(EndBattleType prevResult)
    {
        return prevResult;
    }
}
