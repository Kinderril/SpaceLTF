using System.Collections.Generic;
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
    public EStartGameDifficulty Difficulty;
    public int StepsBeforeDeath;
    public int QuestsOnStart;
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
        int stepsBeforeDeath, int questsOnStart, EStartGameDifficulty difficulty, List<SpellType> posibleSpell
        , int PowerPerTurn, EGameMode gameNode)
    {
        Debug.Log(($"StartNewGameData {shipConfig.ToString()} SectorSize:{SectorSize} " +
                  $" SectorCount:{SectorCount} StepsBeforeDeath:{stepsBeforeDeath}  questsOnStart:{questsOnStart}" +
                  $"  Difficulty:{Difficulty}  PowerPerTurn:{PowerPerTurn}   posibleSpell:{posibleSpell}").Red());
        this.startParametersLevels = startParametersLevels;
        this.shipConfig = shipConfig;
        this.SectorSize = SectorSize;
        this.SectorCount = SectorCount;
        this.QuestsOnStart = questsOnStart;
        this.StepsBeforeDeath = 999;
        this.posibleStartWeapons = posibleStartWeapons;
        this.Difficulty = difficulty;
        this.posibleSpell = posibleSpell;
        this.PowerPerTurn = PowerPerTurn;
        GameNode = gameNode;
    }


    public float CalcDifficulty()
    {
        var deltaPower = ((int)(Difficulty) + 1) * 0.2f;
        var deltaSize = (SectorSize - Library.MIN_GLOBAL_SECTOR_SIZE) * 0.05f;
        var deltaCore = (QuestsOnStart - Library.MIN_GLOBAL_MAP_QUESTS) * 0.12f;

        // var deltaDeath = (Library.MAX_GLOBAL_MAP_DEATHSTART - StepsBeforeDeath) * 0.08f;
        var powerPerTurn = (PowerPerTurn - Library.MIN_GLOBAL_MAP_ADDITIONAL_POWER) * 0.1f;

        return deltaCore + deltaSize + deltaPower + powerPerTurn;
    }
}