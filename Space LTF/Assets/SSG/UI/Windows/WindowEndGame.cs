using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowEndGame : BaseWindow
{
    public TextMeshProUGUI StepsField;
    public TextMeshProUGUI BattleWinsField;
    public TextMeshProUGUI ConfigField;

    public TextMeshProUGUI DifficultyField;
    public TextMeshProUGUI MapSizeField;
    public TextMeshProUGUI FinalArmyPowerField;
    public TextMeshProUGUI RecievedOpendPointsField;
    public TextMeshProUGUI WinLoseField;

    public TextMeshProUGUI OpenDataField;
    public GameObject OpenDataContainer;
    public Image GooBad;
    public Color WinColor;
    public Color LoseColor;


    public override void Init()
    {
        var stat = MainController.Instance.Statistics;
        var player = MainController.Instance.MainPlayer;

        StepsField.text = Namings.Format(Namings.Tag("EndGameDays"), player.MapData.Step);
        var result = stat.EndGameStatistics.LastResult;
        DifficultyField.text = Namings.Format(Namings.Tag("StatisticDifficulty"), result.Difficulty);
        ConfigField.text = Namings.Format(Namings.Tag("StatisticConfig"), result.Config);
        MapSizeField.text = Namings.Format(Namings.Tag("StatisticMapSize"), result.MapSize);
        FinalArmyPowerField.text = Namings.Format(Namings.Tag("StatisticFinalArmyPower"), result.FinalArmyPower);
        RecievedOpendPointsField.text = Namings.Format(Namings.Tag("StatisticRecievedOpendPoints"), result.RecievedOpendPoints);

        BattleWinsField.text = Namings.Format(Namings.Tag("BattleWinsStat"), stat.Wins);
        WinLoseField.text = result.Win ? Namings.Tag("WinEnd") : Namings.Tag("LoseEnd");
        DrawOpens(stat);
        GooBad.color = result.Win ? WinColor : LoseColor;

        base.Init();
    }

    private void DrawOpens(PlayerStatistics stats)
    {
        string openWeapons = "";
        string openConfigs = "";
        bool haveOne = false;
        if (stats.LastWeaponsPairOpen != null)
        {
            haveOne = true;
            openWeapons = Namings.Format(Namings.Tag("OpenWeaponsEndGame"), Namings.Weapon(stats.LastWeaponsPairOpen.Part1), Namings.Weapon(stats.LastWeaponsPairOpen.Part2));
        }
        if (stats.LastOpenShipConfig != null)
        {
            haveOne = true;
            openWeapons = Namings.Format(Namings.Tag("OpenConfigEndGame"), Namings.ShipConfig(stats.LastOpenShipConfig.Config));
        }
        OpenDataContainer.gameObject.SetActive(haveOne);
        if (haveOne)
        {
            OpenDataField.text = $"{openWeapons} {openConfigs}";
        }

    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public void OnToNewGame()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }


}

