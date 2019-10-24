using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SideShipGlobalMapInfo : MonoBehaviour
{
    public StartShipPilotData Ship => _ship;
    private StartShipPilotData _ship;
    public GameObject LevelUpButton;
    public GameObject RepairButton;
//    public TextMeshProUGUI RepairCost;
    public SliderWithTextMeshPro HealthSlider;
    public MoneySlotUI Money;
    public TextMeshProUGUI RepairCostField;
    public TextMeshProUGUI LevelField;
    public Image RankImage;
    public GameObject[] CriticalDamages;
    private MapWindow _window;


    public void Init(StartShipPilotData ship,MapWindow window)
    {
        _window = window;
        _ship = ship;
        EnableActions();
        UpToDate();
        if (CriticalDamages.Length != Library.CRITICAL_DAMAGES_TO_DEATH)
        {
            Debug.LogError($"Wrong count of critical damages must be {Library.CRITICAL_DAMAGES_TO_DEATH}");
        }
    }

    public void EnableActions()
    {
        _ship.Ship.OnShipRepaired += OnShipRepaired;
        _ship.Pilot.OnLevelUp += OnLevelUp;
        _ship.Pilot.Stats.OnRankChange += OnRankChange;
        _ship.Ship.OnShipCriticalChange += OnShipCriticalChange;

    }

    private void OnShipCriticalChange(ShipInventory obj)
    {
        UpdateCriticalDamages();
    }

    private void OnRankChange(PilotRank obj)
    {
        UpToDate();
    }

//    public void OnLevelUpClick()
//    {
//         //        _ship.Pilot.UpgradeLevel(true);
//    }

//    public void OnRepairClick()
//    {                      
//        _ship.TryRepairSelfFull();
//    }

    public void OnClickToInventiry()
    {
        _window.OnArmyShowClick();
    }

    private void OnLevelUp(IPilotParameters obj)
    {
        UpToDate();
    }

    private void OnShipRepaired(ShipInventory obj)
    {
        UpToDate();
    }

    private void UpdateCriticalDamages()
    {

        for (int i = 0; i < Library.CRITICAL_DAMAGES_TO_DEATH; i++)
        {
            var haveDmg = i < _ship.Ship.CriticalDamages;
            CriticalDamages[i].SetActive(haveDmg);
        }
    }

    public void UpToDate()
    {
        UpdateCriticalDamages();
        LevelField.text = _ship.Pilot.CurLevel.ToString("0");
        var costRepair = _ship.MoneyToFullRepair();
        var haveToReRepair = costRepair > 0; 
        RepairCostField.gameObject.SetActive(haveToReRepair);
        if (haveToReRepair)
        {
            RepairCostField.text = costRepair.ToString("0");
        }
        var MaxHealth = ShipParameters.ParamUpdate(_ship.Ship.MaxHealth, _ship.Pilot.HealthLevel, ShipParameters.MaxHealthCoef);
        HealthSlider.InitBorders(0, MaxHealth,true);
        HealthSlider.Slider.value = _ship.Ship.HealthPercent * MaxHealth;
        var shallRepair = _ship.Ship.HealthPointToRepair() > 0;
        RepairButton.gameObject.SetActive(shallRepair);
        Money.Init(_ship.Pilot.Money);
        LevelUpButton.gameObject.SetActive(_ship.Pilot.CanUpgradeAnyParameter(0));
        RankImage.sprite = DataBaseController.Instance.DataStructPrefabs.GetRankSprite(_ship.Pilot.Stats.CurRank);
        int index = 0;
        foreach (var criticalDamage in CriticalDamages)
        {
            var isActive = index < _ship.Ship.CriticalDamages;
            index++;
            criticalDamage.gameObject.SetActive(isActive);
        }
    }

    public void Dispose()
    {
        _ship.Ship.OnShipRepaired -= OnShipRepaired;
        _ship.Pilot.OnLevelUp -= OnLevelUp;
        _ship.Pilot.Stats.OnRankChange -= OnRankChange;
        _ship.Ship.OnShipCriticalChange -= OnShipCriticalChange;
    }
}
