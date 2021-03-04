using System.Collections.Generic;
using UnityEngine;

public class DebugParamsController
{
    //#if UNITY_EDITOR

    public static bool EngineOff;
    public static bool NoDamage;
    // public static bool NoMouseMove;
    public static bool FastRecharge;
    public static bool AllModuls = false;
    public static bool AllSpells = false;
    public static bool AnyWay = false;
    public static bool AllTricks = false;

    //#endif

    public DebugParamsController()
    {
        Debug.LogError("DebugParamsController INITED");

    }

    public static bool NoAmyBorn { get; set; }


    public static void TestHire()
    {
        HireAction(MainController.Instance.MainPlayer);
    }

    public static StartShipPilotData HireAction(Player player,int itemsCount = 1)
    {
        var pilot = Library.CreateDebugPilot();
        WDictionary<ShipType> types = new WDictionary<ShipType>(new Dictionary<ShipType, float>()
        {
            {ShipType.Heavy, 2 },
            {ShipType.Light, 2 },
            {ShipType.Middle, 2 },
        });

        var configsD = new Dictionary<ShipConfig, float>();
        configsD.Add(ShipConfig.krios, 3);
        configsD.Add(ShipConfig.raiders, 3);
        configsD.Add(ShipConfig.ocrons, 3);
        configsD.Add(ShipConfig.federation, 3);
        configsD.Add(ShipConfig.mercenary, 3);

        WDictionary<ShipConfig> configs = new WDictionary<ShipConfig>(configsD);

        var type = types.Random();
        var cng = configs.Random();
        var ship = Library.CreateShip(type, cng, player.SafeLinks, pilot);
        WindowManager.Instance.InfoWindow.Init(null, Namings.Format(Namings.Tag("HirePilot"), Namings.ShipConfig(cng), Namings.ShipType(type)));
        var data = new StartShipPilotData(pilot, ship);
        data.Ship.SetRepairPercent(0.1f);
        WeaponType? typeWeapon = null;
        for (int i = 0; i < itemsCount; i++)
        {
            if (data.Ship.GetFreeWeaponSlot(out var inex))
            {
                WeaponInv weapon;
                if (typeWeapon == null)
                {
                    weapon = Library.CreatWeapon(1);
                    typeWeapon = weapon.WeaponType;
                }
                else
                {
                    weapon = Library.CreateWeaponByType(typeWeapon.Value);
                }

                data.Ship.TryAddWeaponModul(weapon, inex);
            }
        }
        data.Ship.SetRepairPercent(1f);
        player.Army.TryHireShip(data);
        return data;
    }
    public static void LevelUpRandom(Player player)
    {
        var army = player.Army.Army.Suffle();
        var points = 1000f;
        foreach (var pilotData in army)
        {
            if (pilotData.Ship.ShipType != ShipType.Base)
            {
                if (ArmyCreator.TryUpgradePilot(new ArmyRemainPoints(points), pilotData.Pilot, new ArmyCreatorLogs()))
                {
                    Debug.Log("LevelUpRandom complete");
                    return;
                }
            }
        }
        Debug.LogError("can't upgrade");

    }
}
