
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using UnityEngine;


[System.Serializable]
public class DistShotSpell : BaseSpellModulInv
{
    //    private const float dist = 28f;

    private const int BASE_DAMAGE = 10;
    private const float DIST_COEF = .065f;

    [NonSerialized]
    private CurWeaponDamage CurWeaponDamage;


    private const float ANG_OFFSET = 8f;
    private const float BULLET_SPEED = 8f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 28f;
    public DistShotSpell(int costCount, int costTime)
        : base(SpellType.distShot, costCount, costTime  ,
            DistShotCreateBullet, MainAffect, new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT),false)
    {
        CurWeaponDamage = new CurWeaponDamage(1,5);
    }

    private static void DistShotCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {


        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var dist = (target.Position - bullet1.Weapon.Owner.Position).magnitude;
        var c = dist * DIST_COEF;
        int damage = Mathf.Clamp((int)(BASE_DAMAGE / c), 1, BASE_DAMAGE);
        //        int baseSpDamage = (int)(BASE_DAMAGE / c);

        target.ShipParameters.Damage(damage, damage, bullet1.Weapon.DamageDoneCallback,target);
        target.DamageData.ApplyEffect(ShipDamageType.fire,5);
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.distShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }


    public int Level
    {
        get { return 1; }
    }





    protected override void CastAction(Vector3 pos)
    {

     }
    
    public void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        
    }

}

