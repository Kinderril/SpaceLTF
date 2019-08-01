﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class RandomDamageSpell : BaseSpellModulInv
{
    public const float DIST_SHOT = 20f;
    public const float rad = 3f;

    public RandomDamageSpell(int costCount, int costTime)
        : base(SpellType.randomDamage, costCount, costTime,
            MainCreateBullet, CastSpell, MainAffect, new BulleStartParameters(9.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }
    private static void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        MainCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target);
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));
    }
    
    public override Bullet GetBulletPrefab()
    {

        var bullet = DataBaseController.Instance.GetBullet(WeaponType.randomDamage);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
//        var effect2 = DataBaseController.Instance.SpellDataBase.ShieldDamageEffectAOE;
//        EffectController.Instance.Create(effect2, pos, 3f);
//        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
//        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
//        foreach (var shipBase in c1)
//        {
//            ActionShip(shipBase);
//        }
//        foreach (var shipBase in c2)
//        {
//            ActionShip(shipBase);
//        }
    }


    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    private static void ActionShip(ShipBase shipBase)
    {
        float timeDelay = 13f;
        WDictionary< ShipDamageType > wd = new WDictionary<ShipDamageType>(new Dictionary<ShipDamageType, float>()
        {
            { ShipDamageType.engine,3f},
            { ShipDamageType.weapon,2f},
            { ShipDamageType.fire,3f},
            { ShipDamageType.shiled,2f}
        });

        switch (wd.Random())
        {
            case ShipDamageType.engine:
                shipBase.DamageData.ApplyEffect(ShipDamageType.engine, timeDelay);
                break;
            case ShipDamageType.weapon:
                shipBase.DamageData.ApplyEffect(ShipDamageType.weapon, timeDelay);
                break;
            case ShipDamageType.shiled:
                shipBase.DamageData.ApplyEffect(ShipDamageType.shiled, timeDelay);
                break;
            case ShipDamageType.fire:
                shipBase.DamageData.ApplyEffect(ShipDamageType.fire, timeDelay);
                break;
        }
    }


}

