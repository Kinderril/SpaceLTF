using UnityEngine;


public class TryChargeButton : TryApplyToShip
{
    public Animator FailObject;
    private float _nextClickTime;


    // public void OnTryChargeClick()
    // {
    //
    //     if (!IsReady)
    //     {
    //         FailObject.SetTrigger("Play");
    //         return;
    //     }
    //
    //     if (_ship.Commander.TryRechargeShield(_ship))
    //     {
    //         StartCooldown();
    //     }
    //     else
    //     {
    //         FailObject.SetTrigger("Play");
    //     }
    // }

    public void Dispose()
    {
        _ship = null;
    }

    protected override string TextToTooltip()
    {
        return Namings.Format(Namings.Tag("RechargeButton"), Library.COINS_TO_CHARGE_SHIP_SHIELD, Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY);
    }
}

