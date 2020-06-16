using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class ExprolerNewBattleInfo : MonoBehaviour
{
    private ExprolerGlobalMapCell _cell;
    public TextMeshProUGUI CaptureField;
    public TextMeshProUGUI PowerField;
    public TextMeshProUGUI ConfigField;
    public TextMeshProUGUI BattleTypeField;
    public GameObject StarSize4;
    public GameObject StarSize5;
    public GameObject StarSize6;
    public void Init(ExprolerGlobalMapCell cell)
    {
        _cell = cell;
        var slots = MainController.Instance.SafeContainers;
        if (slots.ContainsCompleteId(cell.Id,out var keys))
        {
            StarSize4.SetActive(keys.Contains(4));
            StarSize5.SetActive(keys.Contains(5));
            StarSize6.SetActive(keys.Contains(6));
        }
        else
        {
            StarSize4.SetActive(false);
            StarSize5.SetActive(false);
            StarSize6.SetActive(false);
        }


        gameObject.SetActive(true);
        CaptureField.text = $"{Namings.Tag("Sector")}: {_cell.Id}";
        ConfigField.text = $"{Namings.Tag("Fraction")}: {Namings.ShipConfig(_cell.Config)}";
        PowerField.text = $"{Namings.Tag("Power")}: {_cell.Power}";
        switch (_cell.MapType)
        {
            case ExprolerCellMapType.normal:
                BattleTypeField.text = $"{Namings.Tag("battleTypeNormal")}";
                break;
            case ExprolerCellMapType.milatary:
                BattleTypeField.text = $"{Namings.Tag("battleTypemilatary")}";
                break;
            case ExprolerCellMapType.longType:
                BattleTypeField.text = $"{Namings.Tag("battleTypelongType")}";
                break;
        }

    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }


    public void OnClick4()
    {

        StartPlay(4);
    }   
    public void OnClick5()
    {

        StartPlay(5);
    }   
    public void OnClick6()
    {
        StartPlay(6);
    }

    private void StartPlay(int size)
    {

        MainController.Instance.Exproler.PlaySector(_cell, size );
        gameObject.SetActive(false);
    }
}
