
using System;
using UnityEngine;


[System.Serializable]
public class LineShotSpell : BaseSpellModulInv
{
    //A1 - No death
    //B2 - more fire

    private const float BULLET_SPEED = 10f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 38f;

    private const int FIRE_PERIOD = 6;

    private float FireCoef
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.B2)
            {
                return Level * 1.6f;
            }
            return Level;
        }
    }

    private int FirePeriod => FIRE_PERIOD + Level * 3;
    private int Damage => 5 + Level;

    private static CurWeaponDamage CurrentDamage { get; set; }

    public LineShotSpell()
        : base(SpellType.lineShot, 4, 15,
             new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false)
    {
        CurrentDamage = new CurWeaponDamage(Damage, Damage);
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var ANG_2 = 20f;

        var dirToShoot = target.Position - shootPos;
        //        Debug.Log($"dir to shoot{dirToShoot}   targte{target.Position}   from{shootPos}");

        var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, bullestartparameters);
        var half = ANG_2 / 2f;

        var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
        var r2 = Utils.RotateOnAngUp(dirToShoot, half);

        var b1 = Bullet.Create(origin, weapon, r1, shootPos, target.target, bullestartparameters);
        var b2 = Bullet.Create(origin, weapon, r2, shootPos, target.target, bullestartparameters);


        if (UpgradeType == ESpellUpgradeType.A1)
        {
            b1.DeathOnHit = b2.DeathOnHit = b0.DeathOnHit = false;
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage, damagedone, target);
        target.DamageData.ApplyEffect(ShipDamageType.fire, FirePeriod, FireCoef);
    }

    public override bool ShowLine => true;
    public override float ShowCircle => -1;
    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var dirToShoot = target.Position - shootPos;
        var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, bullestartparameters);
    }

    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.linerShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {

    }

    public override string Desc()
    {
        var totalFireDamage = FirePeriod * FireCoef;
        var damageStr = totalFireDamage.ToString("0");
        return Namings.TryFormat(Namings.Tag("LineSHotSpell"), CurrentDamage.BodyDamage, CurrentDamage.ShieldDamage, FirePeriod, damageStr);
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("LineShotNameA1");
        }
        return Namings.Tag("LineShotNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("LineShotDescA1");
        }
        return Namings.Tag("LineShotDescB2");
    }
}

