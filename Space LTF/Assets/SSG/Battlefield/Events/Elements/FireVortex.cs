using UnityEngine;
using System.Collections;

public class FireVortex : ObjectElementBattleEvent
{
    protected override void Init()
    {
        _applyShip = true;
    }

    protected override void ExitEvent(ShipHitCatcher ship)
    {
        ship.ShipBase.PeriodDamage.StopConstantFire(1f);
    }

    protected override void EnterEvent(ShipHitCatcher ship)
    {
        ship.ShipBase.PeriodDamage.StartConstantFire(1f);
    }
}
