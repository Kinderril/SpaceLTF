using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
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
        return new List<WeaponType>() { Part1 , Part2};

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
    private static string mainPlayer = $"/stats_{MainController.VERSION}.stat";

    [field: NonSerialized]
    public List<OpenShipConfig> OpenShipsTypes = new List<OpenShipConfig>()
        {
            new OpenShipConfig(ShipConfig.mercenary),
            new OpenShipConfig(ShipConfig.raiders),
            new OpenShipConfig(ShipConfig.federation),
            new OpenShipConfig(ShipConfig.ocrons),
            new OpenShipConfig(ShipConfig.krios),
        };

    [field: NonSerialized]
    public List<WeaponsPair> WeaponsPairs = new List<WeaponsPair>()
    {
        new WeaponsPair(WeaponType.laser, WeaponType.impulse),
        new WeaponsPair(WeaponType.laser, WeaponType.rocket),
        new WeaponsPair(WeaponType.rocket, WeaponType.eimRocket),
        new WeaponsPair(WeaponType.impulse, WeaponType.casset),
        new WeaponsPair(WeaponType.casset, WeaponType.beam),
        new WeaponsPair(WeaponType.eimRocket, WeaponType.beam),
    };

    private int _openedWeapons = 0;
    private int _openedConfigs = 0;
    public EndBattleType LastBattle = EndBattleType.win;
    public EndGameStatistics EndGameStatistics = new EndGameStatistics();
    private float _lastDifficulty;

    public WeaponsPair LastWeaponsPairOpen = null;
    public OpenShipConfig LastOpenShipConfig = null;
    public int Wins { get; private set; }

    public void Init()
    {
        if (EndGameStatistics==null)
        {
            EndGameStatistics=new EndGameStatistics();
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

    public void EndGameAll(bool win,Player player)
    {
        if (win)
        {
            OpenNewData();
        }
        var mainShip = player.Army.First(x => x.Ship.ShipType == ShipType.Base);
        var finalPower = ArmyCreator.CalcArmyPower(player.Army);
        EndGameResult res = new EndGameResult(win,_lastDifficulty, mainShip.Ship.ShipConfig, player.MapData.GalaxyData.Size, DateTime.Now, finalPower);
        EndGameStatistics.AddResult(res);
        SaveGame();
    }

    private void OpenNewData()
    {
        var OpenWeapons = false;
        var OpenConfigs = false;
//        var notOpendTypes = OpenShipsTypes.Where(x => !x.IsOpen).ToList();
//        var notOpendWeapons = WeaponsPairs.Where(x => !x.IsOpen).ToList();
//        var notOpendTypesCount = notOpendTypes.Count;
//        var notOpendWeaponsCount = notOpendWeapons.Count;

//        var weaponsOpen = WeaponsPairs.Count - notOpendWeaponsCount;
//        var configsOpen = OpenShipsTypes.Count - notOpendTypesCount;
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
    }

    public void PlayNewGame(StartNewGameData data)
    {
        _lastDifficulty = data.CalcDifficulty();
    }

    public void AddWin()
    {
        Wins++;
    }
}

