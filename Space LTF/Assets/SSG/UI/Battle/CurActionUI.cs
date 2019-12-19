using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CurActionUI : MonoBehaviour
{
    public TextMeshProUGUI ActionText;
    // public TextMeshProUGUI TacticText;
//    public Image ActionImage;
    private ShipBase _ship;

    public void Init(ShipBase ship)
    {
        _ship = ship;
        _ship.OnDispose += OnDispose;
        _ship.OnActionChange += OnActionChange;
        // _ship.OnShipDesicionChange += OnShipDesicionChange;
        OnActionChange(_ship, _ship.CurAction);
        // TacticText.text = ship.DesicionData.GetName();
    }

    // private void OnShipDesicionChange(ShipBase arg1, IShipDesicion arg2)
    // {
    //     // TacticText.text = arg2.GetName();
    // }

    private void OnActionChange(ShipBase arg1, BaseAction arg2)
    {
        if (arg2 == null)
        {
            ActionText.text = Namings.Processing; 
        }
        else
        {
            ActionText.text = Namings.ActionName(arg2.ActionType);
        }
    }

    private void OnDispose(ShipBase obj)
    {
        Dispose();
    }

    private void Dispose()
    {
        if (_ship != null)
        {
            _ship.OnActionChange -= OnActionChange;
            // _ship.OnShipDesicionChange -= OnShipDesicionChange;
            _ship.OnDispose -= OnDispose;
        }
    }

}

