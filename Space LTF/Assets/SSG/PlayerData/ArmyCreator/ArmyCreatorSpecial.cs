using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ArmyCreatorSpecial
{
    public static List<StartShipPilotData> CreateBossEMIFireMerc(float points)
    {
        return CreateBoss(points, ShipConfig.mercenary, FireEMI);
    }

    public static List<StartShipPilotData> CreateBossCassetSprayMerc(float points)
    {
        return CreateBoss(points, ShipConfig.mercenary, CassetSpray);
    }

    public static List<StartShipPilotData> CreateBossLightMinesRaiders(float points)
    {
        return CreateBoss(points, ShipConfig.raiders, LightMines);
    }   
    public static List<StartShipPilotData> CreateBossImpulseCritsFed(float points)
    {
        return CreateBoss(points, ShipConfig.federation, ImpulseCrits);
    }  
    public static List<StartShipPilotData> CreateBossIgnoreShieldKrios(float points)
    {
        return CreateBoss(points, ShipConfig.krios, IgnoreShield);
    } 
    public static List<StartShipPilotData> CreateBossRocketTurnDistKrios(float points)
    {
        return CreateBoss(points, ShipConfig.krios, RocketTurnDist);
    } 
    public static List<StartShipPilotData> CreateBossHeavyWithSpeedOcrons(float points)
    {
        return CreateBoss(points, ShipConfig.ocrons, HeavyWithSpeed);
    } 
    public static List<StartShipPilotData> CreateBossBeamDistRaiders(float points)
    {
        return CreateBoss(points, ShipConfig.raiders, BeamDist);
    } 
    public static List<StartShipPilotData> CreateBossManyEMIMerc(float points)
    {
        return CreateBoss(points, ShipConfig.mercenary, ManyEMI);
    }
    public static List<StartShipPilotData> CreateBossEngineLockersFederation(float points)
    {
        return CreateBoss(points, ShipConfig.federation, EngineLockers);
    }
    public static List<StartShipPilotData> CreateBossMaxSelfDamageOcrons(float points)
    {
        return CreateBoss(points, ShipConfig.ocrons, MaxSelfDamage);
    }
                                                             

    private static List<StartShipPilotData> FireEMI(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponFire}, WeaponType.eimRocket,
            new List<ShipType>() {ShipType.Middle, ShipType.Light}, 3, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> CassetSpray(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponSpray}, WeaponType.casset,
            new List<ShipType>() {ShipType.Middle, ShipType.Heavy}, 3, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> LightMines(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>()
                {SimpleModulType.fireMines, SimpleModulType.damageMines, SimpleModulType.systemMines},
            WeaponType.impulse,
            new List<ShipType>() {ShipType.Light}, 4, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> ImpulseCrits(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponCrit}, WeaponType.impulse,
            new List<ShipType>() {ShipType.Light, ShipType.Middle, ShipType.Heavy}, 3, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> IgnoreShield(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponShieldIgnore}, WeaponType.rocket,
            new List<ShipType>() {ShipType.Light, ShipType.Middle, ShipType.Heavy}, 5, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> RocketTurnDist(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponDist, SimpleModulType.WeaponSpeed}, WeaponType.rocket,
            new List<ShipType>() {ShipType.Middle, ShipType.Heavy}, 4, player, points);
        return subArmy;
    }

    private static List<StartShipPilotData> HeavyWithSpeed(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.ShipSpeed, SimpleModulType.ShipTurnSpeed}, WeaponType.laser,
            new List<ShipType>() {ShipType.Heavy}, 2, player, points);
        return subArmy;
    }
    private static List<StartShipPilotData> BeamDist(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponDist}, WeaponType.beam,
            new List<ShipType>() {ShipType.Middle}, 3, player, points);
        return subArmy;
    }  
    private static List<StartShipPilotData> ManyEMI(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponShootPerTime}, WeaponType.eimRocket,
            new List<ShipType>() { ShipType.Heavy,ShipType.Middle}, 6, player, points);
        return subArmy;
    }    
    private static List<StartShipPilotData> EngineLockers(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.engineLocker}, WeaponType.casset,
            new List<ShipType>() { ShipType.Heavy,ShipType.Middle}, 4, player, points);
        return subArmy;
    }  
    private static List<StartShipPilotData> MaxSelfDamage(ShipConfig config, Player player, float points)
    {
        var subArmy = CreateArmy(config,
            new List<SimpleModulType>() {SimpleModulType.WeaponSelfDamage,SimpleModulType.WeaponPowerShot}, WeaponType.laser,
            new List<ShipType>() { ShipType.Heavy,ShipType.Middle}, 2, player, points);
        return subArmy;
    }


    #region METHODS

    private static List<StartShipPilotData> CreateBoss(float points, ShipConfig config,
        Func<ShipConfig, Player, float, List<StartShipPilotData>> getArmy)
    {
        var bossLogger = new ArmyCreatorLogs();
        List<StartShipPilotData> list = new List<StartShipPilotData>();
        var remainPoints = points;
        var player = new Player("Boss1");

        var pilot = Library.CreateDebugPilot();
        var shipMain = Library.CreateShip(ShipType.Base, config, player, pilot);

        var listOfSpells = ArmyCreatorData.AllSpellsStatic();
        ArmyCreator.TryAddCastModul(new ArmyRemainPoints(points), shipMain, listOfSpells, bossLogger);
        ArmyCreator.TryAddCastModul(new ArmyRemainPoints(points), shipMain, listOfSpells, bossLogger);
        var shipMainStartData = new StartShipPilotData(pilot, shipMain);
        list.Add(shipMainStartData);

        var subArmy = getArmy(config, player, points);

        foreach (var startShipPilotData in subArmy)
        {
            list.Add(startShipPilotData);
        }

        if (remainPoints > 0)
        {
            UpgradeArmy(list, remainPoints);
        }

        return list;
    }

    private static List<StartShipPilotData> CreateArmy(ShipConfig config, List<SimpleModulType> moduls,
        WeaponType weapons, List<ShipType> shipTypes, int count, Player player, float points)
    {
        var remainPoints = points;
        List<StartShipPilotData> list = new List<StartShipPilotData>();
        if (shipTypes.Count == 0)
        {
            shipTypes.Add(ShipType.Middle);
        }

        for (int i = 0; i < count && remainPoints > 0; i++)
        {
            var dt1 = AddShipWithWeapons(shipTypes.RandomElement(), config, player, weapons);
            foreach (var modulType in moduls)
            {
                TryAddModul(dt1.Ship, modulType);
            }

            var power1 = Library.CalcPower(dt1);
            if (remainPoints > power1 || i == 0)
            {
                remainPoints -= power1;
                list.Add(dt1);
            }
            else
            {
                break;
            }
        }

        return list;
    }

    private static void UpgradeArmy(List<StartShipPilotData> army, float remainPoints)
    {
        var armyData = new ArmyCreatorData(army[0].Ship.ShipConfig, true);
        List<StartShipPilotData> armyCopy = army.ToList();
        var upgrades = new Dictionary<StartShipPilotData, WDictionary<LibraryShipUpgradeType>>();
        foreach (var startShipPilotData in army)
        {
            WDictionary<LibraryShipUpgradeType> chances = new WDictionary<LibraryShipUpgradeType>(
                new Dictionary<LibraryShipUpgradeType, float>()
                {
                    {LibraryShipUpgradeType.levelUpPilot, 1f},
                    {LibraryShipUpgradeType.upgradeModul, 1f},
                    {LibraryShipUpgradeType.upgradeWeapon, 1f},
                });
            upgrades.Add(startShipPilotData, chances);
        }

        var logs = new ArmyCreatorLogs();
        var points = new ArmyRemainPoints(remainPoints);
        int maxUpgrades = 100;
        while (points.Points > 0 && armyCopy.Count > 0 && maxUpgrades > 0)
        {
            maxUpgrades--;
            var objectToUpgrade = armyCopy.RandomElement();
            var upg = upgrades[objectToUpgrade];

            if (upg.Count == 0)
            {
                armyCopy.Remove(objectToUpgrade);
            }
            else
            {
                var upgradfeType = upg.Random();
                var isUpgradeComplete =
                    ArmyCreator.UpgradeShip(points, upgradfeType, objectToUpgrade, upg, armyData, logs);
                if (!isUpgradeComplete)
                {
                }
            }
        }
    }

    private static StartShipPilotData AddShipWithWeapons(ShipType type, ShipConfig config, Player player,
        WeaponType weaponType)
    {
        var pilotH = Library.CreateDebugPilot();
        var shipHeavy = Library.CreateShip(type, config, player, pilotH);
        var shipMainStartDataH = new StartShipPilotData(pilotH, shipHeavy);
        var weaponSlots = shipHeavy.WeaponsModuls.Length;
        for (int i = 0; i < weaponSlots; i++)
        {
            TryAddWeapon(shipHeavy, weaponType);
        }

        return shipMainStartDataH;
    }

    private static bool TryAddWeapon(ShipInventory ship, WeaponType weapon)
    {
        if (ship.GetFreeWeaponSlot(out int weaponIndex))
        {
            WeaponInv a1 = Library.CreateWeapon(weapon);
            ship.TryAddWeaponModul(a1, weaponIndex);
            return true;
        }

        return false;
    }

    private static bool TryAddModul(ShipInventory ship, SimpleModulType weapon)
    {
        if (ship.GetFreeSimpleSlot(out int weaponIndex))
        {
            var a1 = Library.CreatSimpleModul(weapon, 1);
            ship.TryAddSimpleModul(a1, weaponIndex);
            return true;
        }

        return false;
    }

    #endregion
}