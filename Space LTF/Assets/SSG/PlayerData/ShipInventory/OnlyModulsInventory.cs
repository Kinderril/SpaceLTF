using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class OnlyModulsInventory : IInventory
{
    public ShipModulsInventory Moduls;
    private readonly PlayerSafe _player;
    public const int COUNT = 2;
    public PlayerSafe Owner => _player;
    public int SlotsCount => COUNT;

    [field: NonSerialized] public event ItemTransferedTo OnItemAdded;

    public OnlyModulsInventory(PlayerSafe _player)
    {
        this._player = _player;
        Moduls = new ShipModulsInventory(COUNT, this);
    }

    public bool GetFreeSlot(out int index, ItemType type)
    {
        index = -1;
        switch (type)
        {
            case ItemType.modul:
                return Moduls.GetFreeSimpleSlot(out index);
        }

        return false;
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        return Moduls.GetFreeSimpleSlot(out index);
    }

    public bool GetFreeSpellSlot(out int index)
    {
        index = -1;
        return false;
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        index = -1;
        return false;
    }

    public bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex)
    {
        return false;
    }

    public bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex)
    {
        return Moduls.TryAddSimpleModul(simpleModul, fieldIndex);
    }

    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        return Moduls.RemoveSimpleModul(simpleModul);
    }

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        return false;
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        return false;
    }

    public bool RemoveSpellModul(BaseSpellModulInv spell)
    {
        return false;
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(Moduls.GetNonNullActiveSlots());
        return list;
    }

    public void TransferItem(IItemInv item, bool val)
    {
        OnItemAdded?.Invoke(item, val);
    }

    public float ValuableItem(IItemInv item)
    {
        return 1f;
    }


    public bool IsShop()
    {
        return false;
    }

    public bool CanMoveToByLevel(IItemInv item, int posibleLevel)
    {
        return true;
    }

    public bool TryAddItem(ParameterItem itemParam)
    {
        return false;
    }

    public bool RemoveItem(ParameterItem itemParam)
    {
        return false;
    }

    public bool CanRemoveModulSlots(int slotsInt)
    {
        return false;
    }

    public bool CanRemoveWeaponSlots(int slotsInt)
    {
        return false;
    }

    public bool IsSlotFree(int preferableIndex, ItemType modul)
    {
        if (modul != ItemType.modul)
            return false;
        return Moduls.IsSlotFree(preferableIndex);
    }

    public int GetItemIndex(IItemInv item)
    {
        switch (item.ItemType)
        {
            case ItemType.modul:
                return Moduls.GetItemIndex(item);
            default:
                return 0;
        }
    }
}