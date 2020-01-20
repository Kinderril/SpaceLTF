using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class StartGameWeaponsChooseUI :MonoBehaviour
{
    public Transform Layout;
    public StartWeaponToggleUI PrefabStartWeaponToggleUI;
    public WeaponsPair Selected { get; private set; }
    public ToggleGroup ToggleGroup;
    private List<StartWeaponToggleUI> _toggleElements = new List<StartWeaponToggleUI>();

    public void Init()
    {
        _toggleElements.Clear();
        var pairs = MainController.Instance.Statistics.WeaponsPairs;
        Selected = pairs.FirstOrDefault(x => x.IsOpen);
        foreach (var sc in pairs)
        {
            var element = DataBaseController.GetItem(PrefabStartWeaponToggleUI);
            element.transform.SetParent(Layout, false);
            var interactable = sc.IsOpen;
            element.Init(sc, OnClickSelect, interactable);
            element.Toggle.group = ToggleGroup;
            if (sc == Selected)
            {
                element.Toggle.isOn = true;
            }
            _toggleElements.Add(element);

        }
    }

    private void OnClickSelect(StartWeaponToggleUI obj)
    {
        Selected = obj.WeaponsPair;
    }

    public void Dispose()
    {
        Layout.ClearTransform();
    }

    public void SetData(WeaponsPair weaponsPair)
    {
        Selected = weaponsPair;
        var element = _toggleElements.First(x => x.WeaponsPair == weaponsPair);
        element.Toggle.isOn = true;

    }

    public void DebugOpenAll()
    {
        foreach (var toggleUi in _toggleElements)
        {
            toggleUi.Toggle.interactable = true;
        }
    }

    public void CheckOpens()
    {
        foreach (var sc in MainController.Instance.Statistics.WeaponsPairs)
        {
            var element = _toggleElements.First(x => x.WeaponsPair == sc);
            element.Init(sc, OnClickSelect, sc.IsOpen);
        }
    }
}

