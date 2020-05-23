using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VortexBattleEvent : SetterElementBattleEvent
{
    public VortexBattleEvent(BattleController battle) :
        base(battle, 1, 3,
            DataBaseController.Instance.DataStructPrefabs.Vortexs, EBattlefildEventType.Vortex)
    {

    }
}
