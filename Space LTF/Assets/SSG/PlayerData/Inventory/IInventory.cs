using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

public delegate void ItemTransferedTo(IItemInv item,bool val);

public interface IInventory
{
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

    event ItemTransferedTo OnItemAdded;

    void TransferItem(IItemInv item, bool val);
    float ValuableItem(IItemInv item);

    Player Owner { get; }
    int SlotsCount { get; }

    bool IsShop();
    bool CanMoveToByLevel(IItemInv item);
}

