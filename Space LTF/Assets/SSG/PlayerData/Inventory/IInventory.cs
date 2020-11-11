﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

public delegate void ItemTransferedTo(IItemInv item,bool val);

//[Serializable]
public interface IInventory
{
    bool GetFreeSlot(out int index,ItemType type);
    bool GetFreeSimpleSlot(out int index);

    bool GetFreeSpellSlot(out int index);

    bool GetFreeWeaponSlot(out int index);

    bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex);

    bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex);

    bool RemoveSimpleModul(BaseModulInv simpleModul);

    bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex);

    bool RemoveWeaponModul(WeaponInv weaponModul);

    bool RemoveSpellModul(BaseSpellModulInv spell);

    List<IItemInv> GetAllItems();

    event ItemTransferedTo OnItemAdded;   //MUST BE NOT SERIALIZED

    void TransferItem(IItemInv item, bool val);
    float ValuableItem(IItemInv item);

    PlayerSafe Owner { get; }
    int SlotsCount { get; }

    bool IsShop();
    bool CanMoveToByLevel(IItemInv item,int posibleLevel);
    bool TryAddItem(ParameterItem itemParam);
    bool RemoveItem(ParameterItem itemParam);
    bool CanRemoveModulSlots(int slotsInt);
    bool CanRemoveWeaponSlots(int slotsInt);
    bool IsSlotFree(int preferableIndex, ItemType weapon);
    int GetItemIndex(IItemInv item);
}

