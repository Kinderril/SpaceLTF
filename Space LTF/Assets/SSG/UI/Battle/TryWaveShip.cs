using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TryWaveShip : UIElementWithTooltip
{
    public Animator FailObject;
    private ShipBase _ship;
    private float _nextClickTime;

    public void Init(ShipBase ship)
    {
        _ship = ship;
    }

    public void OnTryChargeClick()
    {
        if (!_ship.Commander.TryWave(_ship))
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
        return String.Format(Namings.RoundWaveStrike,Library.COINS_TO_WAVE_SHIP,
            Library.COINS_TO_WAVE_SHIP_DELAY, WeaponRoundWaveStrike.SHIELD_DAMAGE, WeaponRoundWaveStrike.BODY_DAMAGE);
    }
}

