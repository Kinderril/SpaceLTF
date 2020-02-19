using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PlayerArmy
{
    public const int MAX_ARMY = 6;
    public List<StartShipPilotData> Army = new List<StartShipPilotData>();
    [field: NonSerialized]
    public event Action<StartShipPilotData, bool> OnAddShip;

    public ShipConfig BaseShipConfig { get; private set; }
    public int Count => Army.Count;

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
            Army.Add(ship);
            if (OnAddShip != null)
            {
                OnAddShip(ship, true);
            }
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

    public static string PowerDesc(SectorData cellSector, float armyPower, int additionalPower)
    {
        var player = MainController.Instance.MainPlayer;
        var playersPower = ArmyCreator.CalcArmyPower(player.Army);

        var isSameSecto = cellSector.Id == player.MapData.CurrentCell.SectorId;
        float powerToCompare;
        if (isSameSecto)
        {
            powerToCompare = armyPower;
            //            Debug.LogError($"power2  :{powerToCompare} ");
        }
        else
        {
            powerToCompare = SectorData.CalcCellPower(player.MapData.VisitedSectors + 1, cellSector.Size,
                cellSector.StartPowerGalaxy, additionalPower);
            //            Debug.LogError($"power1  :{powerToCompare}");
        }

        return ComparePowers(playersPower, powerToCompare);
    }

    public static string ComparePowers(float playersPower, float powerToCompare)
    {

        var delta = playersPower / powerToCompare;
        string srt;
        if (delta < 0.95f)
        {
            srt = Namings.Tag("Risky");
        }else if (delta > 1.15f)
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

        Army.Remove(shipToDel);
        if (OnAddShip != null)
        {
            OnAddShip(shipToDel, false);
        }

    }

    public void SetArmy(List<StartShipPilotData> createStartArmy)
    {
        var first = createStartArmy[0];
        BaseShipConfig = first.Ship.ShipConfig;
        Army = createStartArmy;
    }

    public float GetPower()
    {
        return ArmyCreator.CalcArmyPower(Army);
    }
}

