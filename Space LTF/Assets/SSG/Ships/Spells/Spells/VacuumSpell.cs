using System;
using UnityEngine;


[System.Serializable]
public class VacuumSpell : BaseSpellModulInv
{
    //A1 - more rad
    //B2 - less timer

    private const float DIST_SHOT = 24;
    private const float DAMAGE_BASE = 10;
    private const float radBase = 4f;
    private float shieldDmg => DAMAGE_BASE + Level;
    private float powerThrow => 3 + Level * 0.5f;

    private float rad
    {
        get
        {
            var a = radBase + Level * 2;

            if (UpgradeType == ESpellUpgradeType.A1)
            {
                return a + 3;
            }
            return a;
        }
    }
    public override int CostTime
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.B2)
            {
                return _B2_costTime;
            }
            return _baseCostTime;
        }
    }
    private const int _baseCostTime = 10;
    private const int _B2_costTime = 7;

    public VacuumSpell()
        : base(SpellType.vacuum, 2, _baseCostTime,
             new BulleStartParameters(25, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
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
    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Position, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {

        var dir = target.Position - weapon.CurPosition;
        var d = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        bullestartparameters.distanceShoot = d;
        bullestartparameters.radiusShoot = d;
        var b = Bullet.Create(origin, weapon, dir, weapon.CurPosition, null, bullestartparameters);
    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.vacuumdSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override BulletDestroyDelegate BulletDestroyDelegate => BulletDestroy;

    private void BulletDestroy(Bullet origin, IWeapon weapon, AICell cell)
    {
        var asteroids = cell.GetAllAsteroids();
        foreach (var aiAsteroidPredata in asteroids)
        {
            var dir = aiAsteroidPredata.Position - origin.Position;
            var dist = dir.magnitude;
            if (dist < rad)
            {
                var power = (dist / rad) * powerThrow;
                power = MyExtensions.GreateRandom(power);
                aiAsteroidPredata.Push(-dir, power);
            }
        }
    }

    protected override void CastAction(Vector3 pos)
    {
    }


    private void ActionShip(ShipBase shipBase, Vector3 fromPos, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(shieldDmg, 0, damageDoneCallback, shipBase);
        var dir2 = shipBase.Position - fromPos;
        dir2.y = 0f;
        var dir = -Utils.NormalizeFastSelf(dir2);
        var powerFoShip = powerThrow * 1.5f;
        shipBase.ExternalForce.Init(powerFoShip, 1f, dir);

    }
    public override string Desc()
    {
        return Namings.TryFormat(Namings.Tag("DescVacuumSpell"), powerThrow, shieldDmg);
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("VacuumNameA1");
        }
        return Namings.Tag("VacuumNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("VacuumDescA1");
        }
        return Namings.Tag("VacuumDescB2");
    }
}

