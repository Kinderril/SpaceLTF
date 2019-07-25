using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



[System.Serializable]
public class BaseModulInv : IItemInv
{
//    public bool SupportType;
    public SimpleModulType Type;
    public int Level = 1;
    public string Name { get; set; }

    public virtual bool IsSupport => false;
    public BaseModulInv(SimpleModulType type,int level)
    {
//        SupportType = supportType;
        Type = type;
        Level = level;
        Name = Namings.SimpleModulName(type);
    }


    public int CostValue
    {
        get
        {
            switch (Type)
            {
                case SimpleModulType.autoRepair:
                case SimpleModulType.autoShieldRepair:
                case SimpleModulType.closeStrike:
                case SimpleModulType.shieldRegen:
                case SimpleModulType.antiPhysical:
                case SimpleModulType.antiEnergy:
                case SimpleModulType.shieldLocker:
                case SimpleModulType.engineLocker:
                case SimpleModulType.damageMines:
                case SimpleModulType.systemMines:
                case SimpleModulType.ShipSpeed:
                case SimpleModulType.blink:
                    return (MoneyConsts.MODUL_BASE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1));
                case SimpleModulType.laserUpgrade:
                case SimpleModulType.bombUpgrade:
                case SimpleModulType.EMIUpgrade:
                case SimpleModulType.rocketUpgrade:
                case SimpleModulType.impulseUpgrade:
                case SimpleModulType.ResistDamages:
                    return 2 * (MoneyConsts.MODUL_BASE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1));
                case SimpleModulType.WeaponSpeed:
                case SimpleModulType.WeaponSpray:
                case SimpleModulType.WeaponDist:
                case SimpleModulType.WeaponPush:
                case SimpleModulType.WeaponFire:
                case SimpleModulType.WeaponEngine:
                case SimpleModulType.WeaponShield:
                case SimpleModulType.WeaponWeapon:
                case SimpleModulType.WeaponCrit:
                case SimpleModulType.WeaponAOE:
                case SimpleModulType.WeaponSector:
                case SimpleModulType.WeaponChain:
                case SimpleModulType.WeaponLessDist:
                case SimpleModulType.WeaponShieldIgnore:
                case SimpleModulType.WeaponSelfDamage:
                case SimpleModulType.WeaponShieldPerHit:
                case SimpleModulType.WeaponNoBulletDeath:
                case SimpleModulType.WeaponPowerShot:
                case SimpleModulType.WeaponFireNear:
                    return MoneyConsts.MODUL_SUPPORT_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1);
            }
            UnityEngine.Debug.LogError("CAn't find cost for modul type: " + Type.ToString());
            return MoneyConsts.MODUL_BASE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1);
        }
    }

    public string GetInfo()
    {
        return Name + " (" + Level + ")";
    }

    public string WideInfo()
    {
        return GetInfo() + "\n" + Namings.DescSimpleModul(Type);
    }

    public ItemType ItemType
    {
        get { return ItemType.modul; }
    }

    public IInventory CurrentInventory { get; set; }

    public void Upgrade()
    {
        if (CanUpgradeLevel())
            Level++;
    }
    public bool CanUpgradeLevel()
    {
        return Level < Library.MAX_MOUDL_LEVEL;
    }
}

