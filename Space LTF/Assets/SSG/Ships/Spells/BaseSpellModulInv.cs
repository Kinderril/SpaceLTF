using System;
using UnityEngine;

public enum SpellType
{
    shildDamage = 1,
    engineLock = 2,

    lineShot = 3,
    throwAround = 4,

    //    allToBase = 5,
    //    invisibility =6,//DO NOT USE!

    mineField = 7,
    // randomDamage = 8,

    //    spaceWall = 9,//DO NOT USE!
    distShot = 10,
    priorityTarget = 11,

    artilleryPeriod = 12,
    BaitPriorityTarget = 13,
    repairDrones = 14,

    rechargeShield = 15,
    roundWave = 16,
    machineGun = 17,
}


[Serializable]
public abstract class BaseSpellModulInv : IItemInv, IAffectable, ISpellToGame, IAffectParameters
{
    public BaseSpellModulInv(IInventory currentInventory)
    {
        CurrentInventory = currentInventory;
    }


    protected BaseSpellModulInv(SpellType spell, int costCount, int costTime,
        BulleStartParameters bulleStartParameters, bool isHoming)
    {
        Level = 1;
        IsHoming = isHoming;
        CastSpell = castActionSpell;
        BulleStartParameters = bulleStartParameters;
        AffectAction = new WeaponInventoryAffectTarget(affectAction);
        CreateBullet = createBullet;
        SpellType = spell;
        CostCount = costCount;
        CostTime = costTime;
        ShootPerTime = 1;
    }

    public int CostCount { get; protected set; }
    public int CostTime { get; protected set; }
    public int Level { get; protected set; }
    public bool IsHoming { get; protected set; }
    public bool HaveSpecial { get; protected set; }
    public SpellType SpellType { get; private set; }
    protected CreateBulletDelegate CreateBullet { get; private set; }
    public string Name => Namings.SpellName(SpellType);

    protected abstract CreateBulletDelegate createBullet { get; }
    protected abstract CastActionSpell castActionSpell { get; }
    protected abstract AffectTargetDelegate affectAction { get; }
    public WeaponInventoryAffectTarget AffectAction { get; private set; }
    public CreateBulletDelegate CreateBulletAction => CreateBullet;

    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        CreateBullet = bulletCreate;
    }

    public virtual CurWeaponDamage CurrentDamage => new CurWeaponDamage(0, 0);
    public float AimRadius { get; set; }
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }
    public int ShootPerTime { get; set; }
    public ItemType ItemType => ItemType.spell;

    public IInventory CurrentInventory { get; set; }


    public int CostValue => MoneyConsts.SPELL_BASE_MONEY_COST;

    public int RequireLevel(int posiblLevel)
    {
        return 1;
    }

    public string GetInfo()
    {
        return Name; // + " (" + 1 + ")";
    }

    public string WideInfo()
    {
        var cost = string.Format(Namings.SpellModulChargers, CostCount, CostTime);
        return GetInfo() + "\n" + cost
               + "\n" + Desc();
    }

    public abstract Bullet GetBulletPrefab();
    public abstract float ShowCircle { get; }
    public abstract bool ShowLine { get; }
    public virtual SubUpdateShowCast SubUpdateShowCast { get; }

    public virtual CanCastAtPoint CanCastAtPoint
    {
        get { return pos => true; }
    }

    //    protected abstract void CastAction(Vector3 v);

    public BulleStartParameters BulleStartParameters { get; private set; }
    public CastActionSpell CastSpell { get; private set; }

    public virtual SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }

    [field: NonSerialized] public event Action<BaseSpellModulInv> OnUpgrade;

    protected abstract void CastAction(Vector3 pos);

    public abstract string Desc();

    public void TryUpgrade()
    {
        var owner = CurrentInventory.Owner;
        if (CanUpgrade())
        {
            if (MoneyConsts.SpellUpgrade.ContainsKey(Level))
            {
                var cost = MoneyConsts.SpellUpgrade[Level];
                if (owner.MoneyData.HaveMoney(cost))
                {
                    var txt = string.Format("You want to upgrade {0}", Namings.SpellName(SpellType));
                    WindowManager.Instance.ConfirmWindow.Init(() =>
                    {
                        owner.MoneyData.RemoveMoney(cost);
                        Upgrade();
                    }, null, txt);
                }
                else
                {
                    WindowManager.Instance.NotEnoughtMoney(cost);
                }
            }
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "Spell have max level");
        }
    }

    public bool CanUpgrade()
    {
        return Level < Library.MAX_SPELL_LVL;
    }


    public void Upgrade()
    {
        if (CanUpgrade())
        {
            Level++;
            if (Level == Library.MAX_SPELL_LVL)
                MainController.Instance.Statistics.AddMaxLevelSpells();

            if (Level == 1 + Library.MAX_SPELL_LVL / 2)
            {
                AddSpecial();
            }

            if (OnUpgrade != null)
                OnUpgrade(this);
        }
    }

    private void AddSpecial()
    {
        CostCount++;
        HaveSpecial = true;
        AddSpecialAction();
    }

    protected virtual void AddSpecialAction()
    {

    }

    public virtual Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return maxdistpos;
    }
}