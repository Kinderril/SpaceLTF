using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class ShipModulsInventory
{
    public BaseModulInv[] SimpleModuls;
    private int simpleModulsCount;
    private IInventory _inventory;
    private int _id;

    public ShipModulsInventory(int simpleModulsCount, IInventory inventory)
    {
        _id = Utils.GetId();
        _inventory = inventory;
        this.simpleModulsCount = simpleModulsCount;
        SimpleModuls = new BaseModulInv[simpleModulsCount];
    }

    public bool GetFreeSimpleSlot(out int index)
    {
        for (int i = 0; i < SimpleModuls.Length; i++)
        {
            var m = SimpleModuls[i];
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
        if (SimpleModuls.Length <= fieldIndex)
        {
            Debug.LogError("Too big index slot");
            return false;
        }
        var field = SimpleModuls[fieldIndex];
        if (field == null)
        {
            SimpleModuls[fieldIndex] = simpleModul;
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
        for (int i = 0; i < SimpleModuls.Length; i++)
        {
            var m = SimpleModuls[i];
            if (m == simpleModul)
            {
                SimpleModuls[i] = null;
                _inventory.TransferItem(m, false);
                return true;
            }
        }
        return false;
    }
}

