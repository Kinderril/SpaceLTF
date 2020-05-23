﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackHoleBattleEvent : SetterElementBattleEvent
{
    public BlackHoleBattleEvent(BattleController battle) :
        base(battle, 2, 2,
            DataBaseController.Instance.DataStructPrefabs.BlackHole, EBattlefildEventType.BlackHole)
    {

    }
}
