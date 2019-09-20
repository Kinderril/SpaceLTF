﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
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

[Serializable]
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
    private const string mainPlayer = "/stats.stat";
    public List<OpenShipConfig> OpenShipsTypes = new List<OpenShipConfig>();
    public List<WeaponsPair> WeaponsPairs = new List<WeaponsPair>();
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
        if (OpenShipsTypes.Count == 0)
        {
            var sc1 = new OpenShipConfig(ShipConfig.raiders);
            var sc2 = new OpenShipConfig(ShipConfig.mercenary);
            var sc3 = new OpenShipConfig(ShipConfig.federation);
            var sc4 = new OpenShipConfig(ShipConfig.ocrons);
            var sc5 = new OpenShipConfig(ShipConfig.krios);
            sc2.IsOpen = true;
//#if UNITY_EDITOR
//            sc1.IsOpen = sc2.IsOpen = sc3.IsOpen = sc4.IsOpen = sc5.IsOpen = true;
//#endif
            OpenShipsTypes.Add(sc1);
            OpenShipsTypes.Add(sc2);
            OpenShipsTypes.Add(sc3);
            OpenShipsTypes.Add(sc4);
            OpenShipsTypes.Add(sc5);
        }
        if (WeaponsPairs.Count == 0)
        {
            var p = new WeaponsPair(WeaponType.laser, WeaponType.rocket);
            p.IsOpen = true;
            WeaponsPairs.Add(p);
            var p1 = new WeaponsPair(WeaponType.laser, WeaponType.eimRocket);
            WeaponsPairs.Add(p1);
            var p2 = new WeaponsPair(WeaponType.casset, WeaponType.rocket);
            WeaponsPairs.Add(p2);
            var p3 = new WeaponsPair(WeaponType.eimRocket, WeaponType.impulse);
            WeaponsPairs.Add(p3);
            var p4 = new WeaponsPair(WeaponType.casset, WeaponType.impulse);
            WeaponsPairs.Add(p4);
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

        foreach (var openShipConfig in OpenShipsTypes)
        {
            if (!openShipConfig.IsOpen)
            {
                LastOpenShipConfig = openShipConfig;
                openShipConfig.IsOpen = true;
                break;
            }
        }
        SaveGame();
    }

    private void OpenWeapon()
    {
        foreach (var weaponsPair in WeaponsPairs)
        {
            if (!weaponsPair.IsOpen)
            {
                LastWeaponsPairOpen = weaponsPair;
                weaponsPair.IsOpen = true;
                break;
            }
        }
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
        var notOpendTypes = OpenShipsTypes.Where(x => !x.IsOpen).ToList();
        var notOpendWeapons = WeaponsPairs.Where(x => !x.IsOpen).ToList();
        var notOpendTypesCount = notOpendTypes.Count;
        var notOpendWeaponsCount = notOpendWeapons.Count;

        var weaponsOpen = WeaponsPairs.Count - notOpendWeaponsCount;
        var configsOpen = OpenShipsTypes.Count - notOpendTypesCount;
        if (weaponsOpen == 1 && configsOpen == 1)
        {
            OpenWeapons = OpenConfigs = true;
        }
        else
        {
            if (weaponsOpen < configsOpen)
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

