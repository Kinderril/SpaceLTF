using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VortexBattleEvent : SetterElementBattleEvent
{
    public VortexBattleEvent(BattleController battle,float insideRad) :
        base(battle, 1, 2,
            DataBaseController.Instance.DataStructPrefabs.Vortexs, EBattlefildEventType.Vortex, insideRad)
    {

    }
}
