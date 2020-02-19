using UnityEngine;


[System.Serializable]
public class ThrowAroundSpell : BaseSpellModulInv
{
    //A1 = lock engine
    //B2 = drop weapons load

    private const float DIST_SHOT = 50;
    private const float DAMAGE_BASE = 5;
    private const float rad = 4f;
    private const float timerToLockEngine = 3f;
    private float shieldDmg => DAMAGE_BASE + Level * 3;
    private float powerThrow => 7 + Level * 1.5f;

    public ThrowAroundSpell()
        : base(SpellType.throwAround, 2, 10,
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
    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Position, damagedone);
    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
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
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.throwAroundSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override BulletDestroyDelegate BulletDestroyDelegate => BulletDestroy;

    private void BulletDestroy(Bullet origin, IWeapon weapon, AICell cell)
    {
        Commander commander = weapon.TeamIndex == TeamIndex.green
            ? BattleController.Instance.RedCommander
            : BattleController.Instance.GreenCommander;

        foreach (var obj in commander.Connectors)
        {
            var dir = obj.Position - origin.Position;
            var dist = dir.magnitude;
            if (dist < rad)
            {
                dir.y = 0f;
                var dirNorm = Utils.NormalizeFastSelf(dir);
                var powerFoShip = powerThrow * 1.5f;
                obj.ExternalForce.Init(powerFoShip, 1f, dirNorm);
            }
        }

        var asteroids = cell.GetAllAsteroids();
        foreach (var aiAsteroidPredata in asteroids)
        {
            var dir = aiAsteroidPredata.Position - origin.Position;
            var dist = dir.magnitude;
            if (dist < rad)
            {
                var power = (1f - dist / rad) * powerThrow;
                power = MyExtensions.GreateRandom(power);
                aiAsteroidPredata.Push(dir, power);
            }
        }
    }

    protected override void CastAction(Vector3 pos)
    {
    }


    private void ActionShip(ShipBase shipBase, Vector3 fromPos, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(shieldDmg, 0, damageDoneCallback, shipBase);
        var dir = Utils.NormalizeFastSelf(shipBase.Position - fromPos);
        shipBase.ExternalForce.Init(powerThrow, 1f, dir);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                shipBase.DamageData.ApplyEffect(ShipDamageType.engine, timerToLockEngine);
                break;
            case ESpellUpgradeType.B2:
                shipBase.WeaponsController.UnloadAll();
                break;
        }

    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("TrowAroundSpell"), powerThrow, shieldDmg);
        //            $"Create a shockwave witch throw around all ships in radius with power {powerThrow}. And body damage {bodyDamage}.";
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("TrowAroundNameA1");
        }
        return Namings.Tag("TrowAroundNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Format(Namings.Tag("TrowAroundDescA1"), timerToLockEngine);
        }
        return Namings.Tag("TrowAroundDescB2");
    }
}

