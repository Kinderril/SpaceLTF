using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class ShipModulsInventory
{
//    public BaseModulInv[] SimpleModuls => _simpleModuls;
    private BaseModulInv[] _simpleModuls;
     public int SimpleModulsCount => _startSlots + _additionalSlost;
    private int _startSlots;
    private int _additionalSlost;
    private IInventory _inventory;
    private int _id;

    [field: NonSerialized]
    public event Action<ShipModulsInventory> OnSlotsAdd;
    [field: NonSerialized]
    public event Action<ShipModulsInventory> OnSlotsRemove;

    public ShipModulsInventory(int simpleModulsCount, IInventory inventory)
    {
        _id = Utils.GetId();
        _inventory = inventory;
        _startSlots = simpleModulsCount;
        _simpleModuls = new BaseModulInv[100];
        for (int i = 0; i < SimpleModulsCount; i++)
        {
            _simpleModuls[i] = null;
        }
    }

    public List<BaseModulInv> GetNonNullActiveSlots()
    {
        List < BaseModulInv > slots = new List<BaseModulInv>();
        for (int i = 0; i < SimpleModulsCount && i < _simpleModuls.Length; i++)
        {
            var slot = _simpleModuls[i];
            if (slot != null)
            {
                slots.Add(slot);
            }
        }

        return slots;
    }

    public void AddSlots()
    {
        _additionalSlost = Mathf.Clamp(_additionalSlost + 1, 0, 90);
        OnSlotsAdd?.Invoke(this);
    }

    public bool CanRemoveSlots(int count)
    {
        int canRemove = 0;
        for (int i = 0; i < SimpleModulsCount; i++)
        {
            var slot = _simpleModuls[i];
            if (slot == null)
            {
                canRemove++;
                if (canRemove >= count)
                    return true;
            }
        }
        return false;
    }

    public bool RemoveSlot()
    {
        if (!CanRemoveSlots(1))
        {
            return false;
        }
        _additionalSlost = Mathf.Clamp(_additionalSlost - 1,0,90);
        OnSlotsRemove?.Invoke(this);
        return true;
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        for (int i = 0; i < SimpleModulsCount; i++)
        {
            var m = _simpleModuls[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex)
    {
        if (simpleModul == null)
        {
            return false;
        }
        if (SimpleModulsCount <= fieldIndex)
        {
            Debug.LogError("Too big index slot");
            return false;
        }
        var field = _simpleModuls[fieldIndex];
        if (field == null)
        {
            _simpleModuls[fieldIndex] = simpleModul;
            simpleModul.CurrentInventory = _inventory;
            _inventory.TransferItem(simpleModul, true);
            Debug.Log("Simple modul add to ship " + _id + "  " + simpleModul.Type.ToString());
            return true;
        }
        Debug.LogError("Slot not free");
        return false;
    }

    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        for (int i = 0; i < SimpleModulsCount; i++)
        {
            var m = _simpleModuls[i];
            if (m == simpleModul)
            {
                _simpleModuls[i] = null;
                _inventory.TransferItem(m, false);
                return true;
            }
        }
        return false;
    }
}

