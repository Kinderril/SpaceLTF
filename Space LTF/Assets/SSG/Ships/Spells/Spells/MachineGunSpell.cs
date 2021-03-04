using UnityEngine;


[System.Serializable]
public class MachineGunSpell : BaseSpellModulInv
{
    //A1 - shoot to all
    //B2 - more bullets

    private const float DIST_SHOT = 25f;
    private const float RAD_A1 = 15f;
    // private const float baseDamage = 4;
    private const float rad = 1f;
    private const float _offset = 0.2f;

    //3 + Level * 2;
    private float DmgHull
    {
        get
        {
            var a = 2 + Level * 1;
            if (UpgradeType == ESpellUpgradeType.A1)
            {
                a = a - 1;
            }
            return a;
        }
    }

    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }
    private float DmgShield => 1 + (int)(Level * 1.5f);
    private float _nextBulletTime;
    private bool _lastLeft;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(DmgShield, DmgHull);

    public int BulletsCount => ClacBulletCount(UpgradeType);

    private int ClacBulletCount(ESpellUpgradeType a)
    {
        switch (a)
        {
            case ESpellUpgradeType.A1:
                return 3;
            case ESpellUpgradeType.B2:
                return 6;
        }
        return 4;
    }

    public MachineGunSpell()
        : base(SpellType.machineGun,  15, 
            new BulleStartParameters(18f, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        _localSpellDamageData = new SpellDamageData();
    }

    public override UpdateCastDelegate UpdateCast => UpdateCastInner;


    private void UpdateCastInner(Vector3 trgpos,
        BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castData)
    {
        if (_nextBulletTime > Time.time)
        {
            return;
        }

        var battle = BattleController.Instance;
        if (battle.State != BattleState.process)
        {
            return;
        }

        _nextBulletTime = Time.time + CoinTempController.BATTERY_PERIOD;
        for (int j = 0; j < castData.ShootsCount; j++)
        {
            if (UpgradeType == ESpellUpgradeType.A1)
            {
                var closestsShips = BattleController.Instance.GetAllShipsInRadius(shootpos,
                    BattleController.OppositeIndex(weapon.TeamIndex), DIST_SHOT);
                foreach (var ship in closestsShips)
                {
                    ShootToTarget(ship.Position, origin, weapon, castData.Bullestartparameters);
                }
            }
            else
            {
                for (int i = 0; i < castData.ShootsCount; i++)
                {
                    ShootToTarget(target.Position, origin, weapon, castData.Bullestartparameters);
                }

            }
        }
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castData)
    {


    }

    private void ShootToTarget(Vector3 pos
    , Bullet origin, IWeapon weapon, BulleStartParameters bullestartparameters)
    {
        for (int i = 0; i < BulletsCount; i++)
        {
            var timer =
                MainController.Instance.BattleTimerManager.MakeTimer(.1f * i);
            timer.OnTimer += () =>
            {
                var battle = BattleController.Instance;
                if (battle.State == BattleState.process)
                {
                    var coef = TimeCoef();
                    bullestartparameters.size = Mathf.Clamp(coef * 0.1f, 1, 2f);
                    modificatedCreateBullet(GetTrg(pos), origin, weapon, weapon.CurPosition, bullestartparameters);
                }
            };
        }
    }

    private BulletTarget GetTrg(Vector3 pos)
    {
        float xx;
        float zz;

        if (_lastLeft)
        {
            xx = MyExtensions.Random(-_offset, _offset);
            zz = MyExtensions.Random(-_offset, _offset);
        }
        else
        {

            xx = MyExtensions.Random(-_offset, _offset);
            zz = MyExtensions.Random(-_offset, _offset);
        }
        _lastLeft = !_lastLeft;
        var nTargte = new BulletTarget(pos + new Vector3(xx, 0, zz));
        return nTargte;
    }

    private float TimeCoef()
    {
        var delta = Time.time - _castStartTime;
        var mm = Mathf.Pow(delta - 1, 0.5f);
        var coef = Mathf.Clamp(mm, 1f, 6f);
        return coef;
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target,
        Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var coef = TimeCoef();
        ActionShip(target, additional.CurrentDamage.BodyDamage * coef, additional.CurrentDamage.ShieldDamage * coef, damagedone);
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

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.machineGun);
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
        return Namings.Format(Namings.Tag("MachineGunSpellDesc"), BulletsCount, DmgShield, DmgHull);
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("MachineGunNameA1");
        }
        return Namings.Tag("MachineGunNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {

        if (type == ESpellUpgradeType.A1)
        {
            var bulCout = ClacBulletCount(type);
            return Namings.Format(Namings.Tag("MachineGunDescA1"), bulCout);
        }
        var bulCou2t = ClacBulletCount(type);
        return Namings.Format(Namings.Tag("MachineGunDescB2"), bulCou2t);
    }
}

