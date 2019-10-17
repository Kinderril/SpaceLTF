using System.Collections.Generic;
using UnityEngine;

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

    public StartNewGameData(Dictionary<PlayerParameterType, int> startParametersLevels,
        ShipConfig shipConfig, List<WeaponType> posibleStartWeapons, int SectorSize,  int SectorCount,
        int stepsBeforeDeath, int CoreElementsCount, int BasePower, List<SpellType> posibleSpell)
    {
        Debug.Log(($"StartNewGameData {shipConfig.ToString()} SectorSize:{SectorSize} " +
                  $" SectorCount:{SectorCount} StepsBeforeDeath:{stepsBeforeDeath}  CoreElementsCount:{CoreElementsCount}" +
                  $"  BasePower:{BasePower}   posibleSpell:{posibleSpell}").Red());
        this.startParametersLevels = startParametersLevels;
        this.shipConfig = shipConfig;
        this.SectorSize = SectorSize;
        this.SectorCount = SectorCount;
        this.CoreElementsCount = CoreElementsCount;
        this.StepsBeforeDeath = stepsBeforeDeath;
        this.posibleStartWeapons = posibleStartWeapons;
        this.BasePower = BasePower;
        this.posibleSpell = posibleSpell;
    }

    public float CalcDifficulty()
    {
        var deltaPower = (BasePower - Library.MAX_GLOBAL_MAP_VERYEASY_BASE_POWER) * 0.2f;
        var deltaSize = (SectorSize - Library.MIN_GLOBAL_SECTOR_SIZE) * 0.05f;
        var deltaCore = (CoreElementsCount - Library.MIN_GLOBAL_MAP_CORES) * 0.12f;

        var deltaDeath = (Library.MAX_GLOBAL_MAP_DEATHSTART - StepsBeforeDeath) * 0.08f;

        return deltaDeath + deltaCore + deltaSize + deltaPower - 0.01f;
    }
}