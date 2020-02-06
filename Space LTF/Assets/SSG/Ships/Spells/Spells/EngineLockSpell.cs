using System;
using UnityEngine;


[System.Serializable]
public class EngineLockSpell : BaseSpellModulInv
{
    //A1 - more rad
    //B2 - less timer

    public const float DIST_SHOT = 22f;
    public const float LOCK_PERIOD = 4;
    public const float LOCK_LEVEL = 2f;

    private float rad
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.A1)
            {
                return 4;
            }
            return 2.5f;
        }
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

    private const int _baseCostTime = 15;
    private const int _B2_costTime = 10;

    public float CurLockPeriod => LOCK_PERIOD + LOCK_LEVEL * Level;

    public EngineLockSpell()
        : base(SpellType.engineLock, 3, _baseCostTime, new BulleStartParameters(15, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        EngineCreateBullet(target, origin, weapon, shootPos, bullestartparameters);

    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }
    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;

    private void EngineCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        //        var startPos = target.Position + new Vector3(MyExtensions.Random(-rad, rad), DIST_SHOT, MyExtensions.Random(-rad, rad));
        var startPos = shootpos;
        var dir = target.Position - startPos;
        bullestartparameters.distanceShoot = Mathf.Clamp(dir.magnitude, 1, DIST_SHOT);
        var b = Bullet.Create(origin, weapon, dir, startPos, null, bullestartparameters);

    }


    protected override CreateBulletDelegate createBullet => EngineCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        if (target != null)
            ActionShip(target);
        //        var pos = bullet.Position;
        //        var oppositeIndex = BattleController.OppositeIndex(bullet.Weapon.TeamIndex);
        //        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.EngineLockAOE,pos, CurLockPeriod);
        //        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, oppositeIndex, rad);
        //        foreach (var shipBase in c1)
        //        {
        //            ActionShip(shipBase);
        //        }
        //        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
        //        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
        //        foreach (var shipBase in c1)
        //        {
        //            ActionShip(shipBase);
        //        }
        //        foreach (var shipBase in c2)
        //        {
        //            ActionShip(shipBase);
        //        }
    }



    private void ActionShip(ShipBase shipBase)
    {
        shipBase.DamageData.ApplyEffect(ShipDamageType.engine, CurLockPeriod);//.EngineStop.Stop(2.5f);
    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.engineLockSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {

    }

    public override string Desc()
    {
        return Namings.TryFormat(Namings.Tag("EnerguLockSpell"), CurLockPeriod.ToString("0"), rad.ToString("0"));
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
            return Namings.Tag("EngineLockDescA1");
        }
        return Namings.Tag("EngineLockDescB2");
    }
}

