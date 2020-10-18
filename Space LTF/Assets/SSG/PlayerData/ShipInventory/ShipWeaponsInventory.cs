using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ShipWeaponsInventory
{
    private WeaponInv[] _weapons;
    public int WeaponsCount => _startSlots + _additionalSlost;
    private int _startSlots;
    private int _additionalSlost;
    private IInventory _inventory;
    private int _id;


    [field: NonSerialized]
    public event Action<ShipWeaponsInventory, DragItemType> OnSlotsAdd;
    [field: NonSerialized]
    public event Action<ShipWeaponsInventory, DragItemType> OnSlotsRemove;

    public ShipWeaponsInventory(int weapons, IInventory inventory)
    {
        _id = Utils.GetId();
        _inventory = inventory;
        _startSlots = weapons;
        _weapons = new WeaponInv[100];
        for (int i = 0; i < WeaponsCount; i++)
        {
            _weapons[i] = null;
        }
    }
    public List<WeaponInv> GetNonNullActiveSlots()
    {
        List<WeaponInv> slots = new List<WeaponInv>();
        for (int i = 0; i < WeaponsCount && i < _weapons.Length; i++)
        {
            var slot = _weapons[i];
            if (slot != null)
            {
                slots.Add(slot);
            }
        }

        return slots;
    }

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        if (weaponModul == null)
        {
            return false;
        }
        if (_weapons.Length <= fieldIndex)
        {
            Debug.LogError("Too big index weapon");
            return false;
        }
        var field = _weapons[fieldIndex];
        if (field == null)
        {
            _weapons[fieldIndex] = weaponModul;
            weaponModul.CurrentInventory = _inventory;
            _inventory.TransferItem(weaponModul, true);
            return true;
        }
        Debug.LogError("Slot not free");
        return false;
    }
    public bool IsSlotFree(int preferableIndex)
    {
        if (preferableIndex < WeaponsCount)
        {
            var m = _weapons[preferableIndex];
            return (m == null);
        }
        return false;
    }
    public bool GetFreeSimpleSlot(out int index)
    {
        for (int i = 0; i < WeaponsCount; i++)
        {
            var m = _weapons[i];
            if (m == null)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            var m = _weapons[i];
            if (m == weaponModul)
            {
                _weapons[i] = null;
                _inventory.TransferItem(m, false);
                return true;
            }
        }
        return false;
    }

    public void AddSlots(DragItemType type)
    {
        _additionalSlost = Mathf.Clamp(_additionalSlost + 1, 0, 90);
        OnSlotsAdd?.Invoke(this, type);
    }

    public bool CanRemoveSlots(int count)
    {
        int canRemove = 0;
        for (int i = 0; i < WeaponsCount; i++)
        {
            var slot = _weapons[i];
            if (slot == null)
            {
                canRemove++;
                if (canRemove >= count)
                    return true;
            }
        }
        return false;
    }
    public int GetItemIndex(IItemInv item)
    {
        for (int i = 0; i < WeaponsCount; i++)
        {
            var m = _weapons[i];
            if (m != null && m == item)
            {
                return i;
            }
        }
        return -1;
    }

    public bool RemoveSlot(DragItemType type)
    {
        if (!CanRemoveSlots(1))
        {
            return false;
        }
        _additionalSlost = Mathf.Clamp(_additionalSlost - 1, 0, 90);
        OnSlotsRemove?.Invoke(this, type);
        return true;
    }

}
