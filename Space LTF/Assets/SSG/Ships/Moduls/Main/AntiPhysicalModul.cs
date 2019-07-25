using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AntiPhysicalModul : AntiWeaponModul
{

    private const float DELAY_BASE = 14;
    private const float DELAY_DELTA = 1;
    public AntiPhysicalModul(BaseModulInv b) 
        : base(b)
    {
        _damageType = BulletDamageType.physical;
    }

    protected override float Delay()
    {
        return DELAY_BASE - ModulData.Level * DELAY_DELTA;
    }

    protected override BulletKiller GetEffect()
    {
        return DataBaseController.Instance.SpellDataBase.AntiPhysicalEffect;
    }
}

