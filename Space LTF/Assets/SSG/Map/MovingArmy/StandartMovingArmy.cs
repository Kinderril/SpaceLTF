using System;
using UnityEngine;
using System.Collections;

public class StandartMovingArmy : MovingArmy
{
    public StandartMovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback) 
        : base(startCell, destroyCallback)
    {

    }
}
