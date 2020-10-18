using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class PlayerInventory : IInventory
{

    public int MaxSlots = 48;
    public List<BaseModulInv> Moduls = new List<BaseModulInv>();
    public List<WeaponInv> Weapons = new List<WeaponInv>();
    public List<BaseSpellModulInv> Spells = new List<BaseSpellModulInv>();
    public List<ParameterItem> ParamItems = new List<ParameterItem>();


    [field: NonSerialized]
    public event ItemTransferedTo OnItemAdded;
    private PlayerSafe _player;

    public PlayerInventory(PlayerSafe player,int maxSlots)
    {
        MaxSlots = maxSlots;
        _player = player;
        Moduls.Clear();
        Weapons.Clear();
        Spells.Clear();
        ParamItems.Clear();
    }

    public void TransferItem(IItemInv item, bool val)
    {
        if (item == null)
        {
            Debug.LogError("Tyr to transfer zero item");
            return;
        }
        Debug.Log($"TransferItem {item.WideInfo()}  {val}");
        if (val)
            item.CurrentInventory = this;
        OnItemAdded?.Invoke(item, val);
    }
    [CanBeNull]
    public PlayerSafe Owner => _player;

    public int SlotsCount => MaxSlots;

    public virtual bool IsShop()
    {
        return false;
    }

    public bool CanMoveToByLevel(IItemInv item, int posibleLevel)
    {
        return true;
    }

    public bool TryAddItem(ParameterItem itemParam)
    {
        ParamItems.Add(itemParam);
        TransferItem(itemParam, true);
        return true;

    }

    public bool RemoveItem(ParameterItem itemParam)
    {
        var b = ParamItems.Remove(itemParam);
        TransferItem(itemParam, false);
        return b;
    }

    public virtual bool CanRemoveModulSlots(int slotsInt)
    {
        if (totalSlots() + slotsInt < MaxSlots)
        {
            return true;
        }
        return false;

    }      
    public virtual bool CanRemoveWeaponSlots(int slotsInt)
    {
        if (totalSlots() + slotsInt < MaxSlots)
        {
            return true;
        }
        return false;

    }
    public int GetItemIndex(IItemInv item)
    {
        switch (item.ItemType)
        {
            case ItemType.weapon:
                return GetIndex(Weapons,item);
            case ItemType.modul:
                return GetIndex(Moduls,item);
            case ItemType.spell:
                return GetIndex(Spells,item);
            default:
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                return 0;
        }
    }

    public bool IsSlotFree(int preferableIndex, ItemType type)
    {
        switch (type)
        {
            case ItemType.weapon:
                return IsFree(Weapons,preferableIndex);
            case ItemType.modul:
                return IsFree(Moduls, preferableIndex);
            case ItemType.spell:
                return IsFree(Spells, preferableIndex);
            default:
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                return false;
        }
    }

    private bool IsFree<T>(List<T> list,int index)
    {
        if (index < list.Count)
        {
            var s = list[index];
            return s == null;
        }

        return false;
    }  
    private int GetIndex<T>(List<T> list,IItemInv item) where T : IItemInv
    {
        for (int i = 0; i < list.Count; i++)
        {
            var p1 = list[i] as IItemInv;
            if (p1 != null && p1 == item)
            {
                return i;
            }
        }

        return -1;
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(Weapons);
        list.AddRange(Moduls);
        list.AddRange(Spells);
        list.AddRange(ParamItems);
        return list;
    }

    public int GetFreeSlotsCount()
    {
        var items = GetAllItems();
        var count = items.Count;
        var res = MaxSlots - count;
        return res;
    }

    public bool GetFreeSlot(out int index, ItemType type)
    {
        index = Moduls.Count;
        if (totalSlots() < MaxSlots)
        {
            return true;
        }
        return false;
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        index = Moduls.Count;
        if (totalSlots() < MaxSlots)
        {
            return true;
        }
        return false;
    }

    public virtual float ValuableItem(IItemInv item)
    {
        return 1f;
    }
    public bool GetFreeSpellSlot(out int index)
    {
        index = Spells.Count;
        if (totalSlots() < MaxSlots)
        {
            return true;
        }
        return false;
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        index = Weapons.Count;
        if (totalSlots() < MaxSlots)
        {
            return true;
        }
        return false;
    }

    public bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex)
    {
        Spells.Add(spellModul);
        //        spellModul.CurrentInventory = this;
        TransferItem(spellModul, true);
        return true;
    }

    public bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex)
    {
        Moduls.Add(simpleModul);
        //        simpleModul.CurrentInventory = this; 
        TransferItem(simpleModul, true);
        return true;
    }

    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        var b = Moduls.Remove(simpleModul);
        TransferItem(simpleModul, false);
        return b;
    }

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        Weapons.Add(weaponModul);
        TransferItem(weaponModul, true);
        return true;
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        var b = Weapons.Remove(weaponModul);
        TransferItem(weaponModul, false);
        return b;
    }

    public bool RemoveSpellModul(BaseSpellModulInv spell)
    {
        var b = Spells.Remove(spell);
        TransferItem(spell, false);
        return b;
    }

    private int totalSlots()
    {
        return Moduls.Count + Spells.Count + Weapons.Count + ParamItems.Count;
    }

    public void FixSlotsCount(bool isExprolerMode)
    {
        MaxSlots = PlayerSafe.GetInventorySlotsCount(isExprolerMode);
    }
}

