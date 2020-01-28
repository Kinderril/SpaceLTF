using System;
using UnityEngine;


[System.Serializable]
public class ThrowAroundSpell : BaseSpellModulInv
{
    private const float DIST_SHOT = 8;
    private const float DAMAGE_BODY = 10;
    private const float rad = 4f;

    public ThrowAroundSpell(int costCount, int costTime)
        : base(SpellType.throwAround, costCount, costTime,
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
        //        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
        //        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
        //        foreach (var shipBase in c1)
        //        {
        //            ActionShip(shipBase, pos);
        //        }
        //        foreach (var shipBase in c2)
        //        {
        //            ActionShip(shipBase, pos);
        //        }
    }

    private float bodyDamage => DAMAGE_BODY + Level * 2;
    private float powerThrow => 7 + Level * 2;

    private void ActionShip(ShipBase shipBase, Vector3 fromPos, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(0, bodyDamage, damageDoneCallback, shipBase);
        var dir = Utils.NormalizeFastSelf(shipBase.Position - fromPos);
        shipBase.ExternalForce.Init(powerThrow, 1f, dir);

    }
    public override string Desc()
    {
        return String.Format(Namings.TrowAroundSpell, powerThrow, bodyDamage);
        //            $"Create a shockwave witch throw around all ships in radius with power {powerThrow}. And body damage {bodyDamage}.";
    }
}

