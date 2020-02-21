using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//[Serializable]
public class WeaponsPair
{
    public WeaponsPair(WeaponType Part1, WeaponType Part2)
    {
        this.Part1 = Part1;
        this.Part2 = Part2;
    }

    public bool IsOpen;
    public WeaponType Part1;
    public WeaponType Part2;

    public List<WeaponType> GetAsList()
    {
        return new List<WeaponType>() { Part1, Part2 };

    }
}

//[Serializable]
public class OpenShipConfig
{
    public OpenShipConfig(ShipConfig config)
    {
        Config = config;
    }

    public ShipConfig Config;
    public bool IsOpen;
}


[Serializable]
public class PlayerStatistics
{
    public const int OPEN_FOR_END_GAME = 3;
    public const int POINTS_TO_OPEN = 125;
    private static string mainPlayer = $"/stats_{MainController.VERSION}.stat";

    [field: NonSerialized] public List<OpenShipConfig> OpenShipsTypes;

    [field: NonSerialized] public List<WeaponsPair> WeaponsPairs;

    private int _openedWeapons = 0;
    private int _openedConfigs = 0;
    public int CollectedPoints { get; private set; }
    public EndBattleType LastBattle = EndBattleType.win;
    public EndGameStatistics EndGameStatistics = new EndGameStatistics();
    private int _lastDifficulty;

    [field: NonSerialized]
    public WeaponsPair LastWeaponsPairOpen = null;
    [field: NonSerialized]
    public OpenShipConfig LastOpenShipConfig = null;
    public int Wins { get; private set; }
    public int MaxLevelWeapons { get; private set; }
    public int ShipsDestroyed { get; private set; }
    public int MaxLevelSpells { get; private set; }
    public int Damage { get; private set; }
    public int CollectMaxMoney { get; private set; }
    public int CollectMaxLevelShip { get; private set; }
    public bool TeamFive { get; private set; }
    public bool WinNoweapons { get; private set; }


    public void Init()
    {
        OpenShipsTypes = new List<OpenShipConfig>()
        {
            new OpenShipConfig(ShipConfig.mercenary),
            new OpenShipConfig(ShipConfig.raiders),
            new OpenShipConfig(ShipConfig.federation),
            new OpenShipConfig(ShipConfig.ocrons),
            new OpenShipConfig(ShipConfig.krios),
        };
        WeaponsPairs = new List<WeaponsPair>()
        {
            new WeaponsPair(WeaponType.laser, WeaponType.impulse),
            new WeaponsPair(WeaponType.laser, WeaponType.rocket),
            new WeaponsPair(WeaponType.rocket, WeaponType.eimRocket),
            new WeaponsPair(WeaponType.impulse, WeaponType.casset),
            new WeaponsPair(WeaponType.casset, WeaponType.beam),
            new WeaponsPair(WeaponType.eimRocket, WeaponType.beam),
        };
        if (EndGameStatistics == null)
        {
            EndGameStatistics = new EndGameStatistics();
        }
        EndGameStatistics.Init();
        RefreshOpened();
    }

    private void RefreshOpened()
    {
        if (_openedWeapons < 2)
        {
            _openedWeapons = 2;
        }

        if (_openedConfigs < 1)
        {
            _openedConfigs = 1;
        }

        _openedConfigs = _openedConfigs > OpenShipsTypes.Count ? OpenShipsTypes.Count : _openedConfigs;
        _openedWeapons = _openedWeapons > WeaponsPairs.Count ? WeaponsPairs.Count : _openedWeapons;

        for (int i = 0; i < _openedWeapons; i++)
        {
            var weap = WeaponsPairs[i];
            weap.IsOpen = true;
        }
        for (int i = 0; i < _openedConfigs; i++)
        {
            var ships = OpenShipsTypes[i];
            ships.IsOpen = true;
            if (i == _openedConfigs)
            {
                LastOpenShipConfig = ships;
            }
        }
    }

    public bool TryOpenNext()
    {
        if (CanOpenNext())
        {
            CollectedPoints -= POINTS_TO_OPEN;
            OpenNewData();
            return true;
        }

        return false;
    }

    public bool CanOpenNext()
    {
        return CollectedPoints >= POINTS_TO_OPEN;
    }

    public static PlayerStatistics Load()
    {
        PlayerStatistics ps;
        if (LoadGame(out ps))
        {

        }
        else
        {
            ps = new PlayerStatistics();
        }
        ps.Init();
        return ps;
    }

    public void EndBattle(EndBattleType type)
    {
        LastBattle = type;
    }

