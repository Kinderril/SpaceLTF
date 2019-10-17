using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DragableItemSlot : MonoBehaviour
{
    public DragItemType DragItemType;
    public DragableItem CurrentItem;
    private bool _usage = false;
    public IInventory _inventory { get; private set; }

    public int id;
    private IInventory connectedInventory;
//    public event Action<DragableItemSlot, DragableItem,bool> OnItemImplemented;

    public void Init(IInventory inventory,bool usage)
    {
#if UNITY_EDITOR
        if (inventory == null)
        {
            Debug.LogError("Try to ini nll inventory");
        }
#endif
        _inventory = inventory;
        _usage = usage;
        id = Utils.GetId();
    }
    
    public bool CanPutHere(DragableItem element)
    {
#if UNITY_EDITOR
        if (_inventory == null)
        {
            Debug.LogError("Init slot _inventory == null");
        }
#endif
        if (!_usage)
        {
            return false;
        }
        if (DragItemType == DragItemType.free)
        {
            return true;
        }
        return DragItemType == element.DragItemType;
    }

    public bool CanPutHere(IItemInv element)
    {
        if (element == null)
        {
            Debug.LogError("can't pub ZERO item to slot");
            return false;
        }
#if UNITY_EDITOR
        if (_inventory == null)
        {
            Debug.LogError("Init slot _inventory == null");
        }
#endif
        if (!_usage)
        {
            return false;
        }

        if (CurrentItem != null)
        {
            return false;
        }
        if (DragItemType == DragItemType.free)
        {
            return true;
        }
        switch (element.ItemType)
        {
            case ItemType.weapon:
                return DragItemType == DragItemType.weapon;
            case ItemType.modul:
                return DragItemType == DragItemType.modul;
            case ItemType.spell:
                return DragItemType == DragItemType.spell;
        }
        return false;
    }

    public void ImplimentItem(DragableItem item,Action<bool> Callback)
    {
#if UNITY_EDITOR
        if (_inventory == null)
        {
            Debug.LogError("Init slot _inventory == null");
        }
#endif
        if (CurrentItem == null)
        {
            if (item.ContainerItem.CurrentInventory != _inventory)
            {
#if UNITY_EDITOR
                if (_inventory == null)
                {
                    Debug.LogError("transfered item fail");
                }
#endif
                InventoryOperation.TryItemTransfered(_inventory, item.ContainerItem, Callback);
                return;

            }
        }
        Callback(false);
    }

    public void StartItemSet(IItemInv item)
    {
#if UNITY_EDITOR
        if (item != null && item.CurrentInventory == null)
        {
            var modul = item as BaseModulInv;
            string modulNUll = "";
            if (modul != null)
            {
                modulNUll = $"UnityEditorID:{modul.UnityEditorID}";
            }
            Debug.LogError("Item with null inventory " + item.WideInfo() + "  2:" + item + $"   3:{item.ItemType.ToString()}   4:{modulNUll}   " );
        }
        if (_inventory == null)
        {
            if (item != null)
            {
                Debug.LogError("Init slot _inventory == null " + item.GetInfo() + "   " + item + "   " + item.ItemType.ToString());
            }
            else
            {
                Debug.LogError("Init slot _inventory == null ");
            }
        }
#endif
        if (item == null)
        {
//            Debug.LogError("Implement zero item");
            return;
        }
        if (item.CurrentInventory != _inventory)
        {
            Debug.LogError("setted item have wrong inventory:" + (item.CurrentInventory != null) + "  _inventory:" + _inventory);
        }
        var element = DragableItem.Create(item, _usage);
        CurrentItem = element;
        
        element.transform.SetParent(transform);
        element.transform.localPosition = Vector3.zero;
        CurrentItem.Slot = this;
        VisualUpdate();
    }

    public virtual void VisualUpdate()
    {
    }

    public void SetFree()
    {
        if (CurrentItem == null)
        {
            Debug.LogError("Try free. free slot");
        }
        else
        {
            Debug.Log("Dragabale element Destroy:" + CurrentItem.id);
            GameObject.Destroy(CurrentItem.gameObject);
        }
        CurrentItem = null;
    }

//
//    void OnDestroy()
//    {
//        OnItemImplemented = null;
//    }

    public void Dispose()
    {
//        OnItemImplemented = null;
    }

    public void SetFastOthInventory(IInventory connectedInventory)
    {
        this.connectedInventory = connectedInventory;

    }

    public void DoubleClick(DragableItem dragableItem)
    {
        if (_usage)
        {
            if (connectedInventory != null && dragableItem.ContainerItem != null)
            {
                InventoryOperation.TryItemTransfered(connectedInventory, dragableItem.ContainerItem, b =>
                {
                    
                });
            }
        }
    }
}

