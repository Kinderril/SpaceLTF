using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class StatisticResultElement : MonoBehaviour
{
    public TextMeshProUGUI DifficultyField;
    public TextMeshProUGUI ConfigField;
//    public Image ConfigField;
    public TextMeshProUGUI MapSizeField;
    public TextMeshProUGUI DateField;
    public TextMeshProUGUI FinalArmyPowerField;

    public void Init(EndGameResult result)
    {
        DifficultyField.text = String.Format(Namings.StatisticDifficulty, result.Difficulty);
//        ConfigField.sprite = DataBaseController.Instance.DataStructPrefabs.icon 
//        ConfigField.ima
//            ;result.Config);
        ConfigField.text = String.Format(Namings.StatisticConfig, result.Config);
        MapSizeField.text = String.Format(Namings.StatisticMapSize, result.MapSize);
        DateField.text = String.Format(Namings.StatisticDate, String.Format("{0:d/M/yyyy HH:mm:ss}", result.Date));
        FinalArmyPowerField.text = String.Format(Namings.StatisticFinalArmyPower, result.FinalArmyPower); 
    }

}
