﻿using UnityEngine;


[System.Serializable]
public class MineFieldSpell : BaseSpellModulInv
{
    //A1 - Fire
    //B2 - Engine

    public const int BASE_MINES_COUNT = 3;
    public const float MINES_PERIOD = 20f;
    public const float MINES_DIST = 15f;
    private const float rad = 3.5f;
    private const float damageBody = 7f;
    private const float damageShield = 3f;

    private const float effectPeriod = 5f;

    private int MINES_COUNT => BASE_MINES_COUNT + Level;
    public float DAMAGE_BODY => damageBody + Level;
    public float DAMAGE_SHIELD => damageShield + Level;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(DAMAGE_SHIELD, DAMAGE_BODY);

    public MineFieldSpell()
        : base(SpellType.mineField, 2, 14,
             new BulleStartParameters(8f, 36f, MINES_DIST, MINES_DIST), false,TargetType.Enemy)
    {

    }
    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        var deltaAng = 360f / MINES_COUNT;
        var direction = MyExtensions.IsTrueEqual() ? Vector3.right : Vector3.left;
        var baseDist = (target.Position - weapon.CurPosition).magnitude;
        //        Debug.LogError($"Mine base dist {baseDist}");
        for (int i = 0; i < MINES_COUNT + castData.ShootsCount - 1; i++)
        {
            direction = Utils.RotateOnAngUp(direction, MyExtensions.GreateRandom((deltaAng * i)));
            var position = target.Position + direction * MyExtensions.Random(rad / 4, rad);
            var dir = (position - weapon.CurPosition);
            castData.Bullestartparameters.distanceShoot = baseDist;
            modificatedCreateBullet(new BulletTarget(dir + weapon.CurPosition), origin, weapon, shootPos, castData.Bullestartparameters);
        }
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;

    }
    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.Position - weapon.CurPosition);
        //        Debug.LogError("");                  
        var dist = dir.magnitude;
        //        Debug.LogError($"Mine result dist {dist}");
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, null,
                new BulleStartParameters(Library.MINE_SPEED, 0f, dist, dist));
    }

    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                target.DamageData.ApplyEffect(ShipDamageType.fire, effectPeriod, 1f);
                break;
            case ESpellUpgradeType.B2:
                target.DamageData.ApplyEffect(ShipDamageType.engine, effectPeriod, 1f);
                break;
        }
        shipparameters.Damage(DAMAGE_SHIELD, DAMAGE_BODY, damagedone, target);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;
    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.castMine);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
        //        int c = 4;

    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad, false);
    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("MinesSpell"), MINES_COUNT, MineFieldSpell.MINES_PERIOD.ToString("0"), DAMAGE_SHIELD, DAMAGE_BODY);
        //            $"Set {MinesCount} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location. Each mine damage {damageShield}/{damageBody}";
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("MinesSpellNameA1");
        }
        return Namings.Tag("MinesSpellNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Format(Namings.Tag("MinesSpellDescA1"), effectPeriod);
        }
        return Namings.Format(Namings.Tag("MinesSpellDescB2"), effectPeriod);
    }
}