    private void OpenConfig()
    {
        _openedConfigs++;
        RefreshOpened();
        SaveGame();
    }

    private void OpenWeapon()
    {
        _openedWeapons++;
        RefreshOpened();
        SaveGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        var path = Application.persistentDataPath + mainPlayer;
        FileStream file = File.Create(path);
        bf.Serialize(file, this);
        file.Close();
        Debug.Log($"Stats Saved {path}");
    }


    public static bool LoadGame(out PlayerStatistics stats)
    {
        var path = Application.persistentDataPath + mainPlayer;

        if (File.Exists(path))
        {
            Debug.Log($"TRY Game Loaded {path}");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerStatistics save = (PlayerStatistics)bf.Deserialize(file);
            file.Close();
            stats = save;
            Debug.Log($"Game Loaded {path}");
            return true;
        }
        Debug.Log("No game saved!");
        stats = null;
        return false;
    }

    public void EndGameAll(bool win, Player player)
    {
        if (win)
        {
            AddOpenPoints(_lastDifficulty);
        }
        var mainShip = player.Army.Army.First(x => x.Ship.ShipType == ShipType.Base);
        var finalPower = ArmyCreator.CalcArmyPower(player.Army);
        EndGameResult res = new EndGameResult(win, _lastDifficulty, mainShip.Ship.ShipConfig, player.MapData.GalaxyData.SizeX, DateTime.Now, finalPower);
        EndGameStatistics.AddResult(res);
        SaveGame();
    }

    public void AddOpenPoints(int val)
    {
        CollectedPoints += val;
        SaveGame();
    }

    private void OpenNewData()
    {
        var OpenWeapons = false;
        var OpenConfigs = false;
        if (_openedWeapons == 1 && _openedConfigs == 1)
        {
            OpenWeapons = OpenConfigs = true;
        }
        else
        {
            if (_openedWeapons < _openedConfigs)
            {
                OpenWeapons = true;
            }
            else
            {
                OpenConfigs = true;
            }
        }
        if (OpenWeapons)
        {
            OpenWeapon();
        }
        else
        {
            LastWeaponsPairOpen = null;
        }
        if (OpenConfigs)
        {
            OpenConfig();
        }
        else
        {
            LastOpenShipConfig = null;
        }
        SaveGame();
    }

    public void PlayNewGame(StartNewGameData data)
    {
        var d = data.CalcDifficulty();
        _lastDifficulty = (int)(d * 100);
    }

    public void AddWinFinal(ShipConfig config)
    {

        switch (config)
        {
            case ShipConfig.raiders:
                SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.WIN_AS_RDR);
                break;
            case ShipConfig.federation:
                SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.WIN_AS_FED);
                break;
            case ShipConfig.mercenary:
                SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.WIN_AS_MER);
                break;
            case ShipConfig.ocrons:
                SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.WIN_AS_OCR);
                break;
            case ShipConfig.krios:
                SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.WIN_AS_KRI);
                break;
        }
    }
    public void AddWin(ShipConfig config)
    {

        Wins++;
        if (Wins > 1)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.ACH_WIN_1_BATTLE);
        }
        if (Wins > 10)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.ACH_WIN_10_BATTLE);
        }
        if (Wins > 100)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.ACH_WIN_100_BATTLE);
        }
    }
    public void AddMaxLevelWeapons()
    {
        MaxLevelWeapons++;
    }
    public void AddShipsDestroyed()
    {
        ShipsDestroyed++;
        if (ShipsDestroyed > 100)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.SHIP_DESTROY_100);
        }
        if (ShipsDestroyed > 1000)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.SHIP_DESTROY_1000);
        }
        if (ShipsDestroyed > 10000)
        {
            SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.SHIP_DESTROY_10000);
        }

    }
    public void AddMaxLevelSpells()
    {
        MaxLevelSpells++;
    }
    public void AddDamage(int dmg)
    {
        Damage += dmg;
    }

    public void AddCollectMaxMoney(int money)
    {
        // if (money > CollectMaxMoney)
        // {
        //     CollectMaxMoney = money;
        //     if (CollectMaxMoney > 1000)
        //     {
        //         SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.COLLECT_MONEY_1000);
        //     }
        //     if (CollectMaxMoney > 10000)
        //     {
        //         SteamStatsAndAchievements.Instance.CompleteAchievement(SteamStatsAndAchievements.Achievement.COLLECT_MONEY_10000);
        //     }
        // }

    }
    public void AddCollectMaxLevelShip(int lvl)
    {
        if (lvl > CollectMaxLevelShip)
        {
            CollectMaxLevelShip = lvl;
        }

    }
}

