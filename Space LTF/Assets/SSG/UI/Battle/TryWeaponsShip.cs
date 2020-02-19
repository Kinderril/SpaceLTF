using UnityEngine;


public class TryWeaponsShip : TryApplyToShip
{
    public Animator FailObject;
    private float _nextClickTime;


    // public void OnTryPowerChargeClick()
    // {
    //
    //     if (!IsReady)
    //     {
    //         FailObject.SetTrigger("Play");
    //         return;
    //     }
    //
    //     if (_ship.Commander.TryWeaponBuffShip(_ship))
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
        return Namings.Format(Namings.Tag("PowerWeaponButton"), Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD, Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD_DELAY);
    }
}

