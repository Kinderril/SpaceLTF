using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;


public class InventoryUI : DragZone
{
//    private const int MAX_SLOTS = 16;

    private List<BaseModulInv> Moduls = new List<BaseModulInv>();
    private List<WeaponInv> Weapons = new List<WeaponInv>();
    private List<BaseSpellModulInv> Spells = new List<BaseSpellModulInv>();
    
    public Transform Layout;
    private Player _player;                                           
    private List<DragableItemSlot> _allSLots = new List<DragableItemSlot>();
//    public ScrollView Scroll;
//    private InventorySlots SlotsController;

    public void Init(PlayerInventory player, ConnectInventory connectedInventory)
    {
//        Scroll.contentViewport = transform.parent;
        _inventory = player;
        _player = player.Owner;
        Layout.ClearTransform();
//        _inventory = player;
        Weapons = player.Weapons;
        Moduls = player.Moduls;
        Spells = player.Spells;
        InitCurrentItems();
        InitFreeSlots();
//        InitMoney();
        base.Init(player, true, _allSLots, connectedInventory);
//        Enable();
        //        SlotsController = new InventorySlots(player,_allSLots);

    }

//    private void InitMoney()
//    {
//        if (_player != null)
//        {
//            MoneySlotUI d = InventoryOperation.GetMoneySlot();
//            d.transform.SetParent(Layout);
//            d.Init(_player.MoneyData.MoneyCount);
//        }
//    }

    private void InitCurrentItems()
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            var weapon = Weapons[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory,true);
            SetStartItem(itemSlot,weapon);
            _allSLots.Add(itemSlot);
        }
        for (int i = 0; i < Moduls.Count; i++)
        {
            var weapon = Moduls[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory, true);
            //            var item = DragableItem.Create(weapon, true);
            _allSLots.Add(itemSlot);
            SetStartItem(itemSlot,weapon);
        }
        for (int i = 0; i < Spells.Count; i++)
        {
            var weapon = Spells[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory, true);
            //            var item = DragableItem.Create(weapon, true);
            SetStartItem(itemSlot,weapon);
            _allSLots.Add(itemSlot);
        }
    }

    private DragableItemSlot AttachToLayout()
    {
        var slot = InventoryOperation.GetDragableItemSlot();
        slot.transform.SetParent(Layout, false);
        slot.transform.SetAsFirstSibling();
        return slot;
    }
    
    private void InitFreeSlots()
    {
        var delta = _inventory.SlotsCount - _allSLots.Count;
        for (int i = 0; i < delta; i++)
        {
            var itemSlot = AttachToLayout();

            itemSlot.Init(_inventory, true);
            itemSlot.transform.SetAsLastSibling();
            //            itemSlot.OnItemImplemented += OnItemImplemented;
            _allSLots.Add(itemSlot);
        }
    }

    public override void Dispose()
    {
        foreach (var dragableItemSlot in _allSLots)
        {
            GameObject.Destroy(dragableItemSlot.gameObject);
        }
        _allSLots.Clear();
        base.Dispose();
    }

}

