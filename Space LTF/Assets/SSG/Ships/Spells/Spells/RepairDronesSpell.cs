using System;
using UnityEngine;


[System.Serializable]
public class RepairDronesSpell : BaseSpellModulInv
{
    public const float HEAL_PERCENT = 0.28f;
    public const float MINES_DIST = 7f;
    private const float rad = 1f;

    private const float _sDistToShoot = 4 * 4;
    private bool _lastCheckIsOk = false;
    [field: NonSerialized]
    private ShipBase _lastClosest;

    private int DronesCount => 1;//DRONES_COUNT + Level/2;
    private float HealPercent => HEAL_PERCENT + Level * 0.16f;
    private float HealPerTick => 8 + Level * 2;

    public RepairDronesSpell()
        : base(SpellType.repairDrones, 3, 30,
             new BulleStartParameters(15f, 46f, MINES_DIST, MINES_DIST), false)
    {

    }
    protected override CreateBulletDelegate createBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, BulleStartParameters bullestartparameters)
    {
        if (_lastClosest != null)
        {
            MainCreateBullet(new BulletTarget(_lastClosest), origin, weapon, shootPos, bullestartparameters);
//            MainAffect(_lastClosest.ShipParameters, _lastClosest, null, null, null);
        }
//        var dir = (target.Position - weapon.CurPosition);
//        MainCreateBullet(new BulletTarget(dir + weapon.CurPosition), origin, weapon, shootPos, bullestartparameters);
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var dir = (target.target.Position - weapon.CurPosition);
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, target.target, BulleStartParameters);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        var addHealth = shipparameters.MaxHealth * HealPercent;
        shipparameters.HealthRegen.Start(addHealth, HealPerTick);
    }
    public override bool ShowLine => false;
    public override float ShowCircle => rad;
    public override Bullet GetBulletPrefab()
    {
        var bullet = DataBaseController.Instance.GetBullet(WeaponType.spellRerairDrone);
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
            _lastClosest = closestsShip;
        }
        else
        {
            _lastClosest = null;
            _lastCheckIsOk = false;
            objectToShow.gameObject.SetActive(false);
        }
    }


    public override SpellDamageData RadiusAOE()
    {
        return new SpellDamageData(rad,false);
    }   
    public override string Desc()
    {
        return String.Format(Namings.RepairDroneSpell, DronesCount, Utils.FloatToChance(HealPercent));
        //            $"Set {MinesCount} mines for {MineFieldSpell.MINES_PERIOD.ToString("0")} sec to selected location. Each mine damage {damageShield}/{damageBody}";
    }
}

