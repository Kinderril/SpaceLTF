using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class PlayerInventory : IInventory
{

    public const int MAX_SLOTS = 50;
    public List<BaseModulInv> Moduls = new List<BaseModulInv>();
    public List<WeaponInv> Weapons = new List<WeaponInv>();
    public List<BaseSpellModulInv> Spells = new List<BaseSpellModulInv>();


    [field: NonSerialized]
    public event ItemTransferedTo OnItemAdded;
    private Player _player;

    public PlayerInventory(Player player)
    {
        _player = player;
        Moduls.Clear();
        Weapons.Clear();
        Spells.Clear();
    }

    public void TransferItem(IItemInv item, bool val)
    {
        Debug.Log($"TransferItem {item.WideInfo()}  {val}");
        if (val)
            item.CurrentInventory = this;
        if (OnItemAdded != null)
        {
            OnItemAdded(item, val);
        }
    }
    [CanBeNull]
    public Player Owner { get { return _player; } }
    public virtual bool IsShop()
    {
        return false;
    }

    public List<IItemInv> GetAllItems()
    {
        var list = new List<IItemInv>();
        list.AddRange(Weapons);
        list.AddRange(Moduls);
        list.AddRange(Spells);
        return list;
    }
    public bool GetFreeSimpleSlot(out int index)
    {
        index = Moduls.Count;
        if (totalSlots() < MAX_SLOTS)
        {
            return true;
        }
        return false;
    }

    public bool GetFreeSpellSlot(out int index)
    {
        index = Spells.Count;
        if (totalSlots() < MAX_SLOTS)
        {
            return true;
        }
        return false;
    }

    public bool GetFreeWeaponSlot(out int index)
    {
        index = Weapons.Count;
        if (totalSlots() < MAX_SLOTS)
        {
            return true;
        }
        return false;
    }

    public bool TryAddSpellModul(BaseSpellModulInv spellModul, int fieldIndex)
    {
        Spells.Add(spellModul);
        //        spellModul.CurrentInventory = this;
        TransferItem(spellModul, true);
        return true;
    }

    public bool TryAddSimpleModul(BaseModulInv simpleModul, int fieldIndex)
    {
        Moduls.Add(simpleModul);
        //        simpleModul.CurrentInventory = this; 
        TransferItem(simpleModul, true);
        return true;
    }

    public bool RemoveSimpleModul(BaseModulInv simpleModul)
    {
        var b = Moduls.Remove(simpleModul);
        TransferItem(simpleModul, false);
        return b;
    }

    public bool TryAddWeaponModul(WeaponInv weaponModul, int fieldIndex)
    {
        Weapons.Add(weaponModul);
        //        weaponModul.CurrentInventory = this;
        TransferItem(weaponModul, true);
        return true;
    }

    public bool RemoveWeaponModul(WeaponInv weaponModul)
    {
        var b = Weapons.Remove(weaponModul);
        TransferItem(weaponModul, false);
        return b;
    }

    public bool RemoveSpellModul(BaseSpellModulInv spell)
    {
        var b = Spells.Remove(spell);
        TransferItem(spell, false);
        return b;
    }

    private int totalSlots()
    {
        return Moduls.Count + Spells.Count + Weapons.Count;
    }

}

