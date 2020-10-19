using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class BaseShipSpellContainer : DragZone
{
    public Transform LayoutSpell;
    public Transform LayoutModules;

    private List<DragableItemSlot> slots = new List<DragableItemSlot>();
    private HashSet<DragableItemSlot> modulsSlots = new HashSet<DragableItemSlot>();
    public List<DragableItemSlot> InitSpell(int index,Transform layout,ShipInventory shipInventory,[CanBeNull]OnlyModulsInventory modulsInventory)
    {
        slots.Clear();
        transform.SetParent(layout, false);

        var spellSlot = InventoryOperation.GetDragableItemSlot();
        spellSlot.Init(shipInventory, true, index);
        spellSlot.transform.SetParent(LayoutSpell, false);
        spellSlot.DragItemType = DragItemType.spell;
        slots.Add(spellSlot);

        modulsSlots.Clear();

        if (modulsInventory != null)
        {
            modulsSlots = new HashSet<DragableItemSlot>();
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

    public override void Dispose()
    {
        foreach (var dragableItemSlot in slots)
        {
            dragableItemSlot.Dispose();
        }

        foreach (var dragableItemSlot in modulsSlots)
        {
            dragableItemSlot.Dispose();
        }
        modulsSlots.Clear();
        slots.Clear();
        base.Dispose();
    }

    private void InitCurrentItems(OnlyModulsInventory modulsInventory)
    {
        for (int i = 0; i < modulsInventory.Moduls.SimpleModulsCount; i++)
        {
            var modul = modulsInventory.Moduls.Get(i);
            if (modul != null)
            {

                var slot = GetFreeSlot(i,ItemType.modul);
                if (slot != null)
                {
                    slot.Init(modulsInventory, _usable, i);
                    SetStartItem(slot, modul, _tradeInventory);
                }
                else
                {
                    Debug.LogError($"Can't find slot _slots:{_slots.Count} ");
                }
            }
        }
    }
}
