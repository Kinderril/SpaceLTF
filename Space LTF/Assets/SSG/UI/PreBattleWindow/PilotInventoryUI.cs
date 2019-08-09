using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PilotInventoryUI : MonoBehaviour
{
    private IPilotParameters _pilot;
    private ShipInventory _ship;
    public SliderWithTextMeshPro DelaySlider;
    public ParameterWithLevelUp HealthField;
    public MoneySlotUI MoneyField;
    public Slider LevelUpSlider;
    public ParameterWithLevelUp ShieldField;
    public ParameterWithLevelUp SpeedField;
    public TextMeshProUGUI TacticField;
    public TextMeshProUGUI RankField;
    public TextMeshProUGUI KillsField;
    public Image TacticIcon;
    public Image RankIcon;
    public ParameterWithLevelUp TurnField;
    public Button LevelUpButton;

//    public GameObject LevelUpObject;
//    public TextMeshProUGUI LevelUpField;
    
    public void Init(IPilotParameters pilot, ShipInventory ship)
    {
        _ship = ship;
        _pilot = pilot;
        _ship.OnShipRepaired += OnShipRepaired;
        _pilot.OnTacticChange += OnTacticChange;
        _pilot.OnLevelUp += OnLevelUp;
        UpdateTacticField();
        DelaySlider.InitBorders(0f,20f,true);
        DelaySlider.InitName("Delay");
        DelaySlider.SetValue(pilot.Delay);
        DelaySlider.InitCallback(SliderChange);
        if (_ship.ShipType == ShipType.Base)
        {
            DelaySlider.enabled = false;
        }

        RankUpdate();
        SetParamsAndMoney();

        //HealthField.SetLevelUp(pilot, LibraryPilotUpgradeType.health);
        //SpeedField.SetLevelUp(pilot, LibraryPilotUpgradeType.speed);
        //ShieldField.SetLevelUp(pilot, LibraryPilotUpgradeType.shield);
        //TurnField.SetLevelUp(pilot, LibraryPilotUpgradeType.turnSpeed);
    }

    private void RankUpdate()
    {
        RankIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetRankSprite(_pilot.Stats.CurRank);
        RankField.text =_pilot.Stats.CurRank.ToString();
        var kills = _pilot.Stats.Kills;
        var nextKills = (((int) (kills / 10)) + 1) * 10;
        KillsField.text = $"Kills{kills}/{nextKills}";
    }

//    public void OnLevelUpClick()
//    {
//        if (_pilot.CanUpgradeAnyParameter(0))
//        {
//            _pilot.UpgradeLevel(true,type);
//        }
//    }

    private void OnLevelUp(IPilotParameters obj)
    {
        SetParamsAndMoney();
    }

    private void SetParamsAndMoney()
    {
//        LevelUpButton.gameObject.SetActive(_pilot.CanUpgradeAnyParameter(0));
        MoneyField.Init(_pilot.Money);
        SetInfoParams();
        LevelUpSlider.value = _pilot.PercentLevel;
    }

    private void UpdateTacticField()
    {
        TacticIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_pilot.Tactic);
        TacticField.text = Namings. TacticName(_pilot.Tactic);
    }

    private void SliderChange()
    {
        if (_ship.ShipType == ShipType.Base)
        {
            return;
        }
        _pilot.Delay = DelaySlider.GetValueInt();
    }


    private void OnShipRepaired(ShipInventory obj)
    {
        SetParamsAndMoney();
        SetCurHealths();
    }

    private void SetCurHealths()
    {
        var MaxHealth = ShipParameters.ParamUpdate(_ship.MaxHealth, _pilot.HealthLevel, ShipParameters.MaxHealthCoef);
        var curHp = MaxHealth * _ship.HealthPercent;
        var hpTxt = String.Format("{0}/{1}", curHp.ToString("0"),Info(MaxHealth, _pilot.HealthLevel));
        var txt = LevelInfo(Namings.Health, _pilot.HealthLevel, hpTxt);
        HealthField.SetData(txt,_pilot.HealthLevel,_pilot,LibraryPilotUpgradeType.health);
    }

    private void SetInfoParams()
    {
        var MaxSpeed = ShipParameters.ParamUpdate(_ship.MaxSpeed, _pilot.SpeedLevel, ShipParameters.MaxSpeedCoef);
        var TurnSpeed = ShipParameters.ParamUpdate(_ship.TurnSpeed, _pilot.TurnSpeedLevel, ShipParameters.TurnSpeedCoef);
        var maxShiled = ShipParameters.ParamUpdate(_ship.MaxShiled, _pilot.ShieldLevel, ShipParameters.MaxShieldCoef);

        var shiledInfo = LevelInfo(Namings.Shield, _pilot.ShieldLevel,Info(maxShiled, _pilot.ShieldLevel));
        var speedInfo = LevelInfo(Namings.Speed, _pilot.SpeedLevel, Info(MaxSpeed, _pilot.SpeedLevel));
        var turnInfo = LevelInfo(Namings.TurnSpeed, _pilot.TurnSpeedLevel, Info(TurnSpeed, _pilot.TurnSpeedLevel));
//        var shiledInfo = LevelInfo(Namings.Shield, _pilot.ShieldLevel,Info(maxShiled, _pilot.ShieldLevel));

        ShieldField.SetData(shiledInfo, _pilot.ShieldLevel,_pilot,LibraryPilotUpgradeType.shield);
        SpeedField.SetData(speedInfo,_pilot.SpeedLevel,_pilot,LibraryPilotUpgradeType.speed);
        TurnField.SetData(turnInfo,_pilot.TurnSpeedLevel,_pilot,LibraryPilotUpgradeType.turnSpeed);
        SetCurHealths();
    }

    private static string LevelInfo(string name,int level,string info)
    {
        return String.Format("{0}:{2}", name, level, info);
    }

    public static string Info(float p,int level)
    {
        var drob = p - (int) p;
        string format = "0";
        if (drob > 0f)
        {
            format = "0.0";
        }

        return p.ToString(format);;
    }
    

    public void OnTacticHcangeClick()
    {
        _pilot.ChangeTactic();
    }

    private void OnTacticChange(IPilotParameters arg1, PilotTcatic arg2)
    {
        UpdateTacticField();
    }

    public void Dispose()
    {
//#if UNITY_EDITOR
//    if (_pilot == null)
//    {
//        Debug.LogError("id of pilot Ui is NULL pilot " + id);
//    }
//#endif
        if (_pilot != null)
        {
            _pilot.OnTacticChange -= OnTacticChange;
        }
        if (_pilot != null)
        {
            _pilot.OnLevelUp -= OnLevelUp;
        }
        if (_ship != null)
        {
            _ship.OnShipRepaired -= OnShipRepaired;
        }
    }
}