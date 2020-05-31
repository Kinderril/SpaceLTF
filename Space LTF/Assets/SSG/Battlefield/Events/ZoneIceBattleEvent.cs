using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneIceBattleEvent : SetterElementBattleEvent
{
    public ZoneIceBattleEvent(BattleController battle,float insideRad) :
        base(battle, 1, 2,
            DataBaseController.Instance.DataStructPrefabs.IceZone, EBattlefildEventType.IceZone, insideRad)
    {

    }
}
