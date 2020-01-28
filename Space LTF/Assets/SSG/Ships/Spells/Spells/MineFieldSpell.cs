using System;
using UnityEngine;


[System.Serializable]
public class MineFieldSpell : BaseSpellModulInv
{
    public const int BASE_MINES_COUNT = 3;
    public const float MINES_PERIOD = 20f;
    public const float MINES_DIST = 15f;
    private const float rad = 3.5f;
    private const float damageBody = 7f;
    private const float damageShield = 3f;
    private float _distToShoot;

    private int MINES_COUNT => BASE_MINES_COUNT + Level;
    public float DAMAGE_BODY => damageBody + Level;
    public float DAMAGE_SHIELD => damageShield + Level;

    private float dist;//Костыльный параметр
    public MineFieldSpell(int costCount, int costTime)
        : base(SpellType.mineField, costCount, costTime,
             new BulleStartParameters(9.7f, 36f, MINES_DIST, MINES_DIST), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        var deltaAng = 360f / MINES_COUNT;
        var direction = MyExtensions.IsTrueEqual() ? Vector3.right : Vector3.left;
        var baseDist = (target.Position - weapon.CurPosition).magnitude;
        //        Debug.LogError($"Mine base dist {baseDist}");
        for (int i = 0; i < MINES_COUNT; i++)
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

    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.Damage(DAMAGE_SHIELD, DAMAGE_BODY, damagedone, target);
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
        return String.Format(Namings.MinesSpell, MINES_COUNT, MineFieldSpell.MINES_PERIOD.ToString("0"), DAMAGE_SHIELD, DAMAGE_BODY);
        //            $"Set {MinesCount} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location. Each mine damage {damageShield}/{damageBody}";
    }
}

