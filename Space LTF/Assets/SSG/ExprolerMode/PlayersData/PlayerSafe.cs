using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSafe
{
    [field: NonSerialized]
    public event Action<StartShipPilotData, bool> OnAddShip;   
    [field: NonSerialized]
    public event Action<int> OnCreditsChange;    
    [field: NonSerialized]
    public event Action<int> OnMicroChipsChange;

    public List<StartShipPilotData> Ships;
    public PlayerInventory Inventory;
    public int Credits;
    public int Microchips;
    public string Name;
    public Dictionary<PlayerParameterType, int> StartParametersLevels = new Dictionary<PlayerParameterType, int>()
    {
        { PlayerParameterType.chargesCount,1 },
        { PlayerParameterType.chargesSpeed,1 },
        { PlayerParameterType.engineParameter,1 },
        { PlayerParameterType.repair,1 },
        { PlayerParameterType.scout,1 },
    };

    public PlayerSafe()
    {
        Inventory = new PlayerInventory(this);
    }

    public void Save()
    {

    }

    public void SetArmy(List<StartShipPilotData> createStartArmy)
    {
        Ships = createStartArmy;
    }

    public void SetMoney(int moneyCount)
    {
        Credits = moneyCount;
        OnCreditsChange?.Invoke(Credits);
    }

    public void SetMicrochips(int microchipsCount)
    {
        Microchips = microchipsCount;
        OnMicroChipsChange?.Invoke(Microchips);
    }

    public void CreateNew(ShipConfig shipConfig, EStartPair startPair, string nameFieldText)
    {
        List < StartShipPilotData > army = new List<StartShipPilotData>();
        ShipInventory ship1 = null;
        ShipInventory ship2 = null;
        PilotParameters pilot1 = Library.CreateDebugPilot();
        PilotParameters pilot2 = Library.CreateDebugPilot();
        PilotParameters pilot3 = Library.CreateDebugPilot();
        switch (startPair)
        {
            case EStartPair.MidLight:
                ship1 = Library.CreateShip(ShipType.Middle, shipConfig, this, pilot1);
                ship2 = Library.CreateShip(ShipType.Light, shipConfig, this, pilot2);
                break;
            case EStartPair.MidHvy:
                ship1 = Library.CreateShip(ShipType.Middle, shipConfig, this, pilot1);
                ship2 = Library.CreateShip(ShipType.Heavy, shipConfig, this, pilot2);
                break;
            case EStartPair.LightHvy:
                ship1 = Library.CreateShip(ShipType.Heavy, shipConfig, this, pilot1);
                ship2 = Library.CreateShip(ShipType.Light, shipConfig, this, pilot2);
                break;
        }                  

        Name = nameFieldText;




        var shipData1 = new StartShipPilotData(pilot1,ship1);
        var shipData2 = new StartShipPilotData(pilot2,ship2);
        var baseShip = Library.CreateShip(ShipType.Base, shipConfig, this, pilot3);
        var bShip = new StartShipPilotData(pilot3, baseShip);

        float r = 1000;
        army.Add(bShip);
        army.Add(shipData1);
        army.Add(shipData2);

        StartNewGameData.NewGameAddSpellsRandom(bShip);
        List<WeaponType> posibleStartWeapons = new List<WeaponType>(){WeaponType.laser,WeaponType.rocket,WeaponType.casset,WeaponType.impulse,WeaponType.eimRocket};
        StartNewGameData.AddWeaponsToShips(ref r, shipData1, posibleStartWeapons);
        StartNewGameData.AddWeaponsToShips(ref r, shipData2, posibleStartWeapons);

        shipData1.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);
        shipData2.Ship.TryAddSimpleModul(Library.CreatSimpleModul(1, 2), 0);



        Ships = army;

    }



    public bool HaveMoney(int buyPrice)
    {
        return Credits >= buyPrice;
    }

    public void AddMoney(int sellValue)
    {
        SetMoney(Credits + sellValue);
    }

    public void RemoveMicrochips(int microchipsElement)
    {

        Microchips = Microchips - microchipsElement;
    }

    public void RemoveMoney(int cost)
    {
        SetMoney(Credits - cost);
    }

    public bool HaveMicrochips(int microchipsElement)
    {
        return microchipsElement >= Microchips;
    }

    public void RemoveShip(StartShipPilotData shipToDel)
    {
        Ships.Remove(shipToDel);
        OnAddShip?.Invoke(shipToDel, false);
    }

    public void AddShip(StartShipPilotData ship)
    {
        Ships.Remove(ship);
        OnAddShip?.Invoke(ship, true);
    }
}
