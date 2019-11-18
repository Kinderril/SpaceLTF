using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class ArmyRemainPoints
{
    public float Points;

    public ArmyRemainPoints(float p)
    {
        Points = p;
    }
}

public static class ArmyCreator
{

    public static float CalcArmyPower(List<StartShipPilotData> ships)
    {
        var f = 0f;
        foreach (var ship in ships)
        {
            f = f + Library.CalcPower(ship);
        }

        return f;
    }

//    public static List<StartShipPilotData> CreateArmy(float points, ArmyCreationMode mode, ArmyCreatorData data, bool withBase, Player player)
//    {
//        return CreateArmy(points,)
//    }

    public static List<StartShipPilotData> CreateSimpleEnemyArmyDroid(float remainPoints, ArmyCreatorData data,
        Player player)
    {
        ArmyRemainPoints points = new ArmyRemainPoints(remainPoints);
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float pointsOnStart = points.Points;
        var army = CreateShips(points, data, player, pointsOnStart, 0.1f, logs);
        logs.LogToConsole();
        return army;
    }

    public static List<StartShipPilotData> CreateSimpleEnemyArmy(float remainPoints, ArmyCreatorData data,  Player player)
    {
        Debug.Log($"Start create army {remainPoints} . {data}  {player.Name}");
        if (data.ArmyConfig == ShipConfig.droid)
        {
            return CreateSimpleEnemyArmyDroid(remainPoints, data, player);
        }

        ArmyCreatorLogs logger = new ArmyCreatorLogs();
        float pointsOnStart = remainPoints;
        ArmyRemainPoints points = new ArmyRemainPoints(remainPoints);
        if (remainPoints < Library.MIN_WORKING_SHIP)
        {
            remainPoints = Library.MIN_WORKING_SHIP + 1;
        }

        StartShipPilotData baseShip = null;
        if (remainPoints > Library.MIN_POINTS_TO_CREATE_ARMY_WITH_BASESHIP)
        {
            baseShip = CreateBaseShip(points, data.ArmyConfig, player);
            TryAddCastModul(points, baseShip.Ship, data, logger);
            TryAddCastModul(points, baseShip.Ship, data, logger);
        }

        var army = CreateShips(points, data, player, pointsOnStart,0.5f, logger);
        if (baseShip != null)
            army.Add(baseShip);

        int index = 0;
        int upgradeIterations = 100;
        if (army.Count == 0)
        {
            Debug.LogError($"SHIT!!! ARMY IS NULLL!!! {remainPoints}");
            return army;
        }
        while (upgradeIterations > 0 && remainPoints > 0)
        {
            upgradeIterations--;
            var ship = army[index];
            index++;
            if (ship.Ship.ShipType == ShipType.Base)
            {
                continue;
            }
            if (index >= army.Count)
            {
                index = 0;
            }
            WDictionary<LibraryShipUpgradeType> upgrades = new WDictionary<LibraryShipUpgradeType>(
             new Dictionary<LibraryShipUpgradeType, float>()
            {
             {LibraryShipUpgradeType.addModul ,2} ,
             {LibraryShipUpgradeType.addWeapon  ,2} ,
             {LibraryShipUpgradeType.levelUpPilot ,5} ,
             {LibraryShipUpgradeType.upgradeWeapon ,5} ,
             {LibraryShipUpgradeType.upgradeModul ,3} ,
            });
            var rnd = upgrades.Random();
            UpgradeShip(points, rnd, ship, upgrades, data, logger);
        }
        Debug.LogFormat("Simple army create. RemainPoints:{0}", remainPoints);
        logger.LogToConsole();
        return army;
    }

    private static List<StartShipPilotData> CreateShips(ArmyRemainPoints remainPoints, ArmyCreatorData data, 
        Player player,float pointsOnStart,float percentsToEnd, ArmyCreatorLogs logs)
    {
        List<StartShipPilotData> army = new List<StartShipPilotData>();
        int maxRemainShips = 5;
        bool shallUpgrade = false;

        while (!shallUpgrade && maxRemainShips > 0)
        {
            maxRemainShips--;
            var s1 = CreateShipWithWeapons(remainPoints, data, player, logs);
            if (s1 != null)
            {
                army.Add(s1);
                //            points -= Library.CalcPower(s1);
            }
            else
            {
                Debug.LogWarning("can't create ship. Not enought points " + data.ToString());
            }
            var remainPercent = remainPoints.Points / pointsOnStart;
//            Debug.Log($"Ship for army created remainPercents {remainPercent}.  {remainPoints}/{pointsOnStart}");
            if (remainPercent < percentsToEnd)
            {
                shallUpgrade = true;
            }
        }

        return army;
    }

