using UnityEngine;
using System.Collections;

public class IcePlace : ObjectElementBattleEvent
{
    protected override void Init()
    {
        _applyShip = true;
    }

    protected override void ExitEvent(ShipHitCatcher ship)
    {
        ship.ShipBase.BuffData.DeactivateCostBuff();
    }

    protected override void EnterEvent(ShipHitCatcher ship)
    {
        ship.ShipBase.BuffData.AddConstantSpeed(.4f);
    }
}
