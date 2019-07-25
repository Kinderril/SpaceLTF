using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class SideShipInfoMini : MonoBehaviour
{
    public ShipSlidersInfo ShipSlidersInfo;
    public Image ShipTypeIcon;

    public void Init(ShipBase ship)
    {
        ShipTypeIcon.sprite =
            DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(
                ship.ShipParameters.StartParams.ShipType);

        ShipSlidersInfo.Init(ship);
    }
}

