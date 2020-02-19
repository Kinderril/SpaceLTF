using System;
using TMPro;
using UnityEngine;
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
    private Color winColor = new Color(109f / 255f, 1, 110f / 255f, 48f / 255f);
    private Color loseColor = new Color(241f / 255f, 73f / 255f, 71f / 255f, 48f / 255f);

    public void Init(EndGameResult result)
    {
        DifficultyField.text = Namings.Format(Namings.Tag("StatisticDifficulty"), result.Difficulty.ToString("0"));
        ConfigField.text = Namings.Format(Namings.Tag("StatisticConfig"), result.Config);
        MapSizeField.text = Namings.Format(Namings.Tag("StatisticMapSize"), result.MapSize);
        var dateStr = String.Format("{0:d/M/yyyy HH:mm:ss}", result.Date);
        DateField.text = Namings.Format(Namings.Tag("StatisticDate"), dateStr);
        FinalArmyPowerField.text = Namings.Format(Namings.Tag("StatisticFinalArmyPower"), result.FinalArmyPower);
        if (GoodBad != null)
        {
            GoodBad.color = result.Win ? winColor : loseColor;
        }
    }

}
