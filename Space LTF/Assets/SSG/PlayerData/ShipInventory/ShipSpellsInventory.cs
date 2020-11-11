using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ShipSpellsInventory
{
    private BaseSpellModulInv[] _spells = new BaseSpellModulInv[0];
    public int SpellsCount => _startSlots;// + _additionalSlost;
    private int _startSlots;
//    private int _additionalSlost;
    private IInventory _inventory;
    private int _id;

//
//    [field: NonSerialized]
//    public event Action<ShipWeaponsInventory, DragItemType> OnSlotsAdd;
//    [field: NonSerialized]
//    public event Action<ShipWeaponsInventory, DragItemType> OnSlotsRemove;

    public ShipSpellsInventory(int count, IInventory inventory)
    {
        _id = Utils.GetId();
        _inventory = inventory;
        _startSlots = count;
        _spells = new BaseSpellModulInv[100];
        for (int i = 0; i < _startSlots; i++)
        {
            _spells[i] = null;
        }
    }

    public BaseSpellModulInv[] GetAsCopyArray()
    {
        return _spells.ToArray();
    }
    public int GetItemIndex(IItemInv item)
    {
        for (int i = 0; i < SpellsCount; i++)
        {
            var m = _spells[i];
            if (m != null && m == item)
            {
                return i;
            }
        }
        return -1;
    }
    public List<BaseSpellModulInv> GetNonNullActiveSlots()
    {
        if (_spells == null)
        {
            return new List<BaseSpellModulInv>();
        }
        List<BaseSpellModulInv> slots = new List<BaseSpellModulInv>();
        for (int i = 0; i < SpellsCount && i < _spells.Length; i++)
        {
            var slot = _spells[i];
            if (slot != null)
            {
                slots.Add(slot);
            }
        }

        return slots;
    }

    public bool TryAddSpellModul(BaseSpellModulInv spell, int fieldIndex)
    {
        if (spell == null)
        {
            return false;
        }
        if (_spells.Length <= fieldIndex)
        {
            Debug.LogError("Too big index _spells");
            return false;
        }
        var field = _spells[fieldIndex];
        if (field == null)
        {
            _spells[fieldIndex] = spell;
            spell.CurrentInventory = _inventory;
            _inventory.TransferItem(spell, true);
            return true;
        }
        Debug.LogError("Slot not free");
        return false;
    }
    public bool IsSlotFree(int preferableIndex)
    {
        if (preferableIndex < SpellsCount && preferableIndex >=0 )
        {
            var m = _spells[preferableIndex];
            return (m == null);
        }
        return false;
    }
    public bool GetFreeSimpleSlot(out int index)
    {
        for (int i = 0; i < SpellsCount; i++)
        {
            var m = _spells[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool RemoveSpellModul(BaseSpellModulInv spellModul)
    {
        for (int i = 0; i < _spells.Length; i++)
        {
            var m = _spells[i];
            if (m == spellModul)
            {
                _spells[i] = null;
                _inventory.TransferItem(m, false);
                return true;
            }
        }
        return false;
    }

//    public void AddSlots(DragItemType type)
//    {
//        _additionalSlost = Mathf.Clamp(_additionalSlost + 1, 0, 90);
//        OnSlotsAdd?.Invoke(this, type);
//    }

//    public bool CanRemoveSlots(int count)
//    {
//        int canRemove = 0;
//        for (int i = 0; i < WeaponsCount; i++)
//        {
//            var slot = _weapons[i];
//            if (slot == null)
//            {
//                canRemove++;
//                if (canRemove >= count)
//                    return true;
//            }
//        }
//        return false;
//    }

//    public bool RemoveSlot(DragItemType type)
//    {
//        if (!CanRemoveSlots(1))
//        {
//            return false;
//        }
//        _additionalSlost = Mathf.Clamp(_additionalSlost - 1, 0, 90);
//        OnSlotsRemove?.Invoke(this, type);
//        return true;
//    }

    public IItemInv Get(int i)
    {
        if (_spells == null)
        {
            return null;
        }
        return _spells[i];
    }
}
