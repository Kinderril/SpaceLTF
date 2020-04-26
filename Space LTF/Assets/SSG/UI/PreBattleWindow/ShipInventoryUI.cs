using System;
using System.Collections.Generic;
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
    public CriticalDamageObject[] CriticalDamages;
    public SliderWithTextMeshPro HealthSlider;
    public DragableItemSlot CocpitSlot;
    public DragableItemSlot EngineSlot;
    public DragableItemSlot WingSlot;

    public TextMeshProUGUI RepairCost;
    public Button RepairButton;
    private StartShipPilotData _shipData;


    public void Init(StartShipPilotData shipData, bool usable, ConnectInventory connectedInventory)
    {
        //#if UNITY_EDITOR
        //
        //        var moduls = shipData.Ship.Moduls.SimpleModuls;
        //        for (int i = 0; i < moduls.Length; i++)
        //        {
        //            var modul = moduls[i];
        //            if (modul != null && modul.CurrentInventory == null)
        //            {
        //                moduls[i] = null;
        //            }
        //        }
        //#endif
        CocpitSlot.DragItemType = DragItemType.cocpit;
        EngineSlot.DragItemType = DragItemType.engine;
        WingSlot.DragItemType = DragItemType.wings;
        _shipData = shipData;
        _pilot = _shipData.Pilot;
        _shipInventory = _shipData.Ship;
        _pilot.OnLevelUp += OnLevelUp;
        _shipInventory.OnShipCriticalChange += OnShipCriticalChange;
        _shipInventory.OnShipRepaired += OnShipRepaired;
        _shipInventory.OnItemAdded += OnItemAdded;
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
        allSlots.Add(CocpitSlot);
        allSlots.Add(EngineSlot);
        allSlots.Add(WingSlot);
        base.Init(_shipInventory, usable, allSlots, connectedInventory);
        InitCurrentItems();
        NameField.text = _shipInventory.Name;
        ConfigType.text = Namings.ShipConfig(_shipInventory.ShipConfig);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(_shipInventory.ShipType);
        //        ConfigType.sprite = DataBaseController.Instance.DataStructPrefabs.Get(shipInventory.ShipType);
        //        Slots = new InventorySlots(shipInventory, allSlots);
        SetLevelUpField();
        UpdateCriticalDamages();
        if (CriticalDamages.Length != Library.CRITICAL_DAMAGES_TO_DEATH)
        {
            Debug.LogError($"Wrong count of critical damages must be {Library.CRITICAL_DAMAGES_TO_DEATH}");
        }

        RefreshParameters();

    }

    private void OnItemAdded(IItemInv item, bool val)
    {
        RefreshParameters();
    }

    private void RefreshParameters()
    {
        UpdateHealth();

    }


    public void OnClickDismiss()
    {
        WindowManager.Instance.ConfirmWindow.Init(DismissOk, null, Namings.Format(Namings.Tag("confirmDismiss"), _shipInventory.Name));
    }

    private void DismissOk()
    {
        _shipInventory.Owner.Army.RemoveShip(_shipInventory);

    }

    private void OnShipRepaired(ShipInventory obj)
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        var repairCost = _shipData.MoneyToFullRepair();
        var shallRepair = _shipInventory.HealthPointToRepair() > 0;
        RepairButton.gameObject.SetActive(shallRepair);
        RepairCost.text = repairCost.ToString();
        var MaxHealth = ShipParameters.ParamUpdate(_shipInventory.MaxHealth, _pilot.HealthLevel, ShipParameters.MaxHealthCoef);
        HealthSlider.InitBorders(0, MaxHealth, true);
        HealthSlider.Slider.value = _shipInventory.HealthPercent * MaxHealth;
    }


    public void OnRepairClick()
    {
        _shipData.TryRepairSelfFull();
    }

    private void OnShipCriticalChange(ShipInventory obj)
    {
        UpdateCriticalDamages();
    }

    private void UpdateCriticalDamages()
    {
        for (int i = 0; i < Library.CRITICAL_DAMAGES_TO_DEATH; i++)
        {
            var haveDmg = i < _shipInventory.CriticalDamages;
            CriticalDamages[i].gameObject.SetActive(haveDmg);
        }
    }
    private void OnLevelUp(IPilotParameters obj)
    {
        SetLevelUpField();
        UpdateHealth();
    }

    private void SetLevelUpField()
    {
        LevelField.text = _pilot.CurLevel.ToString();
    }

    private HashSet<DragableItemSlot> InitFreeSlots()
    {
        HashSet<DragableItemSlot> allslots = new HashSet<DragableItemSlot>();
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
            itemSlot.Init(_shipInventory, true);
            itemSlot.transform.SetParent(ModulsLayout, false);
            itemSlot.DragItemType = DragItemType.modul;
            //            _allSlots.Add(itemSlot);
        }

        return allslots;
    }

    private void InitCurrentItems()
    {
        CocpitSlot.Init(_shipInventory, true);
        EngineSlot.Init(_shipInventory, true);
        WingSlot.Init(_shipInventory, true);

        InitCurrent(_shipInventory.CocpitSlot, CocpitSlot);
        InitCurrent(_shipInventory.EngineSlot, EngineSlot);
        InitCurrent(_shipInventory.WingSlot, WingSlot);
        
        for (int i = 0; i < _shipInventory.WeaponsModuls.Length; i++)
        {
            var weapon = _shipInventory.WeaponsModuls[i];
            var slot = GetFreeSlot(ItemType.weapon);
            //            slot.Init(_shipInventory, _usable);
            SetStartItem(slot, weapon);
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
            SetStartItem(slot, modul);
        }
        //        for (int i = 0; i < _shipInventory.SpellsModuls.Length; i++)
        //        {
        //            var spell = _shipInventory.SpellsModuls[i];
        //            var slot = GetFreeSlot(ItemType.spell);
        //            slot.Init(_shipInventory, _usable);
        //            SetStartItem(slot,spell);
        //        }
    }

    private void InitCurrent(ParameterItem parameterItem,DragableItemSlot slot)
    {
        if (parameterItem != null)
        {
            SetStartItem(slot, parameterItem);
        }
    }

    public override void Dispose()
    {
        _shipInventory.OnShipRepaired -= OnShipRepaired;
        _shipInventory.OnItemAdded -= OnItemAdded;
        _shipInventory.OnShipCriticalChange -= OnShipCriticalChange;
        _pilot.OnLevelUp -= OnLevelUp;
        base.Dispose();
    }
}

