﻿using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponSelfDamageModul : BaseSupportModul
{

    private const int Damage = 2;
    private int Self => _self + Level;
    private const int _self = 1;
    public WeaponSelfDamageModul(int level)
        : base(SimpleModulType.WeaponSelfDamage, level)
    {
    }

    private int Dmg => Damage + Level;

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage += Dmg;
        weapon.CurrentDamage.ShieldDamage += Dmg;
    }

    protected override bool BulletImplement => true;

    protected override CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        CreateBulletDelegate delegat1 = (target, origin, weapon, pos, parameters) =>
            {
                weapon.Owner.ShipParameters.Damage(Self,Self, (delta, shieldDelta, attacker) => {},null);
                standartDelegate(target, origin, weapon, pos, parameters);
            };

        return delegat1;
    }

    public override string DescSupport()
    {
        return String.Format("Add a +{0}/+{0} Damage. But hit own ship when strikes on {1}/{1}", Dmg, Self);
    }




}
