using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShipInventoryUI : DragZone
{
    public Image IconType;
    public TextMeshProUGUI ConfigType;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI LevelField;


    public Transform WeaponsLayout;
    public Transform ModulsLayout;
    private IPilotParameters _pilot;
    private ShipInventory _shipInventory;

    public void Init(ShipInventory shipInventory, IPilotParameters pilot, bool usable, ConnectInventory connectedInventory)
    {
        _pilot = pilot;
        _pilot.OnLevelUp += OnLevelUp;
        _shipInventory = shipInventory;
//        _weaponsSlots.Clear();
//        _modulsSlots.Clear();
//        _spellsSlots.Clear();
        WeaponsLayout.ClearTransform();
        ModulsLayout.ClearTransform();
//        SpellsLayout.ClearTransform();
        WeaponsLayout.gameObject.SetActive(_shipInventory.WeaponModulsCount > 0);
        ModulsLayout.gameObject.SetActive(_shipInventory.SimpleModulsCount > 0);
//        SpellsLayout.gameObject.SetActive(_shipInventory.SpellModulsCount > 0);
        var allSlots = InitFreeSlots();
        base.Init(shipInventory, usable, allSlots, connectedInventory);
        InitCurrentItems();
        NameField.text = shipInventory.Name;
        ConfigType.text = Namings.ShipConfig(shipInventory.ShipConfig);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(shipInventory.ShipType);
        //        ConfigType.sprite = DataBaseController.Instance.DataStructPrefabs.Get(shipInventory.ShipType);
        //        Slots = new InventorySlots(shipInventory, allSlots);
        SetLevelUpField();

    }

    private void OnLevelUp(IPilotParameters obj)
    {
        SetLevelUpField();
    }

    private void SetLevelUpField()
    {
        LevelField.text = _pilot.CurLevel.ToString();
    }

    private List<DragableItemSlot> InitFreeSlots()
    {
        List < DragableItemSlot > allslots = new List<DragableItemSlot>();
        for (int i = 0; i < _shipInventory.WeaponModulsCount; i++)
        {
            var weaponSlot = InventoryOperation.GetDragableItemSlot();
            allslots.Add(weaponSlot);
            weaponSlot.Init(_shipInventory, true);
            weaponSlot.transform.SetParent(WeaponsLayout, false);
            weaponSlot.DragItemType = DragItemType.weapon;
//            _allSlots.Add(weaponSlot);
        }
        for (int i = 0; i < _shipInventory.SimpleModulsCount; i++)
        {
            var itemSlot = InventoryOperation.GetDragableItemSlot();
            allslots.Add(itemSlot);
            itemSlot.Init(_shipInventory,true);
            itemSlot.transform.SetParent(ModulsLayout, false);
            itemSlot.DragItemType = DragItemType.modul;
//            _allSlots.Add(itemSlot);
        }

        return allslots;
    }

    private void InitCurrentItems()
    {
        for (int i = 0; i < _shipInventory.WeaponsModuls.Length; i++)
        {
            var weapon = _shipInventory.WeaponsModuls[i];
            var slot = GetFreeSlot(ItemType.weapon);
//            slot.Init(_shipInventory, _usable);
            SetStartItem(slot,weapon);
        }
//        Debug.Log("Moduls of ship:" + _shipInventory.Name + "   " + _shipInventory.Moduls.SimpleModuls.Count(x =>x!=null));
        for (int i = 0; i < _shipInventory.Moduls.SimpleModuls.Length; i++)
        {
            var modul = _shipInventory.Moduls.SimpleModuls[i];
//            if (modul != null)
//            {
//                Debug.Log("Modul of ship:" + _shipInventory.Name + "   " + modul.Type.ToString());
//            }
            var slot = GetFreeSlot(ItemType.modul);
//            slot.Init(_shipInventory, _usable);
            SetStartItem(slot,modul);
        }
//        for (int i = 0; i < _shipInventory.SpellsModuls.Length; i++)
//        {
//            var spell = _shipInventory.SpellsModuls[i];
//            var slot = GetFreeSlot(ItemType.spell);
//            slot.Init(_shipInventory, _usable);
//            SetStartItem(slot,spell);
//        }
    }

    public override void Dispose()
    {
        _pilot.OnLevelUp -= OnLevelUp;
        base.Dispose();
    }
}

