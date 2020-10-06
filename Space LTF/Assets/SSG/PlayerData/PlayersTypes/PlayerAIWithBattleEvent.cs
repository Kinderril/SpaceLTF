using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAIWithBattleEvent : PlayerAI, IPlayerAIWithBattleEvent
{

    public BattleTypeData BattleTypeData { get; private set; } 
    private static
        List<EBattleType> posibeRndTypes = new List<EBattleType>() { EBattleType.baseDefence,
            EBattleType.defenceOfShip, EBattleType.defenceWaves, EBattleType.destroyShipPeriod };

    public PlayerAIWithBattleEvent(string name,bool posibleEvent, BattleTypeData eBattleType = null)
        : base(name)
    {
        if (eBattleType == null)
        {
            if (posibleEvent && MyExtensions.IsTrue01(.33f))
            {
                BattleTypeData = new BattleTypeData(posibeRndTypes.RandomElement());
            }
            else
            {
                BattleTypeData = new BattleTypeData();
            }
        }
        else
        {
            BattleTypeData = eBattleType;
        }

#if UNITY_EDITOR
//        EBattleType = EBattleType.baseDefence;
#endif
    }
}
