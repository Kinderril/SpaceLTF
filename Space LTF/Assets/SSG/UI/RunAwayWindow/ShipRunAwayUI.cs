using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShipRunAwayUI : MonoBehaviour
{
    public TextMeshProUGUI Health;
    public Image IconType;
    public TextMeshProUGUI ConfigType;
    public TextMeshProUGUI NameField;


    public void Init(ShipInventory shipInventory, IPilotParameters pilot)
    {
        NameField.text = shipInventory.Name;
        ConfigType.text = Namings.ShipConfig(shipInventory.ShipConfig);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(shipInventory.ShipType);
        SetCurHealths(shipInventory, pilot);
    }

    private void SetCurHealths(ShipInventory shipInventory,IPilotParameters pilot)
    {
        var MaxHealth = ShipParameters.ParamUpdate(shipInventory.MaxHealth, pilot.HealthLevel, ShipParameters.MaxHealthCoef);
        var curHp = MaxHealth * shipInventory.HealthPercent;
        Health.text = Namings.Tag("Health") + ":" + curHp.ToString("0") + "/" + PilotInventoryUI.Info(MaxHealth, pilot.HealthLevel);
    }
}

