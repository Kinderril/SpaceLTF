using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public    class EngineLockSpell : BaseSpellModulInv
{
    public const float DIST_SHOT = 22f;
    public const float LOCK_PERIOD = 12f;
    private const float rad = 8f;
    [NonSerialized]
    private SpellZoneVisualCircle ObjectToShow;

    public EngineLockSpell(int costCount, int costTime)
        : base(SpellType.engineLock, costCount, costTime, 
            EngineCreateBullet, CastSpell, MainAffect, new BulleStartParameters(9.7f, 36f, 25, 25), false)
    {
    }
    private static void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        EngineCreateBullet(target, origin, weapon, shootPos, bullestartparameters);

    }

    private static void EngineCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {


        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));

    }


    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, 
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var pos = bullet.Position;
        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.EngineLockAOE,pos,3f);
        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
        foreach (var shipBase in c1)
        {
            ActionShip(shipBase);
        }
        foreach (var shipBase in c2)
        {
            ActionShip(shipBase);
        }
    }



    private static void ActionShip(ShipBase shipBase)
    {
        shipBase.DamageData.ApplyEffect(ShipDamageType.engine, LOCK_PERIOD,true);//.EngineStop.Stop(2.5f);
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
}