    public static List<StartShipPilotData> CreateArmy(float pointsStart, ArmyCreationMode mode, int countMin, int countMax,ArmyCreatorData data,bool withBase,Player player)
    {
        ArmyCreatorLogs logs = new ArmyCreatorLogs();
        float pointsOnStart = pointsStart;
        ArmyRemainPoints pointsArmy = new ArmyRemainPoints(pointsOnStart);
        List<StartShipPilotData> army = new List<StartShipPilotData>();
        int armyCount;
        if (countMin >= countMax)
        {
            armyCount = Mathf.Min(countMin, countMax);
        }
        else
        {
            var c1 = MyExtensions.Random(countMin, countMax);
            if (c1 >= countMax)
            {
                c1 = countMax;
            }
            armyCount = c1;
        }
        if (withBase)
        {
            var baseShip = CreateBaseShip(pointsArmy, data.ArmyConfig, player);
            TryAddCastModul(pointsArmy, baseShip.Ship, data, logs);
            TryAddCastModul(pointsArmy, baseShip.Ship, data, logs);
            army.Add(baseShip);
        }
        switch (mode)
        {
//            case ArmyCreationMode.allToOne:
//                Debug.LogError("DO not realize");
//                break;
            case ArmyCreationMode.equalize:
                var pointPerShip = pointsArmy.Points / armyCount;
                if (pointPerShip < Library.MIN_WORKING_SHIP)
                {
//                    Debug.LogError("bad count of points recalculation");
                    var dd = pointsArmy.Points / Library.MIN_WORKING_SHIP;
                    if (dd < 1f)
                    {
                        pointsArmy.Points = Library.MIN_WORKING_SHIP + 1;
//                        Debug.LogError("WRONG ARMY ALERT!!!! can't create create stub army");
//                        return army;
                    }
                    armyCount = (int)dd;
                    pointPerShip = pointsArmy.Points / armyCount;
                }

                bool shallStop = false;
                int createdShips = 0;
                while ((pointsArmy.Points > Library.MIN_WORKING_SHIP || !shallStop) && createdShips < 7)
                {
                    createdShips++;
                    var s1 = CreateShipByValue(pointsArmy, data, player, logs);
                    if (s1 != null)
                    {
                        army.Add(s1);
                        pointsArmy.Points -= Library.CalcPower(s1);
//                        points -= pointPerShip;
                    }
                    else
                    {
                        shallStop = true;
                        Debug.LogWarning("can't create ship. Not enought points " + mode.ToString());
                    }
                }


                break;
            case ArmyCreationMode.random:
                var pointPerShip1 = pointsArmy.Points / armyCount;
                if (pointPerShip1 < Library.MIN_WORKING_SHIP)
                {
                    //                    Debug.LogError("bad count of points recalculation");
                    var dd = pointsArmy.Points / Library.MIN_WORKING_SHIP;
                    if (dd < 1f)
                    {
                        Debug.LogError("WRONG ARMY ALERT!!!! can't create create stub army");
                        return army;
                    }
                    armyCount = (int)dd;
//                    pointPerShip = points / armyCount;
                }

                List<float> values = new List<float>();
                for (int i = 0; i < armyCount; i++)
                {
                    var p1 = pointsArmy.Points * 2f / (float)armyCount;
                    var p2 = pointsArmy.Points * (1f - 2f / (float)armyCount);
                    var p = MyExtensions.Random(p2, p1);
                    if (p < Library.MIN_WORKING_SHIP)
                    {
                        p = Library.MIN_WORKING_SHIP;
                    }
                    values.Add(p);
                }
                foreach (var value in values)
                {
//                    var v = value;
                    var s1 = CreateShipByValue(new ArmyRemainPoints(value), data, player, logs);
                    if (s1 != null)
                    {
                        army.Add(s1);
                        pointsArmy.Points -= Library.CalcPower(s1);
                    }
                    else
                    {
                        Debug.LogError("can't create ship. Not enought points " + mode.ToString());
                    }
                }

                if (pointsArmy.Points > Library.MIN_WORKING_SHIP)
                {                     
                    var s1 = CreateShipByValue(pointsArmy, data, player, logs);
                    if (s1 != null)
                    {
                        army.Add(s1);
//                        pointsArmy.Points -= Library.CalcPower(s1);
                    }
                    else
                    {
                        Debug.LogError("can't create ship. Not enought points " + mode.ToString());
                    }
                }
                break;
        }
#if UNITY_EDITOR
        var realPower = CalcArmyPower(army);
        Debug.Log("ARMY CREATE pointsOnStart:" + pointsOnStart + "   Remain:" + pointsArmy.Points
                  + "    CalculatedPower:" 
                  + realPower + "   armyCount:"  + army.Count + "   mode:" + mode.ToString()) ;
        logs.LogToConsole();
        if (pointsArmy.Points > Library.MIN_WORKING_SHIP * 1.1f && armyCount <= countMax)
        {
            Debug.LogError("Army creating is wrong !");
        }

#endif
        return army;
    }

