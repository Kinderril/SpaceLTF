using UnityEngine;


[System.Serializable]
public class ArtillerySpell : BaseSpellModulInv
{
    //A1 - more bullets        
    //B2 - faster shoot

    private const float DIST_SHOT = 40f;
    private const float SPEED_COEF = 0.6f;
    // private const float baseDamage = 4;
    private const float rad = 16f;


    private float DmgHull => 3 + Level;
    private float DmgShield => 2 + Level;
    public int BulletsCount => BulletByLevel(Level, UpgradeType);

    private int BulletByLevel(int l, ESpellUpgradeType up)
    {
        if (up == ESpellUpgradeType.A1)
        {
            return l * 3 + 4;
        }
        return l * 2 + 3;
    }

    public ArtillerySpell()
        : base(SpellType.artilleryPeriod,  20,
            new BulleStartParameters(9.5f, 36f, DIST_SHOT, DIST_SHOT),
            false,TargetType.Enemy)
    {
        _localSpellDamageData =  new SpellDamageData(rad, false);
    }

    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(DmgShield,DmgHull);

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    private float CalcRad()
    {
        if (!_isInProcess)
        {
            return rad;
        }
        var coef = coefSize();
        return rad * coef;
    }

    private float Period(ESpellUpgradeType upg)
    {
        float period = 0.115f;
        if (upg == ESpellUpgradeType.B2)
        {
            period = 0.065f;
        }

        return period;
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castData)
    {
        _localSpellDamageData.AOERad = rad;
    }

    private float _nextBulletTime;

    private void UpdateCastInner(Vector3 trgpos,
        BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {
        _localSpellDamageData.AOERad = CalcRad();
        if (_nextBulletTime < Time.time)
        {
            for (int i = 0; i < castdata.ShootsCount; i++)
            {

                float period = Period(UpgradeType);
                _nextBulletTime = period + Time.time;
                var offset = CalcRad() * .5f;
                var battle = BattleController.Instance;
                if (battle.State == BattleState.process)
                {
                    var xx = MyExtensions.Random(-offset, offset);
                    var zz = MyExtensions.Random(-offset, offset);

                    var nTargte = new BulletTarget(trgpos + new Vector3(xx, 0, zz));

                    var coef = coefSpeed();
                    castdata.Bullestartparameters.bulletSpeed = castdata.Bullestartparameters.bulletSpeed * coef;
                    modificatedCreateBullet(nTargte, origin, weapon, weapon.CurPosition, castdata.Bullestartparameters);
                }
            }
        }
    }

    protected override void EndCastSpell()
    {
        _localSpellDamageData.AOERad = rad;
        base.EndCastSpell();
    }

    private float coefSpeed()
    {

        var deltaFromStart = Time.time - _castStartTime;
        var res = Mathf.Pow(deltaFromStart, 0.7f);
        res = res * 0.6f + 1;
        return Mathf.Clamp(res, 1, 4) * SPEED_COEF;
    }

    // private const float _upperSIze = 3;

    private float coefSize()
    {
        var deltaFromStart = Time.time - _castStartTime;
        // return 1 - deltaFromStart
        return Mathf.Clamp(1 - deltaFromStart * 0.18f,0.13f, 1f);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target,
        Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, additional.CurrentDamage.BodyDamage, additional.CurrentDamage.ShieldDamage, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var startPos = weapon.CurPosition;
        var dir = Utils.NormalizeFastSelf(target.Position - startPos);
        Bullet.Create(origin, weapon, dir, startPos,
            null, bullestartparameters);
    }

    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => UpdateCastInner;


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.artilleryBullet);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override bool ShowLine => true;
    public override float ShowCircle => rad;
    //    public override bool S => false;

    protected override void CastAction(Vector3 pos)
    {
    }


    private static void ActionShip(ShipBase shipBase, float bodyDamage, float shieldDamage, DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(shieldDamage, bodyDamage, damageDoneCallback, shipBase);
    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("ArtillerySpell"), BulletsCount, DmgHull, DmgShield);
    }

    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("ArtilleryNameA1");
        }
        return Namings.Tag("ArtilleryNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            var b1 = BulletByLevel(Library.MAX_SPELL_LVL, ESpellUpgradeType.A1) -
                     BulletByLevel(Library.MAX_SPELL_LVL, ESpellUpgradeType.None);
            return Namings.Format(Namings.Tag("ArtilleryDescA1"), b1);
        }

        var b2 = Period(ESpellUpgradeType.B2) - Period(ESpellUpgradeType.A1);
        return Namings.Format(Namings.Tag("ArtilleryDescB2"), b2);
    }
}

