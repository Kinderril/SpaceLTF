using System;
using UnityEngine;


[System.Serializable]
public class RepairDronesSpell : BaseSpellModulInv
{
    //A1 - shield
    //B2 - speedBuff x sec

    public const float HEAL_PERCENT = 0.28f;
    public const float SHIELD_PERCENT = 0.40f;
    public const float MINES_DIST = 7f;
    private const float rad = 1f;
    private const float BUFF_TIME = 13f;

    private const float _sDistToShoot = 4 * 4;
    private bool _lastCheckIsOk = false;
    [field: NonSerialized]
    private ShipBase _lastClosest;

    private int DronesCount => 1;//DRONES_COUNT + Level/2;
    private float HealPercent => CalcHealPercent(Level);
    private float HealPerTick => 8 + Level * 2;

    private float CalcHealPercent(int l)
    {
        return HEAL_PERCENT + l * 0.16f;
    }
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(HealPercent, HealPerTick);

    public RepairDronesSpell()
        : base(SpellType.repairDrones, 3, 30,
             new BulleStartParameters(15f, 46f, MINES_DIST, MINES_DIST), false,TargetType.Ally)
    {

    }
    public override ShallCastToTaregtAI ShallCastToTaregtAIAction => shallCastToTaregtAIAction;

    private bool shallCastToTaregtAIAction(ShipPersonalInfo info, ShipBase ship)
    {
        var p = ship.ShipParameters.CurHealth / ship.MaxSpeed();

        if (p < .5f)
        {
            return true;
        }

        return false;
    }

    protected override CreateBulletDelegate standartCreateBullet => MainCreateBullet;
    protected override CastActionSpell castActionSpell => CastSpell;
    protected override AffectTargetDelegate affectAction => MainAffect;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {
        if (_lastClosest != null)
        {
            target = new BulletTarget(_lastClosest);
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
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        Vector3 dir = target.target!=null?(target.target.Position - weapon.CurPosition): (target.Position - weapon.CurPosition);
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, target.target, BulleStartParameters);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        target.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
        var addHealth = shipparameters.MaxHealth * HealPercent;
        shipparameters.HealthRegen.Start(addHealth, HealPerTick);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                var maxShield = target.ShipParameters.ShieldParameters.MaxShield;
                var countToHeal = maxShield * HealPercent * SHIELD_PERCENT;
                target.ShipParameters.ShieldParameters.HealShield(countToHeal);
                break;
            case ESpellUpgradeType.B2:
                target.BuffData.Apply(BUFF_TIME);
                break;
        }
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
        return new SpellDamageData(rad, false);
    }
    public override string Desc()
    {
        return Namings.Format(Namings.Tag("RepairDroneSpell"), DronesCount, Utils.FloatToChance(HealPercent));
    }
    public override string GetUpgradeName(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            return Namings.Tag("RepairDroneNameA1");
        }
        return Namings.Tag("RepairDroneNameB2");
    }
    public override string GetUpgradeDesc(ESpellUpgradeType type)
    {
        if (type == ESpellUpgradeType.A1)
        {
            var p = CalcHealPercent(Library.MAX_SPELL_LVL) * SHIELD_PERCENT;
            return Namings.Format(Namings.Tag("RepairDroneDescA1"), Utils.FloatToChance(p));
        }
        return Namings.Format(Namings.Tag("RepairDroneDescB2"), BUFF_TIME);
    }
}

