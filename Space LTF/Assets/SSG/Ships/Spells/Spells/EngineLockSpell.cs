using UnityEngine;


[System.Serializable]
public class EngineLockSpell : BaseSpellModulInv
{
    //A1 - more rad
    //B2 - less timer

    public const float DIST_SHOT = 62f;
    public const float LOCK_PERIOD = 1;
    public const float LOCK_LEVEL = 0.5f;

    private float rad => GetRad(UpgradeType);

    private float GetRad(ESpellUpgradeType upd)
    {
        if (upd == ESpellUpgradeType.A1)
        {
            return 4;
        }
        return 2.5f;
    }
    // [NonSerialized]
    // private SpellZoneVisualCircle ObjectToShow;

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

    private const int _baseCostTime = 13;
    private const int _B2_costTime = 9;
    private float _lastBulletCreate = 0f;

    public float CurLockPeriod => LOCK_PERIOD + LOCK_LEVEL * Level;

    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(CurLockPeriod, CurLockPeriod);
    public EngineLockSpell()
        : base(SpellType.engineLock,  _baseCostTime,
            new BulleStartParameters(15, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {

        _localSpellDamageData =  new SpellDamageData(rad);
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        _lastBulletCreate = 0;
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    protected override void EndCastSpell()
    {
        _localSpellDamageData.AOERad = rad;
        base.EndCastSpell();
    }

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        var p = ship.DamageData.HaveDamage(ShipDamageType.engine);
        return !p;
    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;

    private void EngineCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        //        var startPos = target.Position + new Vector3(MyExtensions.Random(-rad, rad), DIST_SHOT, MyExtensions.Random(-rad, rad));
        var startPos = target.Position;
        var dir = target.Position - startPos + new Vector3(1f,0,1f);
        bullestartparameters.distanceShoot = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        var b = Bullet.Create(origin, weapon, dir, startPos, null, bullestartparameters);

    }
    public override BulletDestroyDelegate BulletDestroyDelegate => BulletDestroy;

    private void BulletDestroy(Bullet origin, IWeapon weapon, AICell cell)
    {

    }

    protected override CreateBulletDelegate standartCreateBullet => EngineCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => PeriodCast;


    // private float CoefSize()
    // {
    //
    //     var deltaTime = Time.time - _castStartTime;
    //     var coef = Mathf.Pow(deltaTime, 0.9f) + 1;
    //     float p = Mathf.Clamp(coef, 1, 5);
    //     return p;
    // }

    private void PeriodCast(Vector3 trgpos, BulletTarget target, Bullet origin, 
        IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {
        var p = PowerInc();
        _localSpellDamageData.AOERad = rad * p;
        var delta = Time.time - _lastBulletCreate;
        if (delta > CoinTempController.BATTERY_PERIOD)
        {
            _lastBulletCreate = Time.time;
            castdata.Bullestartparameters.size = castdata.Bullestartparameters.size * p;
            modificatedCreateBullet(target, origin, weapon, shootpos, castdata.Bullestartparameters);
            EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.EngineLockAOE,
                target.Position, 1f , p);
        }


    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        if (target != null)
        {
            var coef = PowerInc();
            ActionShip(target, additional.CurrentDamage.BodyDamage * coef);
        }
    }



    private void ActionShip(ShipBase shipBase,float period)
    {
        // var p = CoefSize();
        // var affectPeriod = period * p;
        shipBase.DamageData.ApplyEffect(ShipDamageType.engine, period);//.EngineStop.Stop(2.5f);
    }

    public override Bullet GetBulletPrefab()
    {
        // var bullet = DataBaseController.Instance.GetBullet(WeaponType.engineLockSpell);
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrame);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {

    }

    public override string Desc()
    {
        return Namings.Format(Namings.Tag("EnerguLockSpell"), CurLockPeriod.ToString("0"), rad.ToString("0"));
        //            $"Destroy engines for {CurLockPeriod.ToString("0")} sec.";
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("EngineLockNameA1");
        }
        return Namings.Tag("EngineLockNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            var delta = GetRad(ESpellUpgradeType.A1) - GetRad(ESpellUpgradeType.None);
            return Namings.Format(Namings.Tag("EngineLockDescA1"), delta);
        }

        var d = _baseCostTime - _B2_costTime;
        return Namings.Format(Namings.Tag("EngineLockDescB2"), d);
    }
}

