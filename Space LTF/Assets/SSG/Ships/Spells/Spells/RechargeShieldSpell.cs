using System;
using UnityEngine;


[System.Serializable]
public class RechargeShieldSpell : BaseSpellModulInv
{
    //A1 - AOE 
    //B2 - Resist X sec

    public const float MINES_DIST = 7f;
    public const float OFF_PERIOD = 20f;
    private const float rad = 1f;
    private const float AOE_rad = 4f;

    private const float _sDistToShoot = 4 * 4;
    private bool _lastCheckIsOk = false;
    [field: NonSerialized]
//    private ShipBase _lastClosest;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(HealPercent, HealPercent);

    private float HealPercent => Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT + Level * 0.12f;
    public RechargeShieldSpell()
        : base(SpellType.rechargeShield, 2, 30,
             new BulleStartParameters(15f, 46f, 
                 MINES_DIST, MINES_DIST), false,TargetType.Ally)
    {

    }
    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        var p = ship.ShipParameters.CurShiled / ship.ShipParameters.MaxShield;

        if (p < .5f)
        {
            return true;
        }

        return false;
    }
    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        var period = 0.5f;
        for (int i = 0; i < castData.ShootsCount; i++)
        {
            var pp = i * period;
            if (pp > 0)
            {
                var timer =
                    MainController.Instance.BattleTimerManager.MakeTimer(pp);
                timer.OnTimer += () =>
                {
                    modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
                };
            }
            else
            {
                modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
            }
        }
    }
    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var startPos = shootpos;
        var dir = target.Position - startPos;
        bullestartparameters.distanceShoot = dir.magnitude;
        var b = Bullet.Create(origin, weapon, dir, startPos, null, bullestartparameters);
    }


    private void MainAffect2(ShipParameters shipparameters, ShipBase target, Bullet bullet1,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var ship = target;
        var maxShield = shipparameters.ShieldParameters.MaxShield;
        var countToHeal = maxShield * additional.CurrentDamage.ShieldDamage;
        ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
        shipparameters.ShieldParameters.HealShield(countToHeal);
        if (UpgradeType == ESpellUpgradeType.B2)
        {
            if (!ship.DamageData.IsReflecOn)
            {
                ship.DamageData.TurnOnReflectFor(OFF_PERIOD);
            }
        }
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, 
        Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {

        if (UpgradeType == ESpellUpgradeType.A1)
        {
            var closestsShips = BattleController.Instance.GetAllShipsInRadius(target.Position, target.TeamIndex, ShowCircle);
            foreach (var ship in closestsShips)
            {
                MainAffect2(ship.ShipParameters, ship, null, null, additional);
            }
        }
        else
        {
            MainAffect2(shipparameters, target, null, null, additional);
        }
    }
    public override bool ShowLine => false;

    public override float ShowCircle
    {
        get
        {
            if (UpgradeType == ESpellUpgradeType.A1)
            {
                return AOE_rad;
            }
            return rad;
        }
    }
    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.nextFrameRepair);
        DataBaseController.Instance.Pool.RegisterBullet(bullet);
        return bullet;
    }

    protected override void CastAction(Vector3 pos)
    {
    }

    public override SubUpdateShowCast SubUpdateShowCast => ShowOnShip;

    public override CanCastAtPoint CanCastAtPoint
    {
        get { return pos => _lastCheckIsOk; }
    }

    protected void ShowOnShip(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow)
    {
        var closestsShip = BattleController.Instance.ClosestShipToPos(pos, teamIndex, out float sDist);
        if (sDist < _sDistToShoot && closestsShip != null)
        {
            _lastCheckIsOk = true;
            objectToShow.gameObject.SetActive(true);
            objectToShow.transform.position = closestsShip.Position;
//            _lastClosest = closestsShip;
        }
        else
        {
//            _lastClosest = null;
            _lastCheckIsOk = false;
            objectToShow.gameObject.SetActive(false);
        }
    }



    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(ShowCircle,false);
    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("RechargeSheildSpell"), Utils.FloatToChance(HealPercent));
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("RechargeSheildNameA1");
        }
        return Namings.Tag("RechargeSheildNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Format(Namings.Tag("RechargeSheildDescA1"), AOE_rad);
        }
        return Namings.Format(Namings.Tag("RechargeSheildDescB2"), OFF_PERIOD);
    }
}

