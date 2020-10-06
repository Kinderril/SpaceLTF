using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartNewGameWithSafePlayer : StartNewGameData
{
    private PlayerSafe _playerSafe;
    private int _startPower;
    public StartNewGameWithSafePlayer(Dictionary<PlayerParameterType, int> startParametersLevels, ShipConfig shipConfig,
         int SectorSize, int questsOnStart, 
        List<SpellType> posibleSpell, PlayerSafe playerSafe,int startPower, ExprolerCellMapType mapType) 
        : base(startParametersLevels, shipConfig, new List<WeaponType>(), SectorSize, 1, 
             questsOnStart, EStartGameDifficulty.Normal, posibleSpell, 5, EGameMode.safePlayer, mapType)
    {
        _startPower = startPower;
        _playerSafe = playerSafe;
    }

    public override List<StartShipPilotData> CreateStartArmy(Player player)
    {
        return _playerSafe.Ships;
    }
    public override Player CreatePlayer()
    {
        return new Player("Next Player",_playerSafe);
    }

    public override int GetStartPower()
    {
        return _startPower;
    }
}