    public static StartShipPilotData CreateBaseShip(ArmyRemainPoints v,ShipConfig config,Player player)
    {
        var pilot = Library.CreateDebugPilot();
        var ship = Library.CreateShip(ShipType.Base, config, player,pilot);
        return new StartShipPilotData(pilot, ship);
    }

    [CanBeNull]
    public static StartShipPilotData CreateShipByConfig(ArmyRemainPoints v, ShipConfig config, Player player, ArmyCreatorLogs logs)
    {

        if (v.Points < Library.BASE_SHIP_VALUE)
        {
            return null;
        }
        var listTyper = new List<ShipType>() { ShipType.Light, ShipType.Heavy, ShipType.Middle };
        var pilot = Library.CreateDebugPilot();
        var ship = Library.CreateShip(listTyper.RandomElement(), config, player,pilot);
        v.Points -= Library.BASE_SHIP_VALUE;
        logs.AddLog(v.Points,"create ship");
        var startData = new StartShipPilotData(pilot, ship);
        return startData;
    }

    private const int MAX_ITERATIONS = 15;

    [CanBeNull]
    public static StartShipPilotData CreateShipWithWeapons(ArmyRemainPoints remainPoints, ArmyCreatorData data, Player player, ArmyCreatorLogs logs)
    {
        if (remainPoints.Points < Library.MIN_WORKING_SHIP)
        {
            Debug.LogWarning("this is not enought to build ship");
        }
        var shipWithWeapons = CreateShipByConfig(remainPoints, data.ArmyConfig, player, logs);
        if (shipWithWeapons == null)
        {
            Debug.LogWarning($"Can't create ship by config {data.ArmyConfig} " );
            return null;
        }
        var rndWeapon = data.GetWeaponType();
        var weaponSlots = shipWithWeapons.Ship.WeaponsModuls.Length;
        var moreHalf = 1 + (int) (weaponSlots / 2f);
        moreHalf = Mathf.Clamp(moreHalf, 1, weaponSlots);
        bool weaponCreated = false;
        for (int i = 0; i < moreHalf; i++)
        {
            if (TryAddWeapon(remainPoints, shipWithWeapons.Ship, rndWeapon, true, logs))
            {
                weaponCreated = true;
            }
        }

        if (!weaponCreated)
        {
            Debug.LogError($"Create ship without weapons count:{moreHalf}");
        }
        return shipWithWeapons;
    }
    [CanBeNull]
    public static StartShipPilotData CreateShipByValue(ArmyRemainPoints v,ArmyCreatorData data, Player player, ArmyCreatorLogs logs)
    {
        if (v.Points < Library.MIN_WORKING_SHIP)
        {
            Debug.LogWarning("this is not enought to build ship");
        }

        var startData = CreateShipByConfig(v, data.ArmyConfig, player, logs);
        if (startData == null)
        {
            return null;
        }
        int maxIterations = MAX_ITERATIONS;
        while (maxIterations > 0)
        {
            bool anyway = false;
            bool isWorks;
            LibraryShipUpgradeType shipUpgradeType;
            WDictionary<LibraryShipUpgradeType> chances = null;

            var chancesInner = new Dictionary<LibraryShipUpgradeType, float>();
            if (startData.Ship.WeaponsModuls.All(x => x == null))
            {
                chancesInner.Add(LibraryShipUpgradeType.addWeapon, 3f);
            }
            else
            {
                chancesInner.Add(LibraryShipUpgradeType.addModul, 2f);
                chancesInner.Add(LibraryShipUpgradeType.addWeapon, 3f);
                chancesInner.Add(LibraryShipUpgradeType.levelUpPilot, 3f);
            }

            if (startData.Ship.WeaponsModuls.Any(x => x != null))
            {
                chancesInner.Add(LibraryShipUpgradeType.upgradeWeapon, 2f);
            }
            if (startData.Ship.Moduls.SimpleModuls.Any(x => x != null))
            {
                chancesInner.Add(LibraryShipUpgradeType.upgradeModul, 2f);
            }   
            chances = new WDictionary<LibraryShipUpgradeType>(chancesInner);
            //                isWorks = false;
            shipUpgradeType = chances.Random();
            maxIterations--;
            isWorks = UpgradeShip(v, shipUpgradeType, startData, chances, data, logs);
        }
        return startData;
    }

