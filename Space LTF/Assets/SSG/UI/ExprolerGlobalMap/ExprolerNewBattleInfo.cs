using UnityEngine;
using System.Collections;
using TMPro;

public class ExprolerNewBattleInfo : MonoBehaviour
{
    private ExprolerGlobalMapCell _cell;
    public TextMeshProUGUI CaptureField;
    public TextMeshProUGUI PowerField;
    public TextMeshProUGUI ConfigField;
    public void Init(ExprolerGlobalMapCell cell)
    {
        _cell = cell;
        gameObject.SetActive(true);
        CaptureField.text = $"{Namings.Tag("Sector")}: {_cell.Id}";
        ConfigField.text = $"{Namings.Tag("Fraction")}: {Namings.ShipConfig(_cell.Config)}";
        PowerField.text = $"{Namings.Tag("Power")}: {_cell.Power}";

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
