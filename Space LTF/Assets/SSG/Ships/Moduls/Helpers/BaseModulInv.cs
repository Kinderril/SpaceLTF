using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



[System.Serializable]
public class BaseModulInv : IItemInv
{
    public SimpleModulType Type;
    public int Level = 1;
    public string Name { get; set; }

    public virtual bool IsSupport => false;
#if UNITY_EDITOR
     public int UnityEditorID = 10;
#endif
    public BaseModulInv(SimpleModulType type,int level)
    {
#if UNITY_EDITOR
        UnityEditorID = 1000 + Utils.GetId();
#endif
        Type = type;
        Level = level;
        Name = Namings.SimpleModulName(type);
    }


    public int CostValue
    {
        get
        {
            if (LibraryModuls.IsRare(Type))
            {

                return MoneyConsts.MODUL_RARE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1);
            }
            else
            {
                return (MoneyConsts.MODUL_BASE_MONEY_COST + MoneyConsts.MODUL_LEVEL_MONEY_COST * (Level - 1));
            }
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

