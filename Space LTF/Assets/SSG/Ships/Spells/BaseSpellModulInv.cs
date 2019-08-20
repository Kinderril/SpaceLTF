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
    randomDamage = 8,

//    spaceWall = 9,//DO NOT USE!
    distShot = 10,
    priorityTarget = 11,
    artilleryPeriod = 12,
    BaitPriorityTarget = 13,
}


[System.Serializable]
public abstract class BaseSpellModulInv: IItemInv  , IAffectable , ISpellToGame , IAffectParameters
{
    public int CostCount { get; protected set; }
    public int CostTime { get; protected set; }
    public int Level { get; protected set; }
    public bool IsHoming { get; protected set; }
    //    private float _nextPosibleCast;
    public SpellType SpellType { get; private set; }

    [field: NonSerialized] public event Action<BaseSpellModulInv> OnUpgrade;

    //    [field: NonSerialized]
    //    public ShipBase ShipBase { get; private set; }
    //    protected int WeaponSlot { get; private set; }
    //    [field: NonSerialized]
    //    protected Transform ModulPos { get; private set; }
    protected CreateBulletDelegate CreateBullet { get; private set; }



    public string Name
    {
        get { return Namings.SpellName(SpellType); }
    }

    public BaseSpellModulInv(IInventory currentInventory)
    {
        CurrentInventory = currentInventory;
    }



    public ItemType ItemType => ItemType.spell;

    public IInventory CurrentInventory { get; set; }


    protected BaseSpellModulInv(SpellType spell,int costCount, int costTime, 
                 BulleStartParameters bulleStartParameters,bool isHoming)
    {
        Level = 1;
        IsHoming = isHoming;
        CastSpell = castActionSpell;
        BulleStartParameters = bulleStartParameters;
        AffectAction =  new WeaponInventoryAffectTarget(affectAction);
        CreateBullet = createBullet;
        SpellType = spell;
        CostCount = costCount;
        CostTime = costTime;
    }

    protected abstract CreateBulletDelegate createBullet { get; }
    protected abstract CastActionSpell castActionSpell { get; }
    protected abstract AffectTargetDelegate affectAction  { get; }

    public virtual bool TryCast(CommanderCoinController coinController,Vector3 pos)
    {
//        _nextPosibleCast = Time.time + CostTime;
        coinController.UseCoins(CostCount,CostTime);
        CastAction(pos);
        return true;
    }

    public abstract Bullet GetBulletPrefab();
    public abstract float ShowCircle { get; }
    public abstract bool ShowLine { get; }


    protected abstract void CastAction(Vector3 pos);

    public abstract string Desc();


    public int CostValue { get { return MoneyConsts.SPELL_BASE_MONEY_COST; } }

    public string GetInfo()
    {
        return Name;// + " (" + 1 + ")";
    }

    public string WideInfo()
    {
        string cost = String.Format(Namings.SpellModulChargers, CostCount, CostTime);
        return GetInfo()  + "\n" + cost
             + "\n" + Desc();
    }

//    protected abstract void CastAction(Vector3 v);
         
    public BulleStartParameters BulleStartParameters { get; private set; }
    public WeaponInventoryAffectTarget AffectAction { get; private set; }
    public CastActionSpell CastSpell { get; private set; }
    public CreateBulletDelegate CreateBulletAction => CreateBullet;
    public virtual CurWeaponDamage CurrentDamage => new CurWeaponDamage(0,0);
    public float AimRadius { get; set; }
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }

    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        CreateBullet = bulletCreate;
    }
    public virtual SpellDamageData RadiusAOE()
    {
        return new SpellDamageData();
    }
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
                    var txt = String.Format("You want to upgrade {0}", Namings.SpellName(SpellType));
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
            if (OnUpgrade != null)
            {
                OnUpgrade(this);
            }
        }
    }
}