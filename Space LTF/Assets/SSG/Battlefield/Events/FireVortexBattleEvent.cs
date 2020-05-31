using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireVortexBattleEvent : SetterElementBattleEvent
{
    public FireVortexBattleEvent(BattleController battle, float insideRad) :
        base(battle, 1, 2,
            DataBaseController.Instance.DataStructPrefabs.FireVortexs, EBattlefildEventType.fireVortex, insideRad)
    {

    }
}
