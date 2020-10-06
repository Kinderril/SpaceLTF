using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartNewGameChampaing : StartNewGameData
{
    private Player _playerSafe;
    private int _startPower;
//    public ShipConfig Config { get; private set; }
    public ShipConfig ShiConfigAllise      { get; private set; }
public int Act { get; private set; }
    public StartNewGameChampaing(Player player, Dictionary<PlayerParameterType, int> startParametersLevels,
        ShipConfig shipConfig, List<WeaponType> posibleStartWeapons, int SectorSize,
        int questsOnStart, EStartGameDifficulty difficulty, List<SpellType> posibleSpell
        , int PowerPerTurn, int act, ShipConfig shipConfigAllies) 
        : base(startParametersLevels, shipConfig, posibleStartWeapons, SectorSize, 4, 
             questsOnStart, difficulty, posibleSpell, PowerPerTurn, EGameMode.champaing, ExprolerCellMapType.normal)
    {
        Act = act;
        ShiConfigAllise = shipConfigAllies;
//        Config = shipConfigAllies;
        _playerSafe = player;
        if (act == 0)
        {
            _startPower = base.GetStartPower();
        }
        else
        {
            if (_playerSafe != null)
            {
                var myArmyPower = _playerSafe.Army.GetPower();

                switch (difficulty)
                {
                    case EStartGameDifficulty.VeryEasy:
                        myArmyPower *= 0.8f;
                        break;
                    case EStartGameDifficulty.Easy:
                        myArmyPower *= 0.9f;
                        break;
                    case EStartGameDifficulty.Hard:
                        myArmyPower *= 1.1f;
                        break;
                    case EStartGameDifficulty.Imposilbe:
                        myArmyPower *= 1.2f;
                        break;
                }

                _startPower = (int)myArmyPower;
            }
            else
            {
                _startPower = base.GetStartPower();
            }
        }
        if (_startPower < 5)
        {
            Debug.LogError("Start power is too low. AutoFix");
            _startPower = base.GetStartPower();
        }
    }

    public override List<StartShipPilotData> CreateStartArmy(Player player)
    {
        if (_playerSafe.SafeLinks.Ships==null || _playerSafe.SafeLinks.Ships.Count <= 1)
        {
            var army = base.CreateStartArmy(player);
            _playerSafe.SafeLinks.SetArmy(army);
        }

        return _playerSafe.SafeLinks.Ships;
    }
    public override Player CreatePlayer()
    {
        return _playerSafe;
    }

    public override int GetStartPower()
    {
        return _startPower;
    }
}
