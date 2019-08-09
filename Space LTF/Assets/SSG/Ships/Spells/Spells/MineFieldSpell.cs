using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class MineFieldSpell : BaseSpellModulInv 
{
    public const int MINES_COUNT = 2;
    public const float MINES_PERIOD = 20f;
    private const float rad = 2f;
    private const float damageBody = 3f;
    private const float damageShield = 2f;
    private float _distToShoot;

    private int MinesCount => MINES_COUNT + Level;

    private float dist;//Костыльный параметр
    public MineFieldSpell(int costCount, int costTime)
        : base(SpellType.mineField, costCount, costTime,
             new BulleStartParameters(9.7f, 36f, 25, 25), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var deltaAng = 360f / MinesCount;
        var direction = MyExtensions.IsTrueEqual() ? Vector3.right : Vector3.left;
        var baseDist = (target.Position - weapon.CurPosition).magnitude;
//        Debug.LogError($"Mine base dist {baseDist}");
        for (int i = 0; i < MinesCount; i++)
        {
            direction = Utils.RotateOnAngUp(direction, MyExtensions.GreateRandom((deltaAng * i)));
            var position = target.Position + direction * MyExtensions.Random(rad / 4, rad);
            var dir = (position - weapon.CurPosition);
            bullestartparameters.distanceShoot = baseDist;
            MainCreateBullet(new BulletTarget(dir + weapon.CurPosition), origin, weapon, shootPos, bullestartparameters);
        }
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.Position - weapon.CurPosition);
        //        Debug.LogError("");                  
        var dist = dir.magnitude;
//        Debug.LogError($"Mine result dist {dist}");
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, null,
                new BulleStartParameters(Library.MINE_SPEED, 0f, dist, dist));
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(2, 3, damagedone, target);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;
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
        return new SpellDamageData();
    }
    public override string Desc()
    {
        return
            $"Set {MinesCount} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location. Each mine damage {damageShield}/{damageBody}";
    }
}

