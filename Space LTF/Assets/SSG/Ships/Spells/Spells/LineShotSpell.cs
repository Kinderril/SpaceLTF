
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class LineShotSpell : BaseSpellModulInv 
{                                 
    private const float BULLET_SPEED = 10f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 38f;
    private static CurWeaponDamage CurrentDamage { get; set; }

    public LineShotSpell(int costCount, int costTime)
        : base(SpellType.lineShot, costCount, costTime,
            MainCreateBullet, MainAffect, new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false)
    {
        CurrentDamage = new CurWeaponDamage(1,2);
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(CurrentDamage.ShieldDamage, CurrentDamage.BodyDamage, damagedone,target);
        target.DamageData.ApplyEffect(ShipDamageType.fire,8);
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
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


//        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
//            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));
    }



    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.linerShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
        
    }
}

