using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class BaseShipSpellContainer : DragZone
{
    public Transform LayoutSpell;
    public Transform LayoutModules;

    public List<DragableItemSlot> InitSpell(int index,Transform layout,ShipInventory shipInventory,[CanBeNull]OnlyModulsInventory modulsInventory)
    {
        transform.SetParent(layout, false);
        List<DragableItemSlot> slots = new List<DragableItemSlot>();

        var spellSlot = InventoryOperation.GetDragableItemSlot();
        spellSlot.Init(shipInventory, true, index);
        spellSlot.transform.SetParent(LayoutSpell, false);
        spellSlot.DragItemType = DragItemType.spell;
        slots.Add(spellSlot);

        if (modulsInventory != null)
        {
            var modulsSlots = new HashSet<DragableItemSlot>();
            for (int i = 0; i < OnlyModulsInventory.COUNT; i++)
            {
                var modulSlot = InventoryOperation.GetDragableItemSlot();
                modulSlot.Init(modulsInventory, true,i);
                modulSlot.transform.SetParent(LayoutModules, false);
                modulSlot.DragItemType = DragItemType.modul;
                modulsSlots.Add(modulSlot);
            }
            Init(modulsInventory,true,modulsSlots,null);
            InitCurrentItems(modulsInventory);
        }

        return slots;
    }
    private void InitCurrentItems(OnlyModulsInventory modulsInventory)
    {
        for (int i = 0; i < modulsInventory.Moduls.SimpleModulsCount; i++)
        {
            var modul = modulsInventory.Moduls.Get(i);
            if (modul != null)
            {

                var slot = GetFreeSlot(i,ItemType.spell);
                slot.Init(modulsInventory, _usable, i);
                SetStartItem(slot, modul, _tradeInventory);
            }
        }
    }
}
