using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class MineFieldSpell : BaseSpellModulInv 
{
    public const int MINES_COUNT = 4;
    public const float MINES_PERIOD = 20f;
    private const float rad = 2f;

    private float dist;//Костыльный параметр
    public MineFieldSpell(int costCount, int costTime)
        : base(SpellType.mineField, costCount, costTime,
            MainCreateBullet, CastSpell, MainAffect, new BulleStartParameters(9.7f, 36f, 25, 25), false)
    {

    }

    private static void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var deltaAng = 360f / MINES_COUNT;
        var d = MyExtensions.IsTrue01(0.5f) ? Vector3.right : Vector3.left;
        for (int i = 0; i < MINES_COUNT; i++)
        {
            d = Utils.RotateOnAngUp(d, MyExtensions.GreateRandom((deltaAng * i)));
            var position = shootPos + d * MyExtensions.Random(rad / 4, rad);
            var dir = (position - weapon.CurPosition);
            var dist = dir.magnitude;
            MainCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
        }
    }

    private static void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.Position - weapon.CurPosition);
        var dist = dir.magnitude;
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, null,
                new BulleStartParameters(Library.MINE_SPEED, 0f, dist, dist));
    }

    private static void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(2, 3, damagedone, target);
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.castMine);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
//        int c = 4;

    }

    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad);
    }
}

