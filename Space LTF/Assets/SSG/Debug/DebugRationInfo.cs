using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class DebugRationInfo : MonoBehaviour
{
    public Text Dists;
    public Text Pilot;
    public Text Total;
    public Image ShipShow;
    public Image MyPosShow;
    public Image TargetShow;
    public Image MyShipShow;
    private DebugRating _rating;
    public Transform TextContainer;

    private void UpdateInfo()
    {
        Dists.text = _rating.Dists();
        Pilot.text = _rating.Helps();
        Total.text = _rating.Total();
    }

    void Update()
    {
        if (_rating == null)
        {
            return;
        }
        var cam = CamerasController.Instance.GameCamera;
        UpdateInfo();
        MyPosShow.transform.position = cam.WorldToScreenPoint(_rating.MyPos);
        ShipShow.transform.position = cam.WorldToScreenPoint(_rating.Ship.Position);
        MyShipShow.transform.position = cam.WorldToScreenPoint(_rating.MyShip.Position);
        TargetShow.transform.position = cam.WorldToScreenPoint(_rating.Pos);
        TextContainer.transform.position = cam.WorldToScreenPoint(_rating.Ship.Position);
    }

    public void SetInfo(DebugRating rating)
    {
        _rating = rating;
    }
}

