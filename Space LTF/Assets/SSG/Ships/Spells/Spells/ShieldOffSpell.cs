using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class ShieldOffSpell : BaseSpellModulInv
{
//    public 

    public const float PERIOD = 20f;
    private const float dist = 28f;
    private const float rad = 3f;
    private const float BULLET_SPEED = 13f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 25f;
    public CurWeaponDamage CurrentDamage { get; }
    //    [NonSerialized]
    //    private Bullet bullet;

    public ShieldOffSpell(int costCount, int costTime)
        : base(SpellType.shildDamage, costCount, costTime,
            MainCreateBullet, MainAffect, new BulleStartParameters(9.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {
        CurrentDamage = new CurWeaponDamage(2,0);
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target,damagedone);
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));
    }
      

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.shieldOFfSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    protected override void CastAction(Vector3 pos)
    {
//        EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.ShieldOffAOE, pos, 3f);
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
        //        var dir = Utils.NormalizeFastSelf(pos - ModulPos.position);
        //        Bullet.Create(bullet, this, dir, ModulPos.position, null, BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT);

    }



    private static void ActionShip(ShipBase shipBase,DamageDoneDelegate damageDone)
    {
        shipBase.DamageData.ApplyEffect(ShipDamageType.shiled,PERIOD);
        shipBase.ShipParameters.Damage(3,0, damageDone,shipBase);

    }

//    public void BulletDestroyed(Vector3 position, Bullet bullet)
//    {
//        var c1 = BattleController.Instance.GetAllShipsInRadius(position, TeamIndex.green, rad);
//        var c2 = BattleController.Instance.GetAllShipsInRadius(position, TeamIndex.red, rad);
//        foreach (var shipBase in c1)
//        {
//            ActionShip(shipBase);
//        }
//        foreach (var shipBase in c2)
//        {
//            ActionShip(shipBase);
//        }
//    }

}

