using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class ArmyTypeSelectorUI : MonoBehaviour
{
    public Transform Layout;
    public StartArmyChooserUI PrefabStartArmyChooserUI;
    public ShipConfig Selected { get; private set; }
    public ToggleGroup ToggleGroup;
    private List<StartArmyChooserUI> _toggleElements = new List<StartArmyChooserUI>(); 

    public void Init()
    {
        Selected = ShipConfig.mercenary;
        bool oneIsOpen = false;
        _toggleElements.Clear();
        foreach (var sc in MainController.Instance.Statistics.OpenShipsTypes)
        {
            var element = DataBaseController.GetItem(PrefabStartArmyChooserUI);
            element.transform.SetParent(Layout, false);
            var interactable = sc.IsOpen;
            element.Init(sc.Config,OnClickSelect, interactable);
            element.Toggle.group = ToggleGroup;
            if (sc.IsOpen && !oneIsOpen)
            {
                oneIsOpen = true;
                element.Toggle.isOn = true;
            }
            _toggleElements.Add(element);

        }
    }

    private void OnClickSelect(StartArmyChooserUI obj)
    {
        Selected = obj.Config;
    }

    public void Dispose()
    {
        Layout.ClearTransform();
    }

    public void SetData(ShipConfig config)
    {
        Selected = config;
        var element = _toggleElements.First(x => x.Config == config);
        element.Toggle.isOn = true;
    }
}

