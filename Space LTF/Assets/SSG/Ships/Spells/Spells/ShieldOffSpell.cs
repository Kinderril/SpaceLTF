using UnityEngine;


[System.Serializable]
public class ShieldOffSpell : BaseSpellModulInv
{
    //A1 - -1 battly
    //B2 - fire on

    public const float PERIOD = 13f;
    private const float SHIELD_DAMAGE = 3f;
    private const float rad = 3.5f;
    private const float DIST_SHOT = 61f;
    private const float FIRE_PERIOD = 5f;
    private const int cost_base = 3;
    private const int cost_A1 = 2;
    public CurWeaponDamage CurrentDamage { get; }


    public override int CostCount
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.A1)
            {
                return cost_A1;
            }
            return cost_base;
        }
    }

    private float Period => PERIOD + Level * 3;

    public ShieldOffSpell()
        : base(SpellType.shildDamage, 3, 15,
            new BulleStartParameters(9.7f, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        CurrentDamage = new CurWeaponDamage(2, 0);
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        var p = ship.DamageData.HaveDamage(ShipDamageType.shiled);
        if (!p)
        {
            var per = ship.ShipParameters.CurShiled / ship.ShipParameters.MaxShield;
            return per > 0.3f;
        }
        return false;

    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        MainCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    public override bool ShowLine => false;
    public override float ShowCircle => rad;
    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = target.Position - weapon.CurPosition;
        var dist = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        bullestartparameters.distanceShoot = dist;
        bullestartparameters.radiusShoot = dist;
        var b = Bullet.Create(origin, weapon, dir,
            weapon.CurPosition, null, bullestartparameters);
    }
    public override BulletDestroyDelegate BulletDestroyDelegate => BulletDestroy;

    private void BulletDestroy(Bullet origin, IWeapon weapon, AICell cell)
    {
        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.ShieldOffAOE, origin.Position, 3f);
    }

    public override Bullet GetBulletPrefab()
    {
        // var bullet = DataBaseController.Instance.GetBullet(WeaponType.shieldOFfSpell);
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrame);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }
    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    protected override void CastAction(Vector3 pos)
    {

    }
    private void ActionShip(ShipBase shipBase, DamageDoneDelegate damageDone)
    {
        shipBase.DamageData.ApplyEffect(ShipDamageType.shiled, Period);
        shipBase.ShipParameters.Damage(SHIELD_DAMAGE, 0, damageDone, shipBase);
        if (UpgradeType == ESpellUpgradeType.B2)
        {
            shipBase.DamageData.ApplyEffect(ShipDamageType.fire, FIRE_PERIOD, 1f);
        }

    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("ShieldOffSpell"), Period.ToString("0"), SHIELD_DAMAGE);
        //            $"Disable shields of ships in radius for {Period.ToString("0")} sec. And damages shield for {SHIELD_DAMAGE}.";
    }

    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("ShieldOffNameA1");
        }
        return Namings.Tag("ShieldOffNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("ShieldOffDescA1");
        }
        return Namings.Format(Namings.Tag("ShieldOffDescB2"), FIRE_PERIOD);
    }

}

