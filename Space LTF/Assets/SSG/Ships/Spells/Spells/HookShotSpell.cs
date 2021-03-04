using UnityEngine;


[System.Serializable]
public class HookShotSpell : BaseSpellModulInv
{
    //A1 - more rad
    //B2 - less timer

    private const float DIST_SHOT = 50;

    //    private const float DAMAGE_BASE = 8;
    private const float radBase = 3f;

    private const float enginePeriod = 2f;

    //    private const float MainCoef = 10;
    //    private const float CenterCoef = 1 / (MainCoef * MainCoef);

    //    private float TotalDamage => DAMAGE_BASE + Level * 2;
    private float powerThrow => 0.84f;//3 * 0.28f;
    private float _lastBulletCreate = 0;//3 * 0.28f;

    private float CalcPower(float x)
    {
        return Mathf.Log(x * 0.03f + 0.2f) * 10 + 10;
    }

    private float rad => RadCalc(Level, UpgradeType);

    private float RadCalc(int lvl, ESpellUpgradeType upg)
    {
        var a = radBase + lvl * 2;

        if (upg == ESpellUpgradeType.A1) return a + 3;
        return a;
    }

    public override int CostTime
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.B2) return _B2_costTime;
            return _baseCostTime;
        }
    }
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(rad, rad);

    private const int _baseCostTime = 10;
    private const int _B2_costTime = 7;

    public HookShotSpell()
        : base(SpellType.hookShot,  _baseCostTime,
            new BulleStartParameters(25, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData(rad);
    }

    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos,
        CastSpellData castData)
    {
        var period = 0.5f;
        for (int i = 0; i < castData.ShootsCount; i++)
        {
            var pp = i * period;
            if (pp > 0)
            {
                var timer =
                    MainController.Instance.BattleTimerManager.MakeTimer(pp);
                timer.OnTimer += () =>
                {
                    modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
                };
            }
            else
            {
                modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
            }
        }
    }

    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => PeriodCast;
    public override bool ShowLine => false;
    public override float ShowCircle => rad;

    [System.NonSerialized] private Vector3 _lastTragetPosition;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Weapon.CurPosition, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos,
        BulleStartParameters bullestartparameters)
    {
        _lastTragetPosition = target.Position;
        var dir = target.Position - weapon.CurPosition;
        var d = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        bullestartparameters.distanceShoot = d;
        bullestartparameters.radiusShoot = d;
        var b = Bullet.Create(origin, weapon, dir, weapon.CurPosition, null, bullestartparameters);
    }

    private void PeriodCast(Vector3 trgpos, BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {

        var delta = Time.time - _lastBulletCreate;
        if (delta > CoinTempController.BATTERY_PERIOD)
        {
            _lastBulletCreate = Time.time;
            modificatedCreateBullet(target, origin, weapon, shootpos, castdata.Bullestartparameters);
            // EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.EngineLockAOE, origin.Position, 1f);
        }


    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrame);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        if (info != null && info.Dist > 4)
        {
            return true;
        }

        return false;
    }

    public override BulletDestroyDelegate BulletDestroyDelegate => BulletDestroy;


    private bool PushShip(MovingObject obj, Vector3 posToHook, out float pushedDist)
    {
        var dirToRad = _lastTragetPosition - obj.Position;
        var dist = dirToRad.magnitude;
        //        Debug.LogError($"Push my ship dist{dist}   rad:{rad}");
        if (dist < rad)
        {
            var dir = obj.Position - posToHook;
            dist = dir.magnitude;
            if (dist > 1)
            {
                var dirNorm = -Utils.NormalizeFastSelf(dir);
                var powerFoShip = GetShipPower(dist);
                pushedDist = dist;
                var effert = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.HookShot, obj.Position, 3f);
                TurnToDir(effert, dirNorm);
                obj.ExternalForce.Init(powerFoShip, GetDelay(dist), dirNorm);
                return true;
            }
        }

        pushedDist = 0f;
        return false;
    }

    private void BulletDestroy(Bullet origin, IWeapon weapon, AICell cell)
    {
        var commanderConnectors = weapon.TeamIndex == TeamIndex.green
            ? BattleController.Instance.RedCommander
            : BattleController.Instance.GreenCommander;

        var commanderSelfShips = weapon.TeamIndex == TeamIndex.green
            ? BattleController.Instance.GreenCommander
            : BattleController.Instance.RedCommander;

        var posToHook = weapon.CurPosition;
        var pushedDist = 0f;
        foreach (var obj in commanderSelfShips.Ships.Values)
        {
            if (PushShip(obj, posToHook, out pushedDist))
            {

                obj.DamageData.ApplyEffect(ShipDamageType.engine, GetDelay(pushedDist));
            }
        }

        foreach (var obj in commanderConnectors.Connectors)
            PushShip(obj, posToHook, out pushedDist);


        var bullets = BattleController.Instance.ActiveBullet;
        foreach (var bullet in bullets)
        {
            if (bullet.IsAcive)
            {
                PushShip(bullet, origin.Position, out var pushedDist2);
            }
        }

        var cellToTest = BattleController.Instance.CellController.GetCell(_lastTragetPosition);
        //        Debug.DrawRay(_lastTragetPosition,Vector3.up,Color.magenta,10);
        if (cellToTest != null)
        {
            var asteroids = cellToTest.GetAllAsteroids();
            foreach (var aiAsteroidPredata in asteroids)
            {
                var distTest = (_lastTragetPosition - aiAsteroidPredata.Position).magnitude;
                if (distTest < rad)
                {
                    var dir = aiAsteroidPredata.Position - posToHook;
                    dir.y = 0f;
                    var dist = dir.magnitude;
                    var power = dist * powerThrow;
                    power = MyExtensions.GreateRandom(power);
                    var effert = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.HookShot, aiAsteroidPredata.Position, 3f);
                    TurnToDir(effert, -dir);
                    aiAsteroidPredata.Push(-dir, power);
                }
            }
        }
    }

    private void TurnToDir(BaseEffectAbsorber effect, Vector3 dir)
    {
        effect.transform.rotation = Quaternion.LookRotation(dir);
    }

    protected override void CastAction(Vector3 pos)
    {
    }


    private void ActionShip(ShipBase shipBase, Vector3 fromPos, DamageDoneDelegate damageDoneCallback)
    {
        //        shipBase.ShipParameters.Damage(TotalDamage, 0, damageDoneCallback, shipBase);
        var dir2 = shipBase.Position - fromPos;
        dir2.y = 0f;
        var dist = dir2.magnitude;
        var dir = -Utils.NormalizeFastSelf(dir2);
        var powerFoShip = GetShipPower(dist);
        var delay = GetDelay(dist);
        shipBase.DamageData.ApplyEffect(ShipDamageType.engine, delay);
        var effert = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.HookShot, shipBase.Position, 3f);
        TurnToDir(effert, dir);

        var delta = Time.time - _castStartTime;
        var coef = Mathf.Clamp(Mathf.Pow(delta, 0.6f) * 0.25f, 0, 10);
        powerFoShip *= coef;
        shipBase.ExternalForce.Init(powerFoShip, delay, dir);
    }

    private float GetDelay(float dist)
    {
        return dist * 0.15f;
    }

    private float GetShipPower(float dist)
    {
        return CalcPower(dist);
        //              return powerThrow * 0.2f * dist;
    }

    public override string Desc()
    {
        return Namings.Format(Namings.Tag("DescHookShotSpell"), enginePeriod);
    }

    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1) return Namings.Tag("HookShotNameA1");
        return Namings.Tag("HookShotNameB2");
    }

    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            var d = RadCalc(Library.SPECIAL_SPELL_LVL, ESpellUpgradeType.A1) -
                    RadCalc(Library.SPECIAL_SPELL_LVL, ESpellUpgradeType.None);
            return Namings.Format(Namings.Tag("HookShotDescA1"), d);
        }

        var d1 = _baseCostTime - _B2_costTime;
        return Namings.Format(Namings.Tag("HookShotDescB2"), d1);
    }
}