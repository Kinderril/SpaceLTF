using System;
using UnityEngine;


public class TryWaveShip : TryApplyToShip
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

        if (_ship.Commander.TryWave(_ship))
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
        return Namings.TryFormat(Namings.Tag("RoundWaveStrike"), Library.COINS_TO_WAVE_SHIP,
            Library.COINS_TO_WAVE_SHIP_DELAY, WeaponRoundWaveStrike.SHIELD_DAMAGE, WeaponRoundWaveStrike.BODY_DAMAGE);
    }
}

