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
    private const float AOE_rad = 2f;
    private const float PERIOD_COEF = 0.5f;

    private const float _sDistToShoot = 4 * 4;
    private bool _lastCheckIsOk = false;
    private float _nextBulletTime;
    // [field: NonSerialized]
//    private ShipBase _lastClosest;
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(HealPercent, HealPercent);

    private float HealPercent => (Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT + Level * 0.12f) * PERIOD_COEF;
    public RechargeShieldSpell()
        : base(SpellType.rechargeShield, 20,
             new BulleStartParameters(4f, 46f, 
                 MINES_DIST, MINES_DIST), false,TargetType.Ally)
    {

        _localSpellDamageData =  new SpellDamageData(ShowCircle, false);
    }
    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;
    public override UpdateCastDelegate UpdateCast => UpdateCastInner;
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
        _localSpellDamageData.AOERad = ShowCircle;
    }


    private void UpdateCastInner(Vector3 trgpos,
        BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        var p = PowerInc();
        _localSpellDamageData.AOERad = ShowCircle * p;
        if (_nextBulletTime < Time.time)
        {
            _nextBulletTime = Time.time + CoinTempController.BATTERY_PERIOD * PERIOD_COEF;
            castData.Bullestartparameters.size = castData.Bullestartparameters.size * p;
            modificatedCreateBullet(target, origin, weapon, shootPos, castData.Bullestartparameters);
            EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.ShieldRecharge,
                target.Position, 1f,p);
        }
    }

    protected override void EndCastSpell()
    {
        _localSpellDamageData.AOERad = ShowCircle;
        base.EndCastSpell();
    }

    public override Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return targetdistpos;
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var startPos = target.Position;
        var dir = startPos - weapon.CurPosition;
        bullestartparameters.distanceShoot = dir.magnitude;
        var b = Bullet.Create(origin, weapon, dir, startPos, null, bullestartparameters);
    }

    private void MainAffect2(ShipParameters shipparameters, ShipBase target, Bullet bullet1,
        DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var ship = target;

        var maxShield = shipparameters.ShieldParameters.MaxShield;
        var countToHeal = maxShield * additional.CurrentDamage.ShieldDamage * 0.35f;
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
        var rad2 = ShowCircle;
        var closestsShips = BattleController.Instance.GetAllShipsInRadius(target.Position, target.TeamIndex, rad2);
        foreach (var ship in closestsShips)
        {
            MainAffect2(ship.ShipParameters, ship, null, null, additional);
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
        get { return pos => true; }
        // get { return pos => _lastCheckIsOk; }
    }

    protected void ShowOnShip(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow)
    {
        // _localSpellDamageData.AOERad = ShowCircle;
//         var closestsShip = BattleController.Instance.ClosestShipToPos(pos, teamIndex, out float sDist);
//         if (sDist < _sDistToShoot && closestsShip != null)
//         {
//             _lastCheckIsOk = true;
//             objectToShow.gameObject.SetActive(true);
//             objectToShow.transform.position = closestsShip.Position;
// //            _lastClosest = closestsShip;
//         }
//         else
//         {
// //            _lastClosest = null;
//             _lastCheckIsOk = false;
//             objectToShow.gameObject.SetActive(false);
//         }
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

