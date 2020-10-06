using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PlayerArmy
{
    public const int MAX_ARMY = 6;

    public List<StartShipPilotData> Army
    {
        get
        {
            if (_links == null || _links.Ships == null)
            {
                return null;
            }
            return _links.Ships.Where(x => !x.Ship.IsDead).ToList();
        }
    } 

    private PlayerSafe _links;
    public ShipConfig BaseShipConfig { get; private set; }
    public int Count => Army.Count(x => !x.Ship.IsDead);

    public StartShipPilotData MainShip { get; private set; }

    public PlayerArmy(PlayerSafe links)
    {
        _links = links;
    }

    public void Add(StartShipPilotData ship)
    {
        Army.Add(ship);
    }

    public bool CanAddShip()
    {
        return Army.Count < MAX_ARMY;
    }
    public bool TryHireShip(StartShipPilotData ship)
    {
        if (CanAddShip())
        {
            _links.AddShip(ship);
            return true;
        }
        return false;
    }
    public List<StartShipPilotData> GetShipsToBattle()
    {
        List<StartShipPilotData> list = new List<StartShipPilotData>();
        foreach (var data in Army)
        {
            //            if (!data.Ship.Destroyed)
            //            {
            list.Add(data);
            //            }
        }

        return list;

    }

    public void RemoveShip(ShipInventory shipInventory)
    {
        var shipTo = Army.FirstOrDefault(x => x.Ship == shipInventory);
        if (shipTo == null)
        {
            Debug.LogError("can't find ship to destroy");
            return;
        }

        RemoveShip(shipTo);
    }

    public static string PowerDesc(SectorData cellSector, float armyPower)
    {
        var player = MainController.Instance.MainPlayer;
        var playersPower = ArmyCreator.CalcArmyPower(player.Army);
        return ComparePowers(playersPower, armyPower);
    }

    public static string ComparePowers(float playersPower, float powerToCompare)
    {

        var delta = playersPower / powerToCompare;
        string srt;
        if (delta < 0.95f)
        {
            srt = Namings.Tag("Risky");
        }
        else if (delta > 1.15f)
        {
            srt = Namings.Tag("Easily");
        }
        else
        {

            srt = Namings.Tag("Comparable");
        }

#if UNITY_EDITOR
        srt = $"{srt} {playersPower}/{powerToCompare}";
#endif
        return srt;
    }

    public void RemoveShip(StartShipPilotData shipToDel)
    {
        _links.RemoveShip(shipToDel);
    }

    public void SetArmy(List<StartShipPilotData> createStartArmy)
    {
        MainShip = createStartArmy.FirstOrDefault(x => x.Ship.ShipType == ShipType.Base);
        var first = createStartArmy[0];
        BaseShipConfig = first.Ship.ShipConfig;
        _links.SetArmy(createStartArmy);
    }

    public float GetPower()
    {
        return ArmyCreator.CalcArmyPower(Army);
    }

    public void DebugAddAllExp()
    {
        foreach (var pilotData in Army)
        {
            pilotData.Pilot.Stats.AddExp(111);
        }
    }

    public bool HaveSmtToRepair()
    {
        foreach (var startShipPilotData in Army)
        {
            if (startShipPilotData.Ship.HealthPercent < 0.99999f)
            {
                return true;
            }
        }

        return false;
    }
}

