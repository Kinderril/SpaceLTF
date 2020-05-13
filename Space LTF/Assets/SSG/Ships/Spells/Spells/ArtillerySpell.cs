using UnityEngine;


[System.Serializable]
public class ArtillerySpell : BaseSpellModulInv
{
    //A1 - more bullets        
    //B2 - faster shoot

    private const float DIST_SHOT = 40f;
    // private const float baseDamage = 4;
    private const float rad = 17f;


    private float DmgHull => 3 + Level;
    private float DmgShield => 2 + Level;
    public int BulletsCount => BulletByLevel(Level, UpgradeType);

    private int BulletByLevel(int l, ESpellUpgradeType up)
    {
        if (up == ESpellUpgradeType.A1)
        {
            return l * 4 + 14;
        }
        return l * 3 + 11;
    }

    public ArtillerySpell()
        : base(SpellType.artilleryPeriod, 4, 20,
            new BulleStartParameters(11.5f, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {

    }

    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }
    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad / 2f, false);
    }

    private float Period(ESpellUpgradeType upg)
    {
        float period = 0.145f;
        if (upg == ESpellUpgradeType.B2)
        {
            period = 0.095f;
        }

        return period;
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var battle = BattleController.Instance;
        var offset = rad / 2;
        float period = Period(UpgradeType);
        for (int i = 0; i < BulletsCount; i++)
        {
            var timer =
                MainController.Instance.BattleTimerManager.MakeTimer(i * period);
            timer.OnTimer += () =>
            {
                if (battle.State == BattleState.process)
                {
                    var xx = MyExtensions.Random(-offset, offset);
                    var zz = MyExtensions.Random(-offset, offset);

                    var nTargte = new BulletTarget(target.Position + new Vector3(xx, 0, zz));
                    MainCreateBullet(nTargte, origin, weapon, weapon.CurPosition, bullestartparameters);
                }
            };
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, DmgHull, DmgShield, damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var startPos = weapon.CurPosition;
        var dir = Utils.NormalizeFastSelf(target.Position - startPos);
        Bullet.Create(origin, weapon, dir, startPos,
            null, bullestartparameters);
    }

    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

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

