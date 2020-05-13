﻿using System.Collections.Generic;
using UnityEngine;

public enum EGameMode
{
                 sandBox,
//                 sandBox,
                 simpleTutor,
                 advTutor,
}

public class StartNewGameData
{
    public int BasePower;
    public int StepsBeforeDeath;
    public int CoreElementsCount;
    public int SectorSize;
    public int SectorCount;
    public List<WeaponType> posibleStartWeapons;
    public ShipConfig shipConfig;
    public List<SpellType> posibleSpell;
    public Dictionary<PlayerParameterType, int> startParametersLevels;
    public int PowerPerTurn;
    public EGameMode GameNode;

    public StartNewGameData(Dictionary<PlayerParameterType, int> startParametersLevels,
        ShipConfig shipConfig, List<WeaponType> posibleStartWeapons, int SectorSize, int SectorCount,
        int stepsBeforeDeath, int CoreElementsCount, int BasePower, List<SpellType> posibleSpell, int PowerPerTurn, EGameMode gameNode)
    {
        Debug.Log(($"StartNewGameData {shipConfig.ToString()} SectorSize:{SectorSize} " +
                  $" SectorCount:{SectorCount} StepsBeforeDeath:{stepsBeforeDeath}  CoreElementsCount:{CoreElementsCount}" +
                  $"  BasePower:{BasePower}  PowerPerTurn:{PowerPerTurn}   posibleSpell:{posibleSpell}").Red());
        this.startParametersLevels = startParametersLevels;
        this.shipConfig = shipConfig;
        this.SectorSize = SectorSize;
        this.SectorCount = SectorCount;
        this.CoreElementsCount = CoreElementsCount;
        this.StepsBeforeDeath = 999;
        this.posibleStartWeapons = posibleStartWeapons;
        this.BasePower = BasePower;
        this.posibleSpell = posibleSpell;
        this.PowerPerTurn = PowerPerTurn;
        GameNode = gameNode;
    }


    public float CalcDifficulty()
    {
        var deltaPower = (BasePower - Library.MAX_GLOBAL_MAP_VERYEASY_BASE_POWER) * 0.2f;
        var deltaSize = (SectorSize - Library.MIN_GLOBAL_SECTOR_SIZE) * 0.05f;
        var deltaCore = (CoreElementsCount - Library.MIN_GLOBAL_MAP_CORES) * 0.12f;

        // var deltaDeath = (Library.MAX_GLOBAL_MAP_DEATHSTART - StepsBeforeDeath) * 0.08f;
        var powerPerTurn = (PowerPerTurn - Library.MIN_GLOBAL_MAP_ADDITIONAL_POWER) * 0.1f;

        return deltaCore + deltaSize + deltaPower + powerPerTurn;
    }
}