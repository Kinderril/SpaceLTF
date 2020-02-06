
using System;
using UnityEngine;


[System.Serializable]
public class DistShotSpell : BaseSpellModulInv
{
    //A1 - Engine lock
    //B2 - AOE

    private const int DIST_BASE_DAMAGE = 8;
    private const int BASE_DAMAGE = 18;
    private const int LEVEL_DAMAGE = 8;
    private const int RAD_B2 = 8;
    private const float DIST_COEF = 0.8f;
    private const float ENGINE_OFF_DELTA = 3f;
    private const float ENGINE_OFF_LEVEL = 1f;

    // [NonSerialized]
    // private CurWeaponDamage CurWeaponDamage;

    private const float BULLET_SPEED = 13f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 34f;
    public DistShotSpell()
        : base(SpellType.distShot, 5, 20, new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false)
    {
        // CurWeaponDamage = new CurWeaponDamage(0, 12);
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        DistShotCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }

    private void DistShotCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var b = Bullet.Create(origin, weapon, target.Position - shootpos, shootpos, null, bullestartparameters);
    }

    public int BASE_damage => BASE_DAMAGE + LEVEL_DAMAGE * Level;
    public float Engine_Off => ENGINE_OFF_DELTA + ENGINE_OFF_LEVEL * Level;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var dist = (target.Position - bullet1.Weapon.Owner.Position).magnitude;
        var totalDistDamage = dist * DIST_COEF;
        int damage = BASE_damage + Mathf.Clamp((int)totalDistDamage, 0, DIST_BASE_DAMAGE);
        //        int baseSpDamage = (int)(BASE_DAMAGE / c);

        target.ShipParameters.Damage(0, damage, bullet1.Weapon.DamageDoneCallback, target);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                target.DamageData.ApplyEffect(ShipDamageType.engine, Engine_Off);
                break;
            case ESpellUpgradeType.B2:
                var closestsShips = BattleController.Instance.GetAllShipsInRadius(target.Position,
                    target.TeamIndex, RAD_B2);
                closestsShips.Remove(target);
                if (closestsShips.Count > 0)
                {
                    foreach (var ship in closestsShips)
                    {
                        ship.ShipParameters.Damage(0, totalDistDamage, bullet1.Weapon.DamageDoneCallback, target);
                    }
                }
                break;
        }
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.distShot);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }
    public override bool ShowLine => true;
    public override float ShowCircle => -1;

    protected override CreateBulletDelegate createBullet => DistShotCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    protected override void CastAction(Vector3 pos)
    {

    }
    public override string Desc()
    {
        return Namings.TryFormat(Namings.Tag("DistShotSpell"), BASE_damage, Engine_Off.ToString("0.0"));
        // return Namings.TryFormat(Namings.Tag("DistShotSpellSpecial"), BASE_damage, Engine_Off.ToString("0.0"));

    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("DistShotNameA1");
        }
        return Namings.Tag("DistShotNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("DistShotDescA1");
        }
        return Namings.Tag("DistShotDescB2");
    }

}

