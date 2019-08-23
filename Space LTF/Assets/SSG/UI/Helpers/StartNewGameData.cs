using System.Collections.Generic;
using UnityEngine;

public class StartNewGameData
{
    public int BasePower;
    public int CellsStartDeathStep;
    public int CoreElementsCount;
    public int SectorSize;
    public int SectorCount;
    public List<WeaponType> posibleStartWeapons;
    public ShipConfig shipConfig;
    public SpellType posibleSpell;
    public Dictionary<PlayerParameterType, int> startParametersLevels;

    public StartNewGameData(Dictionary<PlayerParameterType, int> startParametersLevels,
        ShipConfig shipConfig, List<WeaponType> posibleStartWeapons, int SectorSize,  int SectorCount,
        int CellsStartDeathStep, int CoreElementsCount, int BasePower, SpellType posibleSpell)
    {
        Debug.Log(($"StartNewGameData {shipConfig.ToString()} SectorSize:{SectorSize} " +
                  $" SectorCount:{SectorCount} CellsStartDeathStep:{CellsStartDeathStep}  CoreElementsCount:{CoreElementsCount}" +
                  $"  BasePower:{BasePower}   posibleSpell:{posibleSpell}").Red());
        this.startParametersLevels = startParametersLevels;
        this.shipConfig = shipConfig;
        this.SectorSize = SectorSize;
        this.SectorCount = SectorCount;
        this.CoreElementsCount = CoreElementsCount;
        this.CellsStartDeathStep = CellsStartDeathStep;
        this.posibleStartWeapons = posibleStartWeapons;
        this.BasePower = BasePower;
        this.posibleSpell = posibleSpell;
    }
}