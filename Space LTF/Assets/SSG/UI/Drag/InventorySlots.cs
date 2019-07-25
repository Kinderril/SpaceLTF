using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InventorySlots
{
    private IInventory _inventory;
    private List<DragableItemSlot> _slots;

    public InventorySlots(IInventory inventory,List<DragableItemSlot> slots)
    {
        _slots = slots;
        _inventory = inventory;
        _inventory.OnItemAdded += OnItem;
    }

    private void OnItem(IItemInv item, bool val)
    {
        if (val)
        {
            foreach (var itemSlot in _slots)
            {
                if (itemSlot.CanPutHere(item))
                {
                    return;
                }   
            }
            Debug.LogError("can't put item to dragable slot");
        }
        else
        {
            foreach (var slot in _slots)
            {
                if (slot.CurrentItem != null)
                {
                    if (slot.CurrentItem.ContainerItem == item)
                    {
                        slot.SetFree();
                        break;
                    }
                }
            }
        }
    }

    public void Dispose()
    {
        _inventory.OnItemAdded -= OnItem;
    }
}

