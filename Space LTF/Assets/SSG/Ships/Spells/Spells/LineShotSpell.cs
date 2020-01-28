
using System;
using UnityEngine;


[System.Serializable]
public class LineShotSpell : BaseSpellModulInv
{
    private const float BULLET_SPEED = 10f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 38f;

    private const int FIRE_PERIOD = 4;

    private int FirePeriod => FIRE_PERIOD + Level * 2;

    private static CurWeaponDamage CurrentDamage { get; set; }

    public LineShotSpell(int costCount, int costTime)
        : base(SpellType.lineShot, costCount, costTime,
             new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false)
    {
        CurrentDamage = new CurWeaponDamage(2, 4);
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
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage, damagedone, target);
        target.DamageData.ApplyEffect(ShipDamageType.fire, FirePeriod);
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
        return String.Format(Namings.LineSHotSpell, CurrentDamage.BodyDamage, CurrentDamage.ShieldDamage, FirePeriod);
        //            $"Triple shot by selected direction. Base damage: {CurrentDamage.BodyDamage}/{CurrentDamage.ShieldDamage}. And starts fire for {FirePeriod} sec.";
    }
}

