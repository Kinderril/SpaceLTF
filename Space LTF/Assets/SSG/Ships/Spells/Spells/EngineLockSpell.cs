using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public  class EngineLockSpell : BaseSpellModulInv
{
    public const float DIST_SHOT = 8f;
    public const float LOCK_PERIOD = 6;
    public const float LOCK_LEVEL = 2f;
    private const float rad = 8f;
    [NonSerialized]
    private SpellZoneVisualCircle ObjectToShow;

    public float CurLockPeriod => LOCK_PERIOD + LOCK_LEVEL * Level;

    public EngineLockSpell(int costCount, int costTime)
        : base(SpellType.engineLock, costCount, costTime, new BulleStartParameters(15, 36f, DIST_SHOT, DIST_SHOT), false)
    {
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        EngineCreateBullet(target, origin, weapon, shootPos, bullestartparameters);

    }
    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;

    private void EngineCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var startPos = target.Position + new Vector3(MyExtensions.Random(-rad, rad), DIST_SHOT, MyExtensions.Random(-rad, rad));
        var dir = target.Position - startPos;
        bullestartparameters.distanceShoot = dir.magnitude;
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
        shipBase.DamageData.ApplyEffect(ShipDamageType.engine, CurLockPeriod, true);//.EngineStop.Stop(2.5f);
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
        return String.Format(Namings.EnerguLockSpell, CurLockPeriod.ToString("0"));
//            $"Destroy engines for {CurLockPeriod.ToString("0")} sec.";
    }
}

