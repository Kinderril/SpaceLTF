using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ExprolerProfileStartShipCell : MonoBehaviour
{
    public Image Icon;
//    public TextMeshProUGUI LevelField;
    public UIElementWithTooltipCache tooltip;
    public void Init(StartShipPilotData data)
    {
        var tooltipText = $"{Namings.ShipConfig(data.Ship.ShipConfig)}  {Namings.Tag("Level")}: {data.Pilot.CurLevel.ToString()}";
        tooltip.Cache = tooltipText;
        Icon.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(data.Ship.ShipType);
        Icon.color = Library.GetColorByConfig(data.Ship.ShipConfig);
    }
}
