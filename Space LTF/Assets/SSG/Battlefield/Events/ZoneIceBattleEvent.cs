using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneIceBattleEvent : SetterElementBattleEvent
{
    public ZoneIceBattleEvent(BattleController battle) :
        base(battle, 1, 2,
            DataBaseController.Instance.DataStructPrefabs.IceZone, EBattlefildEventType.IceZone)
    {

    }
}
