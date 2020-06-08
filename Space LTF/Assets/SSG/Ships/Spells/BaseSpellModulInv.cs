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
    // roundWave = 16,
    machineGun = 17,
    vacuum = 18,
    mainShipBlink = 19,
    hookShot = 20,
}

public enum ESpellUpgradeType
{
    None = 0,
    A1 = 1,
    B2 = 2,
}


[Serializable]
public abstract class BaseSpellModulInv : IItemInv, IAffectable, ISpellToGame, IAffectParameters
{
    protected BaseSpellModulInv(IInventory currentInventory)
    {
        CurrentInventory = currentInventory;
    }


    protected BaseSpellModulInv(SpellType spell, int costCount, int costTime,
        BulleStartParameters bulleStartParameters, bool isHoming, TargetType targetType)
    {
        Level = 1;
        IsHoming = isHoming;
        CastSpell = castActionSpell;
        BulleStartParameters = bulleStartParameters;
        AffectAction = new WeaponInventoryAffectTarget(affectAction, targetType);
        CreateBullet = createBullet;
        SpellType = spell;
        CostCount = costCount;
        CostTime = costTime;
        ShootPerTime = 1;
    }

    public virtual int CostCount { get; protected set; }
    public virtual int CostTime { get; protected set; }
    public int Level { get; protected set; }
    public bool IsHoming { get; protected set; }
    public SpellType SpellType { get; private set; }
    public ESpellUpgradeType UpgradeType { get; private set; }
    protected CreateBulletDelegate CreateBullet { get; private set; }
    public string Name => Namings.SpellName(SpellType);

    protected abstract CreateBulletDelegate createBullet { get; }
    protected abstract CastActionSpell castActionSpell { get; }
    protected abstract AffectTargetDelegate affectAction { get; }
    public WeaponInventoryAffectTarget AffectAction { get; private set; }
    public CreateBulletDelegate CreateBulletAction => CreateBullet;
    public abstract ShallCastToTaregtAI ShallCastToTaregtAIAction { get; }
    public virtual BulletDestroyDelegate BulletDestroyDelegate => null;

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
        var cost = Namings.Format(Namings.Tag("SpellModulChargers"), CostCount, CostTime);
        return GetInfo() + "\n" + cost
               + "\n" + DescFull();
    }

    public IItemInv Copy()
    {
        var spell = Library.CreateSpell(SpellType);
        spell.Level = Level;
        spell.UpgradeType = UpgradeType;
        return spell;
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

    public string DescFull()
    {
        switch (UpgradeType)
        {
            default:
            case ESpellUpgradeType.None:
                return Desc();
            case ESpellUpgradeType.A1:
            case ESpellUpgradeType.B2:
                var desc = Desc();
                var upgrade = GetUpgradeDesc(UpgradeType);
                return $"{desc}\n{upgrade}";
        }

    }

    public abstract string Desc();

    public void TryUpgrade(ESpellUpgradeType upgradeType)
    {
        var owner = CurrentInventory.Owner;
        if (CanUpgradeByLevel())
        {
            if (MoneyConsts.SpellUpgrade.ContainsKey(Level))
            {
                var cost = MoneyConsts.SpellUpgrade[Level];
                int microchipsElement = MoneyConsts.SpellMicrochipsElements[Level];
                if (owner.HaveMicrochips(microchipsElement))
                {
                    if (owner.HaveMoney(cost))
                    {
                        var txt = Namings.Format(Namings.Tag("wantUpgradeLong"), Namings.SpellName(SpellType), cost, microchipsElement);
                        WindowManager.Instance.ConfirmWindow.Init(() =>
                        {
                            owner.RemoveMicrochips(microchipsElement);
                            owner.RemoveMoney(cost);
                            Upgrade(upgradeType);
                        }, null, txt);
                    }
                    else
                    {
                        WindowManager.Instance.NotEnoughtMoney(cost);
                    }
                }
                else
                {
                    WindowManager.Instance.NotEnoughtUpgrades(microchipsElement);
                }
            }
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "Spell have max level");
        }
    }

    public bool CanUpgradeByLevel()
    {
        return Level < Library.MAX_SPELL_LVL;
    }


    public bool Upgrade(ESpellUpgradeType upgradeType)
    {
        if (CanUpgradeByLevel())
        {
            var isNextSpecial = ShallAddSpecialNextLevel();
            Level++;
            if (Level == Library.MAX_SPELL_LVL)
                MainController.Instance.Statistics.AddMaxLevelSpells();

            if (isNextSpecial)
            {
                if (upgradeType == ESpellUpgradeType.None)
                {
                    upgradeType = MyExtensions.IsTrueEqual() ? ESpellUpgradeType.A1 : ESpellUpgradeType.B2;
                }
                AddSpecial(upgradeType);
            }

            if (OnUpgrade != null)
                OnUpgrade(this);
            return true;
        }

        return false;
    }

    public bool ShallAddSpecialNextLevel()
    {
        return Level + 1 == Library.SPECIAL_SPELL_LVL;
    }

    private void AddSpecial(ESpellUpgradeType upgradeType)
    {
        // CostCount++;
        AddSpecialAction(upgradeType);
    }

    protected void AddSpecialAction(ESpellUpgradeType upgradeType)
    {
        UpgradeType = upgradeType;
    }

    public virtual Vector3 DiscCounter(Vector3 maxdistpos, Vector3 targetdistpos)
    {
        return maxdistpos;
    }

    public abstract string GetUpgradeName(ESpellUpgradeType type);

    public abstract string GetUpgradeDesc(ESpellUpgradeType type);
}