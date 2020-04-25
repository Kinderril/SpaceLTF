using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConnectInventory
{
    public IInventory ConnectedInventory;

    public ConnectInventory(IInventory connectedInventory)
    {
        ConnectedInventory = connectedInventory;
    }
}

public class DragZone : MonoBehaviour
{
    protected IInventory _inventory;
    protected List<DragableItemSlot> _slots = new List<DragableItemSlot>();
    protected bool _usable;
    private ConnectInventory _connectedInventory;
    private bool _inited = false;
    private bool _enabled = false;
    private bool _subcribed = false;
    private int _zoneId;
//    public bool CanDrop = true;


    protected void Init(IInventory inventory, bool usable , List<DragableItemSlot> slots, ConnectInventory connectedInventory)
    {
        _zoneId = Utils.GetId();
        _connectedInventory = connectedInventory;
        _slots = slots;
        _usable = usable;
        _inventory = inventory;
        Enable();
        _inited = true;
        Debug.Log("Init drag zone " + _zoneId);
    }

    protected void SetStartItem(DragableItemSlot slot, IItemInv item)
    {
#if UNITY_EDITOR
        if (slot == null)
        {
            Debug.LogError("WTF????");
        }
#endif
        slot.StartItemSet(item);
        if (_connectedInventory != null)
            slot.SetFastOthInventory(_connectedInventory.ConnectedInventory);
    }
    
    public virtual void Dispose()
    {
//        Debug.Log("DragZone dispose:" + _zoneId + "  " + gameObject.name);
        _slots.Clear();
        Disable();
    }

    private void CheckCurrentItems()
    {
        foreach (var item in _inventory.GetAllItems())
        {
            var slot = _slots.FirstOrDefault(x => x.CurrentItem != null && x.CurrentItem.ContainerItem == item);
            if (slot == null)
            {
                int slotId;
                if (FindSlotForItem(item, _inventory, out slotId))
                {
                    var slotForItem = _slots.FirstOrDefault(x => x.id == slotId);
                    if (slotForItem != null)
                    {
                        SetStartItem(slotForItem, item);
                    }
                    else
                    {
                        Debug.LogError("Can't find slot object");
                    }
                }
                else
                {

                    Debug.LogError("Can't find slot index");
                }
            }
        }
    }

    protected static bool FindSlotForItem(IItemInv item,IInventory inventory, out int slot)
    {
//        int slot;
        switch (item.ItemType)
        {
            case ItemType.weapon:
                if (inventory.GetFreeWeaponSlot(out slot))
                {
                    return true;
                }
                break;
            case ItemType.modul:
                if (inventory.GetFreeSimpleSlot(out slot))
                {
                    return true;
                }
                break;
            case ItemType.spell:
                if (inventory.GetFreeSpellSlot(out slot))
                {
                    return true;
                }
                break;
        }

        slot = -1;
        return false;
    }

    private void OnItem(IItemInv item, bool val)
    {
        Debug.Log("OnItem drag zone:" + _zoneId + "  " + (val?"add":"remove") + "  " + gameObject.name);
        if (val)
        {
#if UNITY_EDITOR
            if (!_inited)
            {
                Debug.LogError("not inited zone");
            }
            foreach (var dragableItemSlot in _slots)
            {
                if (dragableItemSlot == null)
                {
                    Debug.LogError("fin zero slot!!");
                }
            }
#endif
            foreach (var itemSlot in _slots)
            {
                if (itemSlot.CanPutHere(item))
                {
                    SetStartItem(itemSlot,item);
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

    public void DropElement(DragableItem item)
    {
        InventoryOperation.TryItemTransfered(_inventory, item.ContainerItem, b =>
        {
            if (b)
            {
                GameObject.Destroy(item.gameObject);

            }
            else
            {
                item.ReturnToLastParent();
            }
        });
    }

    protected virtual DragableItemSlot GetFreeSlot(ItemType type)
    {
        var freeSlot = _slots.FirstOrDefault(
            x => x.CurrentItem == null && Slot(x.DragItemType, type));
        return freeSlot;


    }

    private bool Slot(DragItemType v , ItemType a)
    {
        switch (v)
        {
            case DragItemType.free:
                return true;
            case DragItemType.weapon:
                return a == ItemType.weapon;
            case DragItemType.modul:
                return a == ItemType.modul;
            case DragItemType.spell:
                return a == ItemType.spell;
        }
        return false;
    }

    protected virtual DragableItemSlot GetItemSlot(IItemInv item)
    {
        return
            _slots.FirstOrDefault(
                x => x.CurrentItem.ContainerItem == item);
    }

    public void Disable()
    {
        if (_enabled)
        {
//            Debug.Log("drag zone Disable:" + _zoneId + "  " + gameObject.name);
            _enabled = false;
            _subcribed = false;
            _inventory.OnItemAdded -= OnItem;
        }
    }
    public void Enable()
    {
        if (!_enabled)
        {
//            CheckCurrentItems();
            Debug.Log("drag zone Enable:" + _zoneId + "  " + gameObject.name);
            _enabled = true;
            _subcribed = true;
            _inventory.OnItemAdded += OnItem;
        }
    }


#if UNITY_EDITOR
    void OnDisable()
    {
        if (_subcribed)
        {
            Debug.LogError("drag zone OnDisable error:" + _zoneId + "  " + gameObject.name);
            TryDebugParent();
        }
    }

    void OnEnable()
    {
        if (_subcribed)
        {
//            Debug.LogError("drag zone OnEnable error:" + _zoneId + "  " + gameObject.name);
//            TryDebugParent();
        }

    }

    void OnDestroy()
    {
        if (_subcribed)
        {
            Debug.LogError("drag zone OnDestroy error:" + _zoneId + "  " + gameObject.name);
            TryDebugParent();
        }
    }
#endif

    private void TryDebugParent()
    {
        string ss = "";
        if (gameObject.transform.parent != null)
        {
            ss += "  " + gameObject.transform.parent.name;
            if (gameObject.transform.parent.parent != null)
            {
                ss += "  "+gameObject.transform.parent.parent.name;
                if (gameObject.transform.parent.parent.parent != null)
                {
                    ss += "  " + gameObject.transform.parent.parent.parent.parent.name;
                    if (gameObject.transform.parent.parent.parent.parent != null)
                    {
                        ss += "  " + gameObject.transform.parent.parent.parent.parent.name;
                    }
                }
            }
        }
        Debug.LogError("Parents info:" + ss);
    }
}

