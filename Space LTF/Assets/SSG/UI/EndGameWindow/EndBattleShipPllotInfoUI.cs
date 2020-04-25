using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndBattleShipPllotInfoUI : MonoBehaviour
{
    public TextMeshProUGUI IconConfig;
    public Image IconType;
    public GameObject CanUpgradeIcon;
    public TextMeshProUGUI FieldCurrentMoney;
    public MoneySlotUI FieldAddMoney;
    public TextMeshProUGUI FieldName;
    public TextMeshProUGUI FieldDamageHealth;
    public TextMeshProUGUI FieldDamageShield;
    public TextMeshProUGUI HealedHpField;
    public StartShipPilotData StartShipPilotData { get; set; }
    public int MoneyToAdd { get; set; }

    public void Init(StartShipPilotData info)
    {
        StartShipPilotData = info;
        var canUpgradeAnyParameter = StartShipPilotData.Pilot.CanUpgradeAnyParameter(0);
        CanUpgradeIcon.gameObject.SetActive(canUpgradeAnyParameter);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(info.Ship.ShipType);
        IconConfig.text = info.Ship.ShipConfig.ToString();
        FieldCurrentMoney.text = info.Pilot.Money.ToString();
        FieldName.text = info.Ship.Name;
        FieldAddMoney.Init(0);
        FieldDamageHealth.text = Namings.Tag("DamageBody") + ":" + info.Ship.LastBattleData.HealthDamage.ToString("0");
        FieldDamageShield.text = Namings.Tag("DamageShield") + ":" + info.Ship.LastBattleData.ShieldhDamage.ToString("0");
        var MaxHealth = ShipParameters.ParamUpdate(info.Ship.MaxHealth, info.Pilot.HealthLevel, ShipParameters.MaxHealthCoef);
//        var hp = MaxHealth;
        MoneyToAdd = 0;
        var cur = info.Ship.HealthPercent * MaxHealth;
        HealedHpField.text = Namings.Tag("Health") + $":{cur}/{MaxHealth}";
    }

    public void SetMoneyAdd(int addMoney)
    {
        MoneyToAdd = addMoney;
        FieldAddMoney.Init(addMoney, "+");//.ToString();
        var canUpgradeAnyParameter = StartShipPilotData.Pilot.CanUpgradeAnyParameter(addMoney);
        CanUpgradeIcon.gameObject.SetActive(canUpgradeAnyParameter);
    }
}
