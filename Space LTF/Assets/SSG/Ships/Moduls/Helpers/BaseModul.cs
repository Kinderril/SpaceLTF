using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum SimpleModulType
{
    autoRepair = 0,
    autoShieldRepair = 1,

    closeStrike = 6,
    shieldRegen = 2,

    antiPhysical = 4,
    antiEnergy = 5,

    shieldLocker = 3,
    engineLocker = 7,

    damageMines = 8,
    systemMines = 9,

    blink = 10,

    laserUpgrade = 11,
    rocketUpgrade = 12,
    impulseUpgrade = 13,
    bombUpgrade = 14,
    EMIUpgrade = 15,

    ShipSpeed = 16,
    ShipTurnSpeed = 17,

    WeaponSpeed = 18,
    WeaponSpray = 19,
    WeaponDist = 20,

    WeaponPush = 21,
    WeaponFire = 22,
    WeaponEngine = 23,
    WeaponShield = 24,
    // WeaponWeapon = 25,

    WeaponCrit = 26,
    WeaponAOE = 27,
    WeaponSector = 28,
    WeaponLessDist = 29,
    WeaponChain = 30,

    WeaponShieldIgnore = 31,
    WeaponSelfDamage = 32,
    WeaponShieldPerHit = 33,
    WeaponNoBulletDeath = 34,
    WeaponPowerShot = 35,

    WeaponFireNear = 36,
    ResistDamages = 37,
    beamUpgrade = 38,
    ShipDecreaseSpeed = 39,
    ShieldDouble = 40,
    WeaponShootPerTime = 41,
    fireMines = 42,
    frontShield = 43,
    armor = 44,
}

public abstract class BaseModul
{
    public BaseModulInv ModulData;
    public float NextPosibleAction { get; protected set; }
    public float Period { get; protected set; }

    public bool _isApply = false;
    public bool IsApply
    {
        get { return _isApply; }
        private set
        {
            _isApply = value;
        }
    }

    protected float _delta;
    protected ShipParameters _parameters;
    protected ShipBase _owner;                             
    
    public static BaseModul Create(BaseModulInv baseModulInv)
    {
        switch (baseModulInv.Type)
        {
            case SimpleModulType.shieldRegen:
                return new ShieldRegenModul(baseModulInv);
            case SimpleModulType.antiPhysical:
                return new AntiPhysicalModul(baseModulInv);
            case SimpleModulType.antiEnergy:
                return new AntiEnergyModul(baseModulInv);
            case SimpleModulType.blink:
                return new BlinkModul(baseModulInv);
            case SimpleModulType.frontShield:
                return new FrontShieldModul(baseModulInv);      
            case SimpleModulType.armor:
                return new ArmorModul(baseModulInv);
            case SimpleModulType.autoRepair:
                return new AutoRepairModul(baseModulInv);
            case SimpleModulType.autoShieldRepair:
                return new AutoShieldRepairModul(baseModulInv);
            case SimpleModulType.shieldLocker:
                return new ShieldLocker(baseModulInv);
            case SimpleModulType.closeStrike:
                return new CloseStrikeModul(baseModulInv);
            case SimpleModulType.engineLocker:
                return new EngineLocker(baseModulInv);
            case SimpleModulType.damageMines:
                return new MineDamageModul(baseModulInv);
            case SimpleModulType.fireMines:
                return new MineFireModul(baseModulInv);
            case SimpleModulType.systemMines:
                return new MineSystemModul(baseModulInv);
//            case SimpleModulType.ShipSpeed:
//                return new ShipSpeedModul(baseModulInv);
//            case SimpleModulType.ShipTurnSpeed:
//                return new ShipTurnSpeedModul(baseModulInv);     
            case SimpleModulType.ResistDamages:
                return new ResistDamagesModul(baseModulInv);
            default:
                Debug.LogError($"Can't cerate base modul {baseModulInv.Type.ToString()}");
                throw new ArgumentOutOfRangeException(baseModulInv.Type.ToString(), baseModulInv.Type, null);
        }
        return null;
    }

    public BaseModul(BaseModulInv baseModulInv)
    {
        ModulData = baseModulInv;
    }

    public bool IsReady()
    {
        return NextPosibleAction < Time.time;
    }

    public void Use()
    {
        NextPosibleAction = Time.time + Period;
    }

    public float PercentReady()
    {
        var d = NextPosibleAction - Time.time;
        return  1f - d /Period;
    }

    public virtual void Apply(ShipParameters Parameters, ShipBase owner)
    {
        if (IsApply)
        {
            Debug.LogError("can't apply twise");
            return;
        }
        _owner = owner;
        _parameters = Parameters;
        IsApply = true;
    }

    public void Crash()
    {
        const float CRASH_TIME = 5f;
        if (IsReady())
        {
            NextPosibleAction = Time.time + CRASH_TIME;
        }
        else
        {
            NextPosibleAction = NextPosibleAction + CRASH_TIME;
        }
    }

    protected void UpdateTime()
    {
        if (IsReady())
        {
            NextPosibleAction = Time.time + Period;
        }
        else
        {
            NextPosibleAction = NextPosibleAction + Period;
        }
    }
    
    public virtual void Dispose()
    {
    }

    public virtual void Delete()
    {
        if (!IsApply)
        {
            Debug.LogError("can't delete twise");
            return;
        }
        if (_owner.ModulEffectDestroy != null)
        {
            _owner.ModulEffectDestroy.Play();
        }
        IsApply = false;
    }

    public virtual void UpdateBattle()
    {
        
    }
}

