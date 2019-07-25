using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class SideShipGlobalMapInfo : MonoBehaviour
{
    private StartShipPilotData _ship;
    public GameObject LevelUpButton;
    public Button RepairButton;
    public TextMeshProUGUI RepairCost;
    public SliderWithTextMeshPro HealthSlider;
    public MoneySlotUI Money;
    public Image RankImage;


    public void Init(StartShipPilotData ship)
    {
        _ship = ship;
        EnableActions();
        UpToDate();
    }

    public void EnableActions()
    {
        _ship.Ship.OnShipRepaired += OnShipRepaired;
        _ship.Pilot.OnLevelUp += OnLevelUp;
        _ship.Pilot.Stats.OnRankChange += OnRankChange;

    }

    private void OnRankChange(PilotRank obj)
    {
        UpToDate();
    }

    public void OnLevelUpClick()
    {
        
//        _ship.Pilot.UpgradeLevel(true);
    }

    public void OnRepairClick()
    {                      
        _ship.TryRepairSelfFull();
    }

    private void OnLevelUp(IPilotParameters obj)
    {
        UpToDate();
    }

    private void OnShipRepaired(ShipInventory obj)
    {
        UpToDate();
    }

    public void UpToDate()
    {
        var MaxHealth = ShipParameters.ParamUpdate(_ship.Ship.MaxHealth, _ship.Pilot.HealthLevel, ShipParameters.MaxHealthCoef);
        HealthSlider.InitBorders(0, MaxHealth,true);
//        HealthSlider.    Field
        HealthSlider.Slider.value = _ship.Ship.HealthPercent * MaxHealth;
        var shallRepair = _ship.Ship.HealthPointToRepair() > 0;
        RepairCost.text = String.Format("Repair:{0}",_ship.MoneyToFullRepair().ToString());
        RepairCost.gameObject.SetActive(shallRepair);
        RepairButton.gameObject.SetActive(shallRepair);
        Money.Init(_ship.Pilot.Money);
        LevelUpButton.gameObject.SetActive(_ship.Pilot.CanUpgradeAnyParameter(0));
        RankImage.sprite = DataBaseController.Instance.DataStructPrefabs.GetRankSprite(_ship.Pilot.Stats.CurRank);

    }

    public void Dispose()
    {
        _ship.Ship.OnShipRepaired -= OnShipRepaired;
        _ship.Pilot.OnLevelUp -= OnLevelUp;
        _ship.Pilot.Stats.OnRankChange -= OnRankChange;
    }
}
