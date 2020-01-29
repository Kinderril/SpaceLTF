
using System;
using UnityEngine;


[System.Serializable]
public class DistShotSpell : BaseSpellModulInv
{
    //    private const float dist = 28f;

    private const int DIST_BASE_DAMAGE = 8;
    private const int BASE_DAMAGE = 18;
    private const int LEVEL_DAMAGE = 8;
    private const float DIST_COEF = 0.8f;
    private const float ENGINE_OFF_DELTA = 3f;
    private const float ENGINE_OFF_LEVEL = 1f;

    [NonSerialized]
    private CurWeaponDamage CurWeaponDamage;

    private const float BULLET_SPEED = 13f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 34f;
    public DistShotSpell()
        : base(SpellType.distShot, 5, 20, new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false)
    {
        CurWeaponDamage = new CurWeaponDamage(0, 12);
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        DistShotCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }

    private void DistShotCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, target.Position - shootpos, shootpos, null, bullestartparameters);
    }

    public int BASE_damage => BASE_DAMAGE + LEVEL_DAMAGE * Level;
    public float Engine_Off => ENGINE_OFF_DELTA + ENGINE_OFF_LEVEL * Level;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var dist = (target.Position - bullet1.Weapon.Owner.Position).magnitude;
        var c = dist * DIST_COEF;
        int damage = BASE_damage + Mathf.Clamp((int)c, 0, DIST_BASE_DAMAGE);
        //        int baseSpDamage = (int)(BASE_DAMAGE / c);

        target.ShipParameters.Damage(0, damage, bullet1.Weapon.DamageDoneCallback, target);
        if (HaveSpecial)
            target.DamageData.ApplyEffect(ShipDamageType.engine, Engine_Off);
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.distShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }
    public override bool ShowLine => true;
    public override float ShowCircle => -1;

    protected override CreateBulletDelegate createBullet => DistShotCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    protected override void CastAction(Vector3 pos)
    {

    }
    public override string Desc()
    {
        if (HaveSpecial)
        {
            return String.Format(Namings.Tag("DistShotSpellSpecial"), BASE_damage, Engine_Off.ToString("0.0"));
        }
        else
        {
            return String.Format(Namings.Tag("DistShotSpell"), BASE_damage, Engine_Off.ToString("0.0"));
        }
        //            $"Single bullet. Base damage {BASE_damage}. Additional damage dependence on distance."; 
    }

}

