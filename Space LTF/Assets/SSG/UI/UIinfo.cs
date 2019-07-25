using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UIinfo : MonoBehaviour
{

    private ShipBase _selectedShip;

    public void Init(ShipBase shipBase)
    {
        _selectedShip = shipBase;
        InitStableInfo();
        SubscribeOnShip();
    }

    private void SubscribeOnShip()
    {
        
    }

    private void InitStableInfo()
    {
        
    }
}

