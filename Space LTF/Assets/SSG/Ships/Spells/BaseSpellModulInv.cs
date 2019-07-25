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
}


[System.Serializable]
public abstract class BaseSpellModulInv: IItemInv  , IAffectable , ISpellToGame , IAffectParameters
{
    public int CostCount { get; protected set; }
    public int CostTime { get; protected set; }
    public bool IsHoming { get; protected set; }
    //    private float _nextPosibleCast;
    public SpellType SpellType { get; private set; }
  
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



    public ItemType ItemType
    {
        get
        {
            return ItemType.spell;
        }
    }

    public IInventory CurrentInventory { get; set; }


    protected BaseSpellModulInv(SpellType spell,int costCount, int costTime, 
        CreateBulletDelegate createBullet, AffectTargetDelegate affectAction, 
        BulleStartParameters bulleStartParameters,bool isHoming)
    {
        IsHoming = isHoming;
        BulleStartParameters = bulleStartParameters;
        AffectAction =  new WeaponInventoryAffectTarget(affectAction);
        CreateBullet = createBullet;
        SpellType = spell;
        CostCount = costCount;
        CostTime = costTime;
    }

    public virtual bool TryCast(CommanderCoinController coinController,Vector3 pos)
    {
//        _nextPosibleCast = Time.time + CostTime;
        coinController.UseCoins(CostCount,CostTime);
        CastAction(pos);
        return true;
    }

    public abstract Bullet GetBulletPrefab();


    protected abstract void CastAction(Vector3 pos);


    public int CostValue { get { return MoneyConsts.SPELL_BASE_MONEY_COST; } }

    public string GetInfo()
    {
        return Name;// + " (" + 1 + ")";
    }

    public string WideInfo()
    {
        string cost = String.Format("Charges require: {0} with delay {1} sec.",CostCount,CostTime);
        return GetInfo()  + "\n" + cost
             + "\n" + Namings.SpellDesc(SpellType);
    }
         
    public BulleStartParameters BulleStartParameters { get; private set; }
    public WeaponInventoryAffectTarget AffectAction { get; private set; }
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
}