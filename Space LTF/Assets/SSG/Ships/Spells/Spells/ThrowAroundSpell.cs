using System;
using UnityEngine;


[System.Serializable]
public class ThrowAroundSpell : BaseSpellModulInv
{
    private const float DIST_SHOT = 18;
    private const float DAMAGE_BASE = 12;
    private const float rad = 4f;
    private float shieldDmg => DAMAGE_BASE + Level * 4;
    private float powerThrow => 7 + Level * 2;

    public ThrowAroundSpell()
        : base(SpellType.throwAround, 2, 15,
             new BulleStartParameters(19.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        MainCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }
    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override bool ShowLine => true;
    public override float ShowCircle => rad;
    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Position, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        //        var startPos = target.Position + new Vector3(MyExtensions.Random(-rad, rad), DIST_SHOT, MyExtensions.Random(-rad, rad));
        var startPos = shootpos;
        var dir = target.Position - startPos;
        bullestartparameters.distanceShoot = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        var b = Bullet.Create(origin, weapon, dir, startPos, null, bullestartparameters);
        //        var b = Bullet.Create(origin, weapon, target.Position - weapon.CurPosition, weapon.CurPosition, null, bullestartparameters);
    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.throwAroundSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
    }


    private void ActionShip(ShipBase shipBase, Vector3 fromPos, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(shieldDmg, 0, damageDoneCallback, shipBase);
        var dir = Utils.NormalizeFastSelf(shipBase.Position - fromPos);
        shipBase.ExternalForce.Init(powerThrow, 1f, dir);

    }
    public override string Desc()
    {
        return String.Format(Namings.TrowAroundSpell, powerThrow, shieldDmg);
        //            $"Create a shockwave witch throw around all ships in radius with power {powerThrow}. And body damage {bodyDamage}.";
    }
}

