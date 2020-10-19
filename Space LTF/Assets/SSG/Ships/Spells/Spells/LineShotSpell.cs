using UnityEngine;


[System.Serializable]
public class LineShotSpell : BaseSpellModulInv
{
    //A1 - No death
    //B2 - more fire

    private const float BULLET_SPEED = 10f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 38f;

    private const int FIRE_PERIOD = 6;

    private float FireCoef
    {
        get { return FireCoefCalc(Level, UpgradeType); }
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(Damage, Damage);

    private float FireCoefCalc(int level, ESpellUpgradeType upd)
    {
        if (upd == ESpellUpgradeType.B2)
        {
            switch (level)
            {
                case 4:
                    return 2.5f;
                case 3:
                    return 2.2f;
            }
        }
        switch (level)
        {
            case 4:
                return 1.9f;
            case 3:
                return 1.6f;
            case 2:
                return 1.3f;
            default:
            case 1:
                return 1f;
        }
    }

    private int FirePeriod => FIRE_PERIOD + Level;
    private int Damage => 5 + Level;

    public LineShotSpell()
        : base(SpellType.lineShot, 4, 11,
             new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {

    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        var ANG_2 = 20f;

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

            modificatedCreateBullet(target, origin, weapon, sp1, castData.Bullestartparameters);
            modificatedCreateBullet(target, origin, weapon, sp2, castData.Bullestartparameters);
        }

    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {

        shipparameters.Damage(Damage, Damage, damagedone, target);
        target.DamageData.ApplyEffect(ShipDamageType.fire, FirePeriod, FireCoef);
    }

    public override bool ShowLine => true;
    public override float ShowCircle => -1;
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

