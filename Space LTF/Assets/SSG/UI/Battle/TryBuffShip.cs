using System;
using UnityEngine;


public class TryBuffShip : TryApplyToShip
{
    public Animator FailObject;
    private float _nextClickTime;


    public void OnTryChargeClick()
    {
        if (!IsReady)
        {
            FailObject.SetTrigger("Play");
            return;
        }

        if (_ship.Commander.TryBuffShip(_ship))
        {
            StartCooldown();
        }
        else
        {
            FailObject.SetTrigger("Play");
        }
    }

    public void Dispose()
    {
        _ship = null;
    }
    protected override string TextToTooltip()
    {
        return Namings.TryFormat(Namings.Tag("BuffButton"), Library.COINS_TO_CHARGE_SHIP_SHIELD, Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY);
    }
}

