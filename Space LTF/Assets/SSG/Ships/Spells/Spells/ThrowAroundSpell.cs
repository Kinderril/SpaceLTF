using UnityEngine;


[System.Serializable]
public class ThrowAroundSpell : SpellWithSizeCoef
{
    //A1 = lock engine
    //B2 = drop weapons load

    private const float DIST_SHOT = 50;
    private const float DAMAGE_BASE = 5;
    private float rad => 4f * SizeCoef();
    private const float powerAsteroidCoef = 2.25f;
    private const float timerToLockEngine = 3f;
    private float shieldDmg => DAMAGE_BASE + Level * 3;
    private float powerThrow => 7 + Level * 1.5f;
    private float _lastBulletCreate = 0f;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(shieldDmg, powerThrow);

    public ThrowAroundSpell()
        : base(SpellType.throwAround,  10,
             new BulleStartParameters(19.7f, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData(rad);
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        // var period = 0.5f;
        // for (int i = 0; i < castData.ShootsCount; i++)
        // {
        //     var pp = i * period;
        //     if (pp > 0)
        //     {
        //         var timer =
        //             MainController.Instance.BattleTimerManager.MakeTimer(pp);
        //         timer.OnTimer += () =>
        //         {
        //             modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
        //         };
        //     }
        //     else
        //     {
        //         modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
        //     }
        // }
    }
    private void PeriodCast(Vector3 trgpos, BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {

        var delta = Time.time - _lastBulletCreate;
        if (delta > CoinTempController.BATTERY_PERIOD)
        {
            _localSpellDamageData.AOERad = rad;
            _lastBulletCreate = Time.time;
            modificatedCreateBullet(target, origin, weapon, shootpos, castdata.Bullestartparameters);
        }


    }

    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;

    }
    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => PeriodCast;
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

    private void MainCreateBullet(BulletTarget target, 
        Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
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
                var power = (1f - dist / rad) * powerThrow * powerAsteroidCoef;
                power = MyExtensions.GreateRandom(power);
                aiAsteroidPredata.Push(dir, power);
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
            var dirNorm = Utils.NormalizeFastSelf(dir);
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

