using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RepairShipUI : MonoBehaviour
{
    public Image TypeIcon;
    public TextMeshProUGUI NameField;

    public void Init(ShipInventory ship)
    {
        TypeIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(ship.ShipType);
        NameField.text = ship.Name;
    }

    public void OnFullRepairClick()
    {
        
    }
}

