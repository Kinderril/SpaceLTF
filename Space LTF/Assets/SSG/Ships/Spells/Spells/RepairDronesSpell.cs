﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class RepairDronesSpell : BaseSpellModulInv 
{
    public const int DRONES_COUNT = 3;
    public const float HEAL_PERCENT = 0.2f;
    public const float MINES_DIST = 5f;
    private const float rad = 3.5f;

    private float _distToShoot;

    private int DronesCount => DRONES_COUNT + Level;
    private float HealPercent => HEAL_PERCENT + Level/50f;

    private float dist;//Костыльный параметр
    public RepairDronesSpell(int costCount, int costTime)
        : base(SpellType.repairDrones, costCount, costTime,
             new BulleStartParameters(9.7f, 36f, MINES_DIST, MINES_DIST), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.Position - weapon.CurPosition);
        MainCreateBullet(new BulletTarget(dir + weapon.CurPosition), origin, weapon, shootPos, bullestartparameters);
        }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.Position - weapon.CurPosition);
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, null,  BulleStartParameters);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.HealHp(shipparameters.CurHealth * HealPercent);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;
    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.spellRerairDrone);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
//        int c = 4;

    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }
    public override string Desc()
    {
        return    String.Format(Namings.RepairDroneSpell, DronesCount, Utils.FloatToChance(HealPercent));
//            $"Set {MinesCount} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location. Each mine damage {damageShield}/{damageBody}";
    }
}

