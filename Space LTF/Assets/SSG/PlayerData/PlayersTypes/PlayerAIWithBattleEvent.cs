using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAIWithBattleEvent : PlayerAI, IPlayerAIWithBattleEvent
{

    public EBattleType EBattleType { get; private set; } = EBattleType.standart;
    private static
        List<EBattleType> posibeRndTypes = new List<EBattleType>() { EBattleType.baseDefence, EBattleType.defenceOfShip, EBattleType.defenceWaves, EBattleType.destroyShipPeriod };

    public PlayerAIWithBattleEvent(string name, Dictionary<PlayerParameterType, int> startData = null)
        : base(name, startData)
    {
        //        if (startCell.indX > 4)
        //        {
        if (MyExtensions.IsTrue01(.33f))
        {
            EBattleType = posibeRndTypes.RandomElement();
        }
        //        }
#if UNITY_EDITOR
        EBattleType = EBattleType.baseDefence;
#endif
    }
}