    public static bool UpgradeShip(ArmyRemainPoints v,LibraryShipUpgradeType shipUpgradeType, StartShipPilotData startData,
        WDictionary<LibraryShipUpgradeType> chances,ArmyCreatorData data, ArmyCreatorLogs logs)
    {
        bool isWorks = false;
        switch (shipUpgradeType)
        {
            case LibraryShipUpgradeType.addWeapon:
                WeaponType rndWeapon;
                if (startData.Ship.WeaponsModuls.Length > 0)
                {
                    try
                    {
                        rndWeapon = startData.Ship.WeaponsModuls[0].WeaponType;
                    }
                    catch (Exception e)
                    {
                        string dataInfoDEbug = "";
                        foreach (var shipWeaponsModul in startData.Ship.WeaponsModuls)
                        {
                            var weap = (shipWeaponsModul == null) ? "null" : "weap";
                            dataInfoDEbug += "  " + weap;
                        }
                        Debug.LogError($"UpgradeShip no weapon Length:{startData.Ship.WeaponsModuls.Length}  ShipType:{startData.Ship.ShipType}   dataInfoDEbug:{dataInfoDEbug}");

                        rndWeapon = data.GetWeaponType();
                    }
                }
                else
                {
                    rndWeapon = data.GetWeaponType();
                }
                isWorks = TryAddWeapon(v, startData.Ship, rndWeapon, false, logs);
                if (!isWorks)
                {
                    if (chances != null)
                        chances.Remove(shipUpgradeType);
                }

                break;
            case LibraryShipUpgradeType.addModul:
                isWorks = TryAddModul(v, startData.Ship, data, logs);
                if (!isWorks)
                {
                    if (chances != null)
                        chances.Remove(shipUpgradeType);
                }
                break;
            case LibraryShipUpgradeType.upgradeModul:
                isWorks = TryUpgradeModul(v, startData.Ship,logs);
                if (!isWorks)
                {
                    if (chances != null)
                        chances.Remove(shipUpgradeType);
                }
                break;
            case LibraryShipUpgradeType.upgradeWeapon:
                isWorks = TryUpgradeWeapon(v, startData.Ship, logs);
                if (!isWorks)
                {
                    if (chances != null)
                        chances.Remove(shipUpgradeType);
                }
                break;
            case LibraryShipUpgradeType.levelUpPilot:
                isWorks = TryUpgradePilot(v, startData.Pilot, logs);
                if (!isWorks)
                {
                    if (chances != null)
                        chances.Remove(shipUpgradeType);
                }
                break;
        }

        return isWorks;
    }

    public static List<LibraryPilotUpgradeType> PosiblePilotUpgrades(IPilotParameters pilot)
    {

        List<LibraryPilotUpgradeType> list = new List<LibraryPilotUpgradeType>();
        if (pilot.CanUpgradeByLevel(pilot.CurLevel))
        {
            list.Add(LibraryPilotUpgradeType.health);
        }
        if (pilot.CanUpgradeByLevel(pilot.CurLevel))
        {
            list.Add(LibraryPilotUpgradeType.shield);
        }
        if (pilot.CanUpgradeByLevel(pilot.CurLevel))
        {
            list.Add(LibraryPilotUpgradeType.speed);
        }
        if (pilot.CanUpgradeByLevel(pilot.CurLevel))
        {
            list.Add(LibraryPilotUpgradeType.turnSpeed);
        }
        return list;
    }

    public static bool TryUpgradePilot(ArmyRemainPoints v, IPilotParameters pilot, ArmyCreatorLogs logs)
    {
        var d = Library.PILOT_LEVEL_COEF ; //Cost of point
        if (v.Points >= d)
        {
            List<LibraryPilotUpgradeType> list = PosiblePilotUpgrades(pilot);
            if (list.Count == 0)
            {
                return false;
            }
            v.Points -= d;
            logs.AddLog(v.Points,"upgrade pilot");
            var rnd = list.RandomElement();
            pilot.UpgradeLevelByType(rnd,false);
            return true;
        }
        return false;
    }

