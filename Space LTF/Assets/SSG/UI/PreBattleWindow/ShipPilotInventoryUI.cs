using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ShipPilotInventoryUI : MonoBehaviour
{
//    public Image ImgTactic;
//    public Button ChangeTacticButton;
    public ShipInventoryUI ShipInventory;
    public PilotInventoryUI PilotInventory;

    private Action ontoggleSwitched;
    public Toggle ToggleElement;
//    public Image Destroyed;

    public void Init(StartShipPilotData data,bool usable, ConnectInventory connectedInventory,Action ontoggleSwitched)
    {
        this.ontoggleSwitched = ontoggleSwitched;
//        Destroyed.gameObject.SetActive(data.Ship.Destroyed);
        ShipInventory.Init(data.Ship, data.Pilot, usable, connectedInventory);
        var openInfoPilot = true;
        ToggleElement.interactable = usable;
        ToggleElement.isOn = openInfoPilot;
        PilotInventory.gameObject.SetActive(openInfoPilot);
        if (usable)
        {
            PilotInventory.Init(data.Pilot, data.Ship);
        }
//        else
//        {
//            PilotInventory.gameObject.SetActive(false);
//        }
    }

    public void OnEnableAdditionsClick()
    {
        PilotInventory.gameObject.SetActive(ToggleElement.isOn);
        ontoggleSwitched();
    }

    public void Dispose()
    {
        ShipInventory.Dispose();
        PilotInventory.Dispose();
    }

    public void Disable()
    {
        ShipInventory.Disable();
    }
    public void Enable()
    {
        ShipInventory.Enable();
    }
}

