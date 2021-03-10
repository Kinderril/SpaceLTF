using System;
using UnityEngine;


[System.Serializable]
public class RepairDronesSpell : BaseSpellModulInv
{
    //A1 - shield
    //B2 - speedBuff x sec

    public const float HEAL_PERCENT = 0.16f;
    public const float SHIELD_PERCENT = 0.40f;
    public const float MINES_DIST = 57f;
    private const float rad = 1f;
    private const float BUFF_TIME = 13f;
    private const float CAST_COEF = CoinTempController.BATTERY_PERIOD * .5f;

    private const float _sDistToShoot = 4 * 4;
    private int DronesCount => 1;//DRONES_COUNT + Level/2;
    private float HealPercent => CalcHealPercent(Level);
    private float HealPerTick => 8 + Level * 2;

    private float _nextBulletTime;

    private float CalcHealPercent(int l)
    {
        return HEAL_PERCENT + l * 0.12f * CAST_COEF;
    }
    public override CurWeaponDamage CurrentDamage => new CurWeaponDamage(HealPercent, HealPerTick);

    public RepairDronesSpell()
        : base(SpellType.repairDrones,  12,
             new BulleStartParameters(3f, 46f, MINES_DIST, MINES_DIST), false,TargetType.Ally)
    {
        _localSpellDamageData = new SpellDamageData(rad, false);
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
    public override UpdateCastDelegate UpdateCast => UpdateCastInner;

    private void CastSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos, CastSpellData castData)
    {

    }

    private void UpdateCastInner(Vector3 trgpos,
        BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, CastSpellData castdata)
    {

        if (_nextBulletTime < Time.time)
        {
            var btl = BattleController.Instance;
            _nextBulletTime = CAST_COEF * CoinTempController.BATTERY_PERIOD + Time.time;
            for (int i = 0; i < castdata.ShootsCount; i++)
            {
                var battle = BattleController.Instance;
                if (battle.State == BattleState.process)
                {
                    var close = btl.GreenCommander.GetClosestShip(trgpos,false);
                    var nTargte = new BulletTarget(close);

                    // Debug.LogError("cat droid");
                    modificatedCreateBullet(nTargte, origin, weapon, weapon.CurPosition, castdata.Bullestartparameters);
                }
            }
        }
    }

    private void MainCreateBullet(BulletTarget target, Bullet origin, IWeapon weapon,
        Vector3 shootpos, BulleStartParameters bullestartparameters)
    {
        var xx = MyExtensions.Random(-1f, 1f);
        var zz = MyExtensions.Random(-1f, 1f);
        Vector3 dir = Utils.NormalizeFastSelf(new Vector3(xx, 00, zz));
        var copy = bullestartparameters.Copy();
        var coef = PowerInc();


        copy.turnSpeed = copy.turnSpeed * coef;
        Bullet.Create(origin, weapon, dir, weapon.CurPosition, target.target, copy);
    }

    private void MainAffect(ShipParameters shipparameters, ShipBase target, Bullet bullet1, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        target.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
        var coef = PowerInc();
        var clmap = Mathf.Clamp(coef, 1f, 3f);
        var addHealth = shipparameters.MaxHealth * additional.CurrentDamage.ShieldDamage * clmap;
        shipparameters.HealthRegen.Start(addHealth, additional.CurrentDamage.BodyDamage);
        switch (UpgradeType)
        {
            case ESpellUpgradeType.A1:
                var maxShield = target.ShipParameters.ShieldParameters.MaxShield;
                var countToHeal = maxShield * additional.CurrentDamage.ShieldDamage * SHIELD_PERCENT;
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
        get { return pos => true; }
        // get { return pos => _lastCheckIsOk; }
    }

    protected void ShowOnShip(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow)
    {
   
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

