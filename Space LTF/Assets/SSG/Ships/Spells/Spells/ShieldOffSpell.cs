using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class ShieldOffSpell : BaseSpellModulInv
{
    public const float PERIOD = 13f;
    private const float SHIELD_DAMAGE = 3f;
    private const float rad = 3f;
    private const float DIST_SHOT = 25f;
    public CurWeaponDamage CurrentDamage { get; }

    private float Period => PERIOD + Level * 4;

    public ShieldOffSpell(int costCount, int costTime)
        : base(SpellType.shildDamage, costCount, costTime,
            new BulleStartParameters(9.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {
        CurrentDamage = new CurWeaponDamage(2,0);
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        MainCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    public override bool ShowLine => true;
    public override float ShowCircle => rad;
    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target,damagedone);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, target.Position - weapon.CurPosition,
            weapon.CurPosition, null, bullestartparameters);
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



    private void ActionShip(ShipBase shipBase,DamageDoneDelegate damageDone)
    {
        shipBase.DamageData.ApplyEffect(ShipDamageType.shiled, Period);
        shipBase.ShipParameters.Damage(SHIELD_DAMAGE,0, damageDone,shipBase);

    }
    public override string Desc()
    {
        return  String.Format(Namings.ShieldOffSpell, Period.ToString("0"), SHIELD_DAMAGE);
//            $"Disable shields of ships in radius for {Period.ToString("0")} sec. And damages shield for {SHIELD_DAMAGE}.";
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

