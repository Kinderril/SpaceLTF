using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DefenceShipBattle : BattleTypeEvent
{
    private bool _isShipDead = false;

    public override bool HaveActiveTime => false;
    public DefenceShipBattle()
        : base(Namings.Tag("defendMarkedShip"))
    {
    }
    public override void Init(BattleController battle)
    {
        base.Init(battle);
        var ships = battle.GreenCommander.Ships.Values;
        var toDef = ships.FirstOrDefault(x => x.ShipInventory.Marked);
        if (toDef != null)
        {
            toDef.OnDeath += OnDeath;
        }
    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, List<StartShipPilotData> paramsOfShips, Player player)
    {
        if (teamIndex == TeamIndex.green)
        {
            var config = player.MapData.CurrentCell.ConfigOwner;
            if (config == ShipConfig.droid)
            {
                config = player.Army.BaseShipConfig;
            }
            var pilot = Library.CreateDebugPilot();
            var ship = Library.CreateShip(ShipType.Middle, config, player.SafeLinks, pilot);
            ship.Marked = true;
            paramsOfShips.Add(new StartShipPilotData(pilot, ship));
        }
        return paramsOfShips;
    }

    private void OnDeath(ShipBase obj)
    {
        _isShipDead = true;
    }


    public override EndBattleType WinCondition(EndBattleType prevResult)
    {
        if (prevResult == EndBattleType.win)
        {
            if (_isShipDead)
            {
                return EndBattleType.win;
            }
            else
            {
                return EndBattleType.winFull;
            }
        }
        return prevResult;
    }
}
