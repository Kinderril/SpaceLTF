using System;
using UnityEngine;


[System.Serializable]
public class RechargeShieldSpell : BaseSpellModulInv
{
    //A1 - AOE 
    //B2 - Resist X sec

    public const float LEFRP_COEF = 0.8f;
    public const float OFF_PERIOD = 20f;

    private const int RAD_B2 = 4;
    private const float AOE_rad = 2f;

    private const float BULLET_SPEED = 50f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 42;
    private const float PERIOD_COEF = 0.5f;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(HealPercent, HealPercent);

    private float HealPercent => (Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT + Level * 0.12f) * PERIOD_COEF;


    [NonSerialized] private Vector3 _prevTrg;
    [NonSerialized] private BeamBulletNoTarget ControlBullet;

    public RechargeShieldSpell()
        : base(SpellType.rechargeShield,  15, 
            new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED,
                DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData();
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        _prevTrg = target.Position;
        modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);

    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    private void DistShotCreateBullet(BulletTarget target, Bullet origin,
        IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = target.Position - shootpos;
        //        Debug.LogError($"dir:{dir}    target.Position:{target.Position}");
        bullestartparameters.distanceShoot = 0.25f;
        var b = Bullet.Create(origin, weapon, dir, shootpos, null, bullestartparameters);
        b.SpellBulletPower = new SpellBulletPower(bullestartparameters.Power);
        ControlBullet = b as BeamBulletNoTarget;
        if (ControlBullet != null)
        {
            ControlBullet.coefWidth = GetWidth();
        }
    }

    private float GetWidth()
    {

        if (UpgradeType == ESpellUpgradeType.B2)
        {
            return RAD_B2;
        }
        else
        {
            return 1f;
        }
    }


    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var ship = target;

        var maxShield = shipparameters.ShieldParameters.MaxShield;
        var countToHeal = maxShield * additional.CurrentDamage.ShieldDamage * 0.35f;
        ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
        // Debug.LogError($"heqal shiedl:{countToHeal}");
        shipparameters.ShieldParameters.HealShield(countToHeal);
        if (UpgradeType == ESpellUpgradeType.B2)
        {
            if (!ship.DamageData.IsReflecOn)
            {
                ship.DamageData.TurnOnReflectFor(OFF_PERIOD);
            }
        }
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.beamRepair);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }
    public override bool ShowLine => true;
    public override float ShowCircle => -1;

    protected override CreateBulletDelegate standartCreateBullet => DistShotCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => ProcessCast;

    private void ProcessCast(Vector3 trgpos, BulletTarget target, 
        Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {
        var nextTrg = Vector3.Lerp(trgpos, _prevTrg, LEFRP_COEF);
        var dir = Utils.NormalizeFastSelf(nextTrg - shootpos);
        var deltaTime = Time.time - _castStartTime;
        Vector3 trg = shootpos + dir * deltaTime * 10;
        _prevTrg = nextTrg;

        if (ControlBullet != null && ControlBullet.IsAcive)
        {
            ControlBullet.MoveTargetTo(trg);
            ControlBullet.coefWidth = GetWidth() * PowerInc();
            ControlBullet.SetDeathTime(Time.time +.1f);
        }
    }

    protected override void CastAction(Vector3 pos)
    {

    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("RechargeSheildSpell"), Utils.FloatToChance(HealPercent));
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("RechargeSheildNameA1");
        }
        return Namings.Tag("RechargeSheildNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Format(Namings.Tag("RechargeSheildDescA1"), AOE_rad);
        }
        return Namings.Format(Namings.Tag("RechargeSheildDescB2"), OFF_PERIOD);
    }

}

