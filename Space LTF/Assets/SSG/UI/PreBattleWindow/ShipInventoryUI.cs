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
    public UIElementWithTooltipCache _cofigBonus;


    public void Init(StartShipPilotData shipData, bool usable, ConnectInventory connectedInventory,IInventory tradeInventory = null)
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
        _tradeInventory = tradeInventory;
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
        _shipInventory.Moduls.OnSlotsRemove += OnSlotsRemove;
        _shipInventory.Moduls.OnSlotsAdd += OnSlotsAdd;
        _shipInventory.WeaponsModuls.OnSlotsAdd += OnSlotsAddWeapon;
        _shipInventory.WeaponsModuls.OnSlotsRemove += OnSlotsRemoveWeapon;
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
        _cofigBonus.Cache = Namings.TooltipConfigProsConsCalc(_shipData.Ship.ShipConfig);

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
        _shipInventory.Owner.RemoveShip(_shipData);

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
        var calulatedParams = ShipParameters.CalcParams(_shipInventory, _pilot, new List<EParameterShip>()
        {
            EParameterShip.bodyPoints
        });
        var MaxHealth = calulatedParams[EParameterShip.bodyPoints];
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
        }
        for (int i = 0; i < _shipInventory.SimpleModulsCount; i++)
        {
            var itemSlot = InventoryOperation.GetDragableItemSlot();
            allslots.Add(itemSlot);
            itemSlot.Init(_shipInventory, true);
            itemSlot.transform.SetParent(ModulsLayout, false);
            itemSlot.DragItemType = DragItemType.modul;
        }
        return allslots;
    }
    private void OnSlotsRemoveWeapon(ShipWeaponsInventory arg1, DragItemType type)
    {
        RemoveSlot(type);
    }     
    private void OnSlotsRemove(ShipModulsInventory arg1, DragItemType type)
    {
        RemoveSlot(type);
    }

    private void OnSlotsAdd(ShipModulsInventory arg1,DragItemType type)
    {
        AddModulSlot(type);
    }   
    private void OnSlotsAddWeapon(ShipWeaponsInventory arg1,DragItemType type)
    {
        AddWeaponSlot(type);
    }

    private void AddModulSlot(DragItemType type)
    {
        var itemSlot = InventoryOperation.GetDragableItemSlot();
        itemSlot.Init(_shipInventory, true);
        itemSlot.transform.SetParent(ModulsLayout, false);
        itemSlot.DragItemType = type;
        AddSlot(itemSlot);
    }   

    private void AddWeaponSlot(DragItemType type)
    {
        var itemSlot = InventoryOperation.GetDragableItemSlot();
        itemSlot.Init(_shipInventory, true);
        itemSlot.transform.SetParent(WeaponsLayout, false);
        itemSlot.DragItemType = type;
        AddSlot(itemSlot);
    }

    private void RemoveSlot(DragItemType type)
    {
        var slot = FindSlotToRemove(type);
        if (slot != null)
        {
            RemoveSlot(slot);
        }

    }

    private void InitCurrentItems()
    {
        CocpitSlot.Init(_shipInventory, true);
        EngineSlot.Init(_shipInventory, true);
        WingSlot.Init(_shipInventory, true);

        InitCurrent(_shipInventory.CocpitSlot, CocpitSlot);
        InitCurrent(_shipInventory.EngineSlot, EngineSlot);
        InitCurrent(_shipInventory.WingSlot, WingSlot);

        var weapon = _shipInventory.WeaponsModuls.GetNonNullActiveSlots();
        foreach (var weaponInv in weapon)
        {
            var slot = GetFreeSlot(ItemType.weapon);
            SetStartItem(slot, weaponInv, _tradeInventory);
        }

        foreach (var modulInv in _shipInventory.Moduls.GetNonNullActiveSlots())
        {
            var slot = GetFreeSlot(ItemType.modul);
            SetStartItem(slot, modulInv, _tradeInventory);
        }
    }

    private void InitCurrent(ParameterItem parameterItem,DragableItemSlot slot)
    {
        if (parameterItem != null)
        {
            SetStartItem(slot, parameterItem,_tradeInventory);
        }
    }

    public override void Dispose()
    {
        _shipInventory.Moduls.OnSlotsRemove -= OnSlotsRemove;
        _shipInventory.Moduls.OnSlotsAdd -= OnSlotsAdd;
        _shipInventory.WeaponsModuls.OnSlotsAdd -= OnSlotsAddWeapon;
        _shipInventory.WeaponsModuls.OnSlotsRemove -= OnSlotsRemoveWeapon;
        _shipInventory.OnShipRepaired -= OnShipRepaired;
        _shipInventory.OnItemAdded -= OnItemAdded;
        _shipInventory.OnShipCriticalChange -= OnShipCriticalChange;
        _pilot.OnLevelUp -= OnLevelUp;
        base.Dispose();
    }
}

