using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;


public class InventoryUI : DragZone
{
//    private const int MAX_SLOTS = 16;

    private List<BaseModulInv> Moduls = new List<BaseModulInv>();
    private List<WeaponInv> Weapons = new List<WeaponInv>();
    private List<BaseSpellModulInv> Spells = new List<BaseSpellModulInv>();
    private List<ParameterItem> ParamItems = new List<ParameterItem>();
    
    public Transform Layout;
    private PlayerSafe _player;                                           
    private HashSet<DragableItemSlot> _allSLots = new HashSet<DragableItemSlot>();

    public bool ShallRefreshPos = true;
    public bool ShallRefreshPosWithZero = false;

//    public ScrollView Scroll;
//    private InventorySlots SlotsController;

    public void Init(PlayerInventory player, ConnectInventory connectedInventory,bool canDrop, IInventory tradeInventory = null)
    {
        _tradeInventory = tradeInventory;
        //        Scroll.contentViewport = transform.parent;
        _inventory = player;
        _player = player.Owner;
        Layout.ClearTransform();
//        _inventory = player;
        Weapons = player.Weapons;
        Moduls = player.Moduls;
        ParamItems = player.ParamItems;
        Spells = player.Spells;
        InitCurrentItems(canDrop);
        InitFreeSlots(canDrop);
//        InitMoney();
        base.Init(player, true, _allSLots, connectedInventory);
        RefreshPosition();

    }

    public void RefreshPosition()
    {
        if (!ShallRefreshPos)
        {
            return;
        }
        var rect = Layout.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        if (ShallRefreshPosWithZero)
        {
            Layout.gameObject.transform.localPosition = new Vector3(0, -1000, 0);
        }
        else
        {
            var localPos = Layout.gameObject.transform.localPosition;
            Layout.gameObject.transform.localPosition = new Vector3(localPos.x, -1000, localPos.z);

        }
//        Debug.LogError($"RefreshPosition:{ Layout.gameObject.transform.localPosition}");
    }
    


    private void InitCurrentItems(bool canDrop)
    {
        for (int i = 0; i < ParamItems.Count; i++)
        {
            var paramItem = ParamItems[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory,true);
            SetStartItem(itemSlot,paramItem,_tradeInventory);
            itemSlot.CanDrop = canDrop;
            _allSLots.Add(itemSlot);
        }  
        for (int i = 0; i < Weapons.Count; i++)
        {
            var weapon = Weapons[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory,true);
            SetStartItem(itemSlot,weapon, _tradeInventory);
            itemSlot.CanDrop = canDrop;
            _allSLots.Add(itemSlot);
        }
        for (int i = 0; i < Moduls.Count; i++)
        {
            var weapon = Moduls[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory, true);
            //            var item = DragableItem.Create(weapon, true);
            _allSLots.Add(itemSlot);
            SetStartItem(itemSlot,weapon, _tradeInventory);
            itemSlot.CanDrop = canDrop;
        }
        for (int i = 0; i < Spells.Count; i++)
        {
            var weapon = Spells[i];
            var itemSlot = AttachToLayout();
            itemSlot.Init(_inventory, true);
            //            var item = DragableItem.Create(weapon, true);
            SetStartItem(itemSlot,weapon, _tradeInventory);
            _allSLots.Add(itemSlot);
            itemSlot.CanDrop = canDrop;
        }
    }

    private DragableItemSlot AttachToLayout()
    {
        var slot = InventoryOperation.GetDragableItemSlot();
        slot.transform.SetParent(Layout, false);
        slot.transform.SetAsFirstSibling();
        return slot;
    }
    
    private void InitFreeSlots(bool canDrop)
    {
        var delta = _inventory.SlotsCount - _allSLots.Count;
        for (int i = 0; i < delta; i++)
        {
            var itemSlot = AttachToLayout();

            itemSlot.Init(_inventory, true);
            itemSlot.transform.SetAsLastSibling();
            itemSlot.CanDrop = canDrop;
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

