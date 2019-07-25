using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SideShipMiniInfo : MonoBehaviour
{

    public ShipSlidersInfo ShipSlidersInfo;
    public Image ShipTypeIcon;

    private ShipBase _ship;
    public void Init(ShipBase ship)
    {
        _ship = ship;
        ShipSlidersInfo.Init(ship);
        ShipTypeIcon.sprite =
            DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(ship.ShipParameters.StartParams.ShipType);
    }

    public void Dispose()
    {
        ShipSlidersInfo.Dispose();
    }
}