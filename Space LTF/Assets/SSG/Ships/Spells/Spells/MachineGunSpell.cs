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

    //3 + Level * 2;
    private float DmgHull
    {
        get
        {
            var a = 3 + Level * 2;
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
        : base(SpellType.machineGun, 1, 45, 
            new BulleStartParameters(14f, 36f, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castData)
    {
        var battle = BattleController.Instance;
        var offset = rad / 2;
        if (UpgradeType == ESpellUpgradeType.A1)
        {
            var closestsShips = BattleController.Instance.GetAllShipsInRadius(shootpos,
                BattleController.OppositeIndex(weapon.TeamIndex), DIST_SHOT);
            foreach (var ship in closestsShips)
            {
                for (int i = 0; i < BulletsCount + castData.ShootsCount - 1; i++)
                {
                    ShootToTarget(battle, offset, i * .3f, ship.Position, origin, weapon, castData.Bullestartparameters);
                }
            }
        }
        else
        {
            for (int i = 0; i < BulletsCount + castData.ShootsCount - 1; i++)
            {
                ShootToTarget(battle, offset, i * .3f, target.Position, origin, weapon, castData.Bullestartparameters);
            }

        }

    }

    private void ShootToTarget(BattleController battle, float offset, float timerDelta, Vector3 pos
    , Bullet origin, IWeapon weapon, BulleStartParameters bullestartparameters)
    {
        var timer =
            MainController.Instance.BattleTimerManager.MakeTimer(timerDelta);
        timer.OnTimer += () =>
        {
            if (battle.State == BattleState.process)
            {
                var xx = MyExtensions.Random(-offset, offset);
                var zz = MyExtensions.Random(-offset, offset);

                var nTargte = new BulletTarget(pos + new Vector3(xx, 0, zz));
                modificatedCreateBullet(nTargte, origin, weapon, weapon.CurPosition, bullestartparameters);
            }
        };
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

