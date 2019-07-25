using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TryChargeButton : UIElementWithTooltip
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
//        if (_nextClickTime < Time.time)
//        {
//            _nextClickTime = Time.time +
            if (!_ship.Commander.TryRecharge(_ship))
            {
                FailObject.SetTrigger("Play");
            }
//        }
    }

    public void Dispose()
    {
        _ship = null;
    }

    protected override string TextToTooltip()
    {
        return String.Format(Namings.RechargeButton,Library.COINS_TO_CHARGE_SHIP_SHIELD,Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY);
    }
}

