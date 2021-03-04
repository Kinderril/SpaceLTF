using System;
using UnityEngine;


[System.Serializable]
public class DistShotSpell : BaseSpellModulInv
{
    //A1 - Engine lock
    //B2 - AOE

    private const int DIST_BASE_DAMAGE = 8;
    private const int BASE_DAMAGE = 7;
    private const int LEVEL_DAMAGE = 5;

    private const int RAD_B2 = 4;
//    private const float DIST_COEF = 0.8f;
    private const float ENGINE_OFF_DELTA = 3f;
    private const float ENGINE_OFF_LEVEL = 1f;

    // [NonSerialized]
    // private CurWeaponDamage CurWeaponDamage;

    private const float BULLET_SPEED = 50f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 42;
    [NonSerialized] private BeamBulletNoTarget ControlBullet;
    public DistShotSpell()
        : base(SpellType.distShot,  13, 
            new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData();
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
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
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(Engine_Off, BASE_damage);
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

    public int BASE_damage => BASE_DAMAGE + LEVEL_DAMAGE * Level;
    public float Engine_Off => ENGINE_OFF_DELTA;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var dist = (target.Position - bullet1.Weapon.Owner.Position).magnitude;
        var totalDistDamage = dist;// * DIST_COEF;
        int damage = (int)(additional.CurrentDamage.BodyDamage + Mathf.Clamp((int)totalDistDamage, 0, DIST_BASE_DAMAGE));

        var deltaTime = Time.time - _castStartTime;
        var dmgShield = Mathf.Pow(deltaTime, 0.7f) - 2;
        dmgShield = Mathf.Clamp(dmgShield, 0, 999);
        target.ShipParameters.Damage(dmgShield, damage, bullet1.Weapon.DamageDoneCallback, target);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                target.DamageData.ApplyEffect(ShipDamageType.engine, additional.CurrentDamage.ShieldDamage);
                break;
        }
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.beamNoTarget);
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
        var dir = Utils.NormalizeFastSelf(trgpos - shootpos);
        var deltaTime = Time.time - _castStartTime;
        Vector3 trg = shootpos + dir * deltaTime * 10;
        if (ControlBullet != null && ControlBullet.IsAcive)
        {
            ControlBullet.MoveTargetTo(trg);
            var p = Mathf.Clamp(Mathf.Pow(deltaTime, 0.4f) + 1, 1, 5);
            ControlBullet.coefWidth = p;
            ControlBullet.SetDeathTime(Time.time + 0.1f);
        }
    }

    protected override void CastAction(Vector3 pos)
    {

    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("DistShotSpell"), BASE_damage, Engine_Off.ToString("0.0"));
        // return Namings.TryFormat(Namings.Tag("DistShotSpellSpecial"), BASE_damage, Engine_Off.ToString("0.0"));

    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("DistShotNameA1");
        }
        return Namings.Tag("DistShotNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Format(Namings.Tag("DistShotDescA1"), Engine_Off);
        }
        return Namings.Format(Namings.Tag("DistShotDescB2"), RAD_B2);
    }

}

