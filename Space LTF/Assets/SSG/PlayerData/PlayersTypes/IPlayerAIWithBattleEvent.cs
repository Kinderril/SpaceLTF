using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class BattleTypeData
{
    public int AddToPlayerCount = 0;
    public int AddToEnemyCount = 0;
    public ShipConfig _addSelfConfig;
    public ShipConfig _addEnemyConfig;
    public BattleTypeData(EBattleType eBattleType = EBattleType.standart)
    {
        EBattleType = eBattleType;
    }

    public BattleTypeData(int addToPlayerCount, int addToEnemyCount, ShipConfig _addSelfConfig, ShipConfig _addEnemyConfig)
    {
        this._addSelfConfig = _addSelfConfig;
        this._addEnemyConfig = _addEnemyConfig;
        AddToPlayerCount = addToPlayerCount;
        AddToEnemyCount = addToEnemyCount;
        EBattleType = EBattleType.massiveFight;
    }
    public EBattleType EBattleType { get; private set; }
}

public interface IPlayerAIWithBattleEvent 
{
    BattleTypeData BattleTypeData { get;  }

}
