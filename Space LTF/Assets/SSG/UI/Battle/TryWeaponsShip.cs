﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TryWeaponsShip : UIElementWithTooltip
{
    public Animator FailObject;
    private ShipBase _ship;
    private float _nextClickTime;

    public void Init(ShipBase ship)
    {
        _ship = ship;
    }

    public void OnTryPowerChargeClick()
    {
        if (!_ship.Commander.TryWeaponBuffShip(_ship))
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
        return String.Format(Namings.PowerWeaponButton, Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD, Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD_DELAY);
    }
}

