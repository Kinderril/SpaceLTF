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
    public Image GoodBad;
    private Color winColor = new Color(109f/255f, 1, 110f / 255f, 48f / 255f);
    private Color loseColor = new Color(241f/255f, 73f/255f, 71f / 255f, 48f / 255f);

    public void Init(EndGameResult result)
    {
        DifficultyField.text = String.Format(Namings.StatisticDifficulty, Utils.FloatToChance(result.Difficulty));
        ConfigField.text = String.Format(Namings.StatisticConfig, result.Config);
        MapSizeField.text = String.Format(Namings.StatisticMapSize, result.MapSize);
        DateField.text = String.Format(Namings.StatisticDate, String.Format("{0:d/M/yyyy HH:mm:ss}", result.Date));
        FinalArmyPowerField.text = String.Format(Namings.StatisticFinalArmyPower, result.FinalArmyPower);
        if (GoodBad != null)
        {
            GoodBad.color = result.Win ? winColor : loseColor;
        }
    }

}
