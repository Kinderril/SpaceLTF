using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ExprolerGlobalMapCell : MonoBehaviour
{
    public GameObject BlockObject;
    public GameObject UnblockObject;
    public TextMeshProUGUI LevelField;
    public int Id;
    public int Power = 10;
//    public int Size = 6;
    public ShipConfig Config = ShipConfig.droid;
    public UIElementWithTooltipCache Tooltip;
    public List<ExprolerGlobalMapCell> Neighhoods = new List<ExprolerGlobalMapCell>();
    private Action<ExprolerGlobalMapCell> _clickCell;

    public bool IsOpen;

    public void Init()
    {
        LevelField.text = Power.ToString();
        UnblockObject.SetActive(false);
        BlockObject.SetActive(true);
        Tooltip.Cache = $"{Namings.ShipConfig(Config)} Power:{Power}";// {Namings.Tag("Size")}:{Size}";
    }

    public void Unblock()
    {
        IsOpen = true;
//        UnblockObject.SetActive(true);
        BlockObject.SetActive(false);

    }

    public void Complete()
    {
        UnblockObject.SetActive(true);
    }

    public void SetClickCallback(Action<ExprolerGlobalMapCell> clickCell)
    {
        _clickCell = clickCell;
    }

    public void OnClickCell()
    {
        _clickCell(this);
    }
}
