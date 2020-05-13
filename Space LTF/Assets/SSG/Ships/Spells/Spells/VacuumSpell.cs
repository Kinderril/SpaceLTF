using UnityEngine;


[System.Serializable]
public class VacuumSpell : BaseSpellModulInv
{
    //A1 - more rad
    //B2 - less timer

    private const float DIST_SHOT = 50;
    private const float DAMAGE_BASE = 8;
    private const float radBase = 4f;
    private float shieldDmg => DAMAGE_BASE + Level * 2;
    private float powerThrow => 3 + Level * 0.8f;

    private float rad => RadCalc(Level, UpgradeType);

    private float RadCalc(int lvl, ESpellUpgradeType upg)
    {
        var a = radBase + lvl * 2;

        if (upg == ESpellUpgradeType.A1)
        {
            return a + 3;
        }
        return a;
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;

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
             new BulleStartParameters(25, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
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
        Commander commander = weapon.TeamIndex == TeamIndex.green
            ? BattleController.Instance.RedCommander
            : BattleController.Instance.GreenCommander;

        foreach (var obj in commander.Connectors)
        {
            AffectMovingObject(obj, origin.Position);
        }
        var bullets = BattleController.Instance.ActiveBullet;
        foreach (var bullet in bullets)
        {
            if (bullet.IsAcive)
            {
                AffectMovingObject(bullet, origin.Position);
            }
        }

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

    private void AffectMovingObject(MovingObject obj, Vector3 startPos)
    {
        var dir = obj.Position - startPos;
        var dist = dir.magnitude;
        if (dist < rad)
        {
            dir.y = 0f;
            var dirNorm = -Utils.NormalizeFastSelf(dir);
            var powerFoShip = powerThrow * 1.5f;
            obj.ExternalForce.Init(powerFoShip, 1f, dirNorm);
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
        return Namings.Format(Namings.Tag("DescVacuumSpell"), powerThrow, shieldDmg);
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
            var d = RadCalc(Library.SPECIAL_SPELL_LVL, ESpellUpgradeType.A1) -
                    RadCalc(Library.SPECIAL_SPELL_LVL, ESpellUpgradeType.None);
            return Namings.Format(Namings.Tag("VacuumDescA1"), d);
        }

        var d1 = _baseCostTime - _B2_costTime;
        return Namings.Format(Namings.Tag("VacuumDescB2"), d1);
    }
}