    private static bool TryUpgradeWeapon(ArmyRemainPoints v, ShipInventory ship, ArmyCreatorLogs logs)
    {
        var val = Library.WEAPON_LEVEL_COEF;//* Library.ShipPowerCoef(ship.ShipType);
        if (v.Points >= val)
        {
            var rndWeapons = ship.WeaponsModuls.Where(x => x != null && x.Level < Library.MAX_WEAPON_LVL).ToList();
            if (rndWeapons.Count > 0)
            {
                v.Points -= val;
                logs.AddLog(v.Points,"upgrade weapon");
                var rndWeapon = rndWeapons.RandomElement();
                rndWeapon.Upgrade();
                return true;
            }
        }
        return false;
    }

    private static bool TryUpgradeModul(ArmyRemainPoints v, ShipInventory ship, ArmyCreatorLogs logs)
    {
        var val = Library.BASE_SIMPLE_MODUL_VALUE_UPGRADE;//* Library.ShipPowerCoef(ship.ShipType);
        if (v.Points >= val)
        {
            var rndModuls = ship.Moduls.SimpleModuls.Where(x => x != null && x.CanUpgradeLevel()).ToList();
            if (rndModuls.Count > 0)
            {
                v.Points -= val;
                var rndModul = rndModuls.RandomElement();
                rndModul.Upgrade();
                logs.AddLog(v.Points,"upgrade modul");
                return true;
            }
        }
        return false;
    }

//    private static bool TryUpgradeSimple(ref float v, ShipInventory ship)
//    {
//        var val = Library.MODUL_LEVEL_COEF * Library.ShipCoef(ship.ShipType);
//        if (v >= val)
//        {
//            var rndSimples = ship.SimpleModuls.Where(x => x != null && x.Level < MAX_WEAPON_LVL).ToList();
//            if (rndSimples.Count > 0)
//            {
//                v -= val;
//                var rnd = rndSimples.RandomElement();
//                rnd.Upgrade();
//                return true;
//            }
//        }
//        return false;
//    }

    public static bool TryAddWeapon(ArmyRemainPoints v, ShipInventory ship, WeaponType weapon,bool anyway, ArmyCreatorLogs logs)
    {
        int weaponIndex;
        var val = Library.BASE_WEAPON_VALUE;// * Library.ShipPowerCoef(ship.ShipType);
        if (v.Points >= val || anyway)
        {
            if (ship.GetFreeWeaponSlot(out weaponIndex))
            {
                WeaponInv a1 = Library.CreateWeapon(weapon);
                ship.TryAddWeaponModul(a1, weaponIndex);
                v.Points -= val;
                logs.AddLog(v.Points,"add weapon");
                return true;
            }
        }
        return false;
    }

    public static bool TryAddModul(ArmyRemainPoints v, ShipInventory ship,ArmyCreatorData listSimple, ArmyCreatorLogs logs)
    {
        var lvl = 1;
        var val = Library.BASE_SIMPLE_MODUL_VALUE;// * Library.ShipPowerCoef(ship.ShipType);
        if (v.Points >= val && ship.GetFreeSimpleSlot(out var simpleIndex))
        {
            v.Points -= val;
            logs.AddLog(v.Points,$"add modul. {lvl}");
            var e = listSimple.GetSimpleType();
            listSimple.RemoveSimple(e);

            var m1 = Library.CreatSimpleModul(e, lvl);
            ship.TryAddSimpleModul(m1, simpleIndex);
            return true;
        }
        return false;
    }

    public static bool TryAddCastModul(ArmyRemainPoints v, ShipInventory ship,ArmyCreatorData listSimple, ArmyCreatorLogs logs)
    {
        int simpleIndex;
        var val = Library.BASE_SPELL_VALUE;// * Library.ShipPowerCoef(ship.ShipType);
        if (v.Points >= val && ship.GetFreeSpellSlot(out simpleIndex))
        {
            v.Points -= val;
            logs.AddLog(v.Points,"add cast modul");
            var e = listSimple.GetSpellType();
            listSimple.RemoveSpell(e);
            var m1 = Library.CreateSpell(e);
            ship.TryAddSpellModul(m1, simpleIndex);
            return true;
        }
        return false;
    }

}

