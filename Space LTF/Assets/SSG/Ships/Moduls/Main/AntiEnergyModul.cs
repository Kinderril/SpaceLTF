﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AntiEnergyModul : AntiWeaponModul
{

    private const float DELAY_BASE = 5;
    private const float DELAY_DELTA = 2;
    public AntiEnergyModul(BaseModulInv b) 
        : base(b)
    {
        _damageType = BulletDamageType.energy;
    }


    protected override float Delay()
    {
        return DELAY_BASE - ModulData.Level * DELAY_DELTA;
    }

    protected override BulletKiller GetEffect()
    {
        return DataBaseController.Instance.SpellDataBase.AntiEnergyEffect;
    }
}

