[System.Serializable]
public class BaseModulInv : IItemInv
{
    public SimpleModulType Type;
    public int Level = 1;
    private int _baseRequreLevel = 0;
    public string Name { get; set; }

    public virtual bool IsSupport => false;
#if UNITY_EDITOR
    public int UnityEditorID = 10;
#endif
    public BaseModulInv(SimpleModulType type, int level)
    {
#if UNITY_EDITOR
        UnityEditorID = 1000 + Utils.GetId();
#endif
        Type = type;
        Level = level;
        Name = Namings.SimpleModulName(type);
        _baseRequreLevel = GetBaseReuire(type);
    }

    public static int GetBaseReuire(SimpleModulType type)
    {
        switch (type)
        {
            case SimpleModulType.antiPhysical:
            case SimpleModulType.antiEnergy:
            case SimpleModulType.WeaponSpeed:
            case SimpleModulType.WeaponDist:
            case SimpleModulType.ShipSpeed:
            case SimpleModulType.ShipTurnSpeed:
            case SimpleModulType.WeaponSector:
                return 0;
//            case SimpleModulType.closeStrike:
            case SimpleModulType.shieldRegen:
            case SimpleModulType.systemMines:
            case SimpleModulType.damageMines:
            case SimpleModulType.fireMines:
            case SimpleModulType.WeaponShield:
            case SimpleModulType.WeaponLessDist:
                // case SimpleModulType.WeaponWeapon:
                return 2;
            case SimpleModulType.WeaponAOE:
            case SimpleModulType.WeaponNoBulletDeath:
            case SimpleModulType.WeaponFireNear:
            case SimpleModulType.frontShield:
            case SimpleModulType.ResistDamages:
            case SimpleModulType.autoShieldRepair:
            case SimpleModulType.WeaponPush:
                return 3;
            case SimpleModulType.shieldLocker:
            case SimpleModulType.engineLocker:
            case SimpleModulType.WeaponFire:
            case SimpleModulType.WeaponEngine:
            case SimpleModulType.ShieldDouble:
                return 4;
            case SimpleModulType.WeaponSpray:
            case SimpleModulType.armor:
            case SimpleModulType.blink:
            case SimpleModulType.WeaponCrit:
            case SimpleModulType.WeaponPowerShot:
            case SimpleModulType.WeaponMultiTarget:
                return 5;
            case SimpleModulType.ShipDecreaseSpeed:
            case SimpleModulType.WeaponSelfDamage:
            case SimpleModulType.WeaponShootPerTime:
            case SimpleModulType.WeaponShieldIgnore:
            case SimpleModulType.autoRepair:
            case SimpleModulType.WeaponShieldPerHit:
                return 6;
            case SimpleModulType.laserUpgrade:
            case SimpleModulType.rocketUpgrade:
            case SimpleModulType.impulseUpgrade:
            case SimpleModulType.bombUpgrade:
            case SimpleModulType.EMIUpgrade:
            case SimpleModulType.beamUpgrade:
                return 9;
        }
        return 0;
    }


    public int CostValue
    {
        get
        {
            var coef = BaseModulInv.GetBaseReuire(Type);
            //            if (LibraryModuls.(Type))
            //            {
            //
            //                return MoneyConsts.MODUL_RARE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1);
            //            }
            //            else
            //            {
            //                return (MoneyConsts.MODUL_BASE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1));
            //            }              
            return MoneyConsts.MODUL_BASE_MONEY_COST + (int)(coef * MoneyConsts.MODUL_BASE_MONEY_COST_MID) + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1);

        }
    }

    public int RequireLevel(int posibleLevel = -1)
    {
        if (posibleLevel <= 0)
        {
            posibleLevel = Level;
        }
        return 1 + (posibleLevel - 1) * Library.MODUL_REQUIRE_LEVEL_COEF + _baseRequreLevel;
    }

    public string GetInfo()
    {
        return Name + " (" + Level + ")";
    }

    public string WideInfo()
    {
        return GetInfo() + "\n" + GetDesc();
    }

    public IItemInv Copy()
    {
        return new BaseModulInv(Type, Level);
    }

    public virtual string GetDesc()
    {
        return Namings.DescSimpleModul(Type);
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

