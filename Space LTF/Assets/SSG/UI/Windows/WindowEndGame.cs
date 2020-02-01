using System;
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
        
        StepsField.text = String.Format(Namings.EndGameDays, player.MapData.Step);
        var result = stat.EndGameStatistics.LastResult;
        DifficultyField.text = String.Format(Namings.StatisticDifficulty, result.Difficulty);
        ConfigField.text = String.Format(Namings.StatisticConfig, result.Config);
        MapSizeField.text = String.Format(Namings.StatisticMapSize, result.MapSize);
        FinalArmyPowerField.text = String.Format(Namings.StatisticFinalArmyPower, result.FinalArmyPower);
        BattleWinsField.text = String.Format(Namings.BattleWinsStat, stat.Wins);
        WinLoseField.text = result.Win ? Namings.WinEnd : Namings.LoseEnd;
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
            openWeapons = String.Format(Namings.OpenWeaponsEndGame, Namings.Weapon(stats.LastWeaponsPairOpen.Part1), Namings.Weapon(stats.LastWeaponsPairOpen.Part2));
        }   
        if (stats.LastOpenShipConfig != null)
        {
            haveOne = true;
            openWeapons = String.Format(Namings.OpenConfigEndGame, Namings.ShipConfig(stats.LastOpenShipConfig.Config));
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

