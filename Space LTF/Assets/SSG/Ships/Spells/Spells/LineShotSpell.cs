using System.Security.Cryptography.X509Certificates;
using UnityEngine;


[System.Serializable]
public class LineShotSpell : BaseSpellModulInv
{
    //A1 - No death
    //B2 - more fire

    private const float BULLET_SPEED = 9f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 48f;

    private const int FIRE_PERIOD = 5;

    public override bool ShowLine => true;
    public override float ShowCircle => -1;
    private float _nextBulletTime;

    private float FireCoef
    {
        get { return FireCoefCalc(Level, UpgradeType); }
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(FirePeriod, Damage);

    private float FireCoefCalc(int level, ESpellUpgradeType upd)
    {
        if (upd == ESpellUpgradeType.B2)
        {
            switch (level)
            {
                case 4:
                    return 2.2f;
                case 3:
                    return 2.1f;
            }
        }
        switch (level)
        {
            case 4:
                return 1.7f;
            case 3:
                return 1.4f;
            case 2:
                return 1.1f;
            default:
            case 1:
                return 1f;
        }
    }

    private int FirePeriod => FIRE_PERIOD + Level;
    private int Damage => 5 + Level;

    public LineShotSpell()
        : base(SpellType.lineShot,  11,
             new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData();
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        _nextBulletTime = 0f;

    }
    private void UpdateCastInner(Vector3 trgpos,
        BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        if (_nextBulletTime < Time.time)
        {
            var ANG_2 = 20f;
            var period = CoinTempController.BATTERY_PERIOD * PowerDec();
            period = Mathf.Clamp(period, 0.2f, 1);
            _nextBulletTime = Time.time + period;
            //        Debug.Log($"dir to shoot{dirToShoot}   targte{target.Position}   from{shootPos}");
            for (int i = 0; i < castData.ShootsCount; i++)
            {
                var dirToShoot = target.Position - shootPos;
                modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
                var half = ANG_2 / 2f;

                var r1 = Utils.RotateOnAngUp(dirToShoot, -half);
                var r2 = Utils.RotateOnAngUp(dirToShoot, half);

                var sp1 = target.Position + r1;
                var sp2 = target.Position + r2;

                modificatedCreateBullet(new BulletTarget(sp1), origin, weapon, shootPos, castData.Bullestartparameters);
                modificatedCreateBullet(new BulletTarget(sp2), origin, weapon, shootPos, castData.Bullestartparameters);
            }
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {

        shipparameters.Damage(additional.CurrentDamage.BodyDamage, additional.CurrentDamage.BodyDamage, damagedone, target);
        target.DamageData.ApplyEffect(ShipDamageType.fire, additional.CurrentDamage.ShieldDamage, FireCoef);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var dirToShoot = target.Position - shootPos;
        var b0 = Bullet.Create(origin, weapon, dirToShoot, shootPos, target.target, bullestartparameters);
        if (UpgradeType == ESpellUpgradeType.A1)
        {
            b0.DeathOnHit = false;
        }
    }

    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => UpdateCastInner;

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.linerShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {

    }

    public override string Desc()
    {
        var totalFireDamage = FirePeriod * FireCoef;
        var damageStr = totalFireDamage.ToString("0");
        return Namings.Format(Namings.Tag("LineSHotSpell"), Damage, Damage, FirePeriod, damageStr);
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("LineShotNameA1");
        }
        return Namings.Tag("LineShotNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("LineShotDescA1");
        }
        var b1 = FireCoefCalc(Library.MAX_SPELL_LVL, ESpellUpgradeType.B2) -
                 FireCoefCalc(Library.MAX_SPELL_LVL, ESpellUpgradeType.None);
        return Namings.Format(Namings.Tag("LineShotDescB2"), b1);
    }
}

