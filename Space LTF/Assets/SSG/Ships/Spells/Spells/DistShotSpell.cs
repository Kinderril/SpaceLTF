using UnityEngine;


[System.Serializable]
public class DistShotSpell : BaseSpellModulInv
{
    //A1 - Engine lock
    //B2 - AOE

    private const int DIST_BASE_DAMAGE = 8;
    private const int BASE_DAMAGE = 7;
    private const int LEVEL_DAMAGE = 5;

    private const int RAD_B2 = 4;
//    private const float DIST_COEF = 0.8f;
    private const float ENGINE_OFF_DELTA = 3f;
    private const float ENGINE_OFF_LEVEL = 1f;

    // [NonSerialized]
    // private CurWeaponDamage CurWeaponDamage;

    private const float BULLET_SPEED = 50f;
    private const float BULLET_TURN_SPEED = .2f;
    private const float DIST_SHOT = 154f;
    public DistShotSpell()
        : base(SpellType.distShot, 5, 13, 
            new BulleStartParameters(BULLET_SPEED, BULLET_TURN_SPEED, DIST_SHOT, DIST_SHOT), false,TargetType.Enemy)
    {
        // CurWeaponDamage = new CurWeaponDamage(0, 12);
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        DistShotCreateBullet(target, origin, weapon, shootPos, bullestartparameters);
    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        return true;
    }

    private void DistShotCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = target.Position - shootpos;
//        Debug.LogError($"dir:{dir}    target.Position:{target.Position}");
        var b = Bullet.Create(origin, weapon, dir, shootpos, null, bullestartparameters);
        var beamNoTrg = b as BeamBulletNoTarget;
        if (beamNoTrg != null)
        {
            if (UpgradeType == ESpellUpgradeType.B2)
            {
                beamNoTrg.coefWidth = RAD_B2;
            }
            else
            {
                beamNoTrg.coefWidth = 1f;
            }
        }
    }

    public int BASE_damage => BASE_DAMAGE + LEVEL_DAMAGE * Level;
    public float Engine_Off => ENGINE_OFF_DELTA;

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var dist = (target.Position - bullet1.Weapon.Owner.Position).magnitude;
        var totalDistDamage = dist;// * DIST_COEF;
        int damage = BASE_damage + Mathf.Clamp((int)totalDistDamage, 0, DIST_BASE_DAMAGE);

        target.ShipParameters.Damage(0, damage, bullet1.Weapon.DamageDoneCallback, target);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                target.DamageData.ApplyEffect(ShipDamageType.engine, Engine_Off);
                break;
//            case ESpellUpgradeType.B2:
//                var closestsShips = BattleController.Instance.GetAllShipsInRadius(target.Position,
//                    target.TeamIndex, RAD_B2);
//                closestsShips.Remove(target);
//                if (closestsShips.Count > 0)
//                {
//                    foreach (var ship in closestsShips)
//                    {
//                        ship.ShipParameters.Damage(0, totalDistDamage, bullet1.Weapon.DamageDoneCallback, target);
//                    }
//                }
//                break;
        }
    }


    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.beamNoTarget);
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
        return Namings.Format(Namings.Tag("DistShotSpell"), BASE_damage, Engine_Off.ToString("0.0"));
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
            return Namings.Format(Namings.Tag("DistShotDescA1"), Engine_Off);
        }
        return Namings.Format(Namings.Tag("DistShotDescB2"), RAD_B2);
    }

}

