using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class ThrowAroundSpell : BaseSpellModulInv
{
    private const float DIST_SHOT = 25f;
    private const float rad = 3f;

    public ThrowAroundSpell(int costCount, int costTime)
        : base(SpellType.throwAround, costCount, costTime,
            MainCreateBullet, MainAffect, new BulleStartParameters(9.7f, 36f, DIST_SHOT, DIST_SHOT), false)
    {

    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        ActionShip(target, bullet.Position, damagedone);
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, weapon.CurPosition, weapon.CurPosition, null,
            new BulleStartParameters(Library.MINE_SPEED, 0f, DIST_SHOT, DIST_SHOT));
    }

    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.throwAroundSpell);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
//        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
//        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
//        foreach (var shipBase in c1)
//        {
//            ActionShip(shipBase, pos);
//        }
//        foreach (var shipBase in c2)
//        {
//            ActionShip(shipBase, pos);
//        }
    }


    private static void ActionShip(ShipBase shipBase,Vector3 fromPos,DamageDoneDelegate damageDoneCallback)
    {
        shipBase.ShipParameters.Damage(0,2, damageDoneCallback,shipBase);
        var dir = Utils.NormalizeFastSelf(shipBase.Position - fromPos);
        shipBase.ExternalForce.Init(10,1f,dir);

    }

}

