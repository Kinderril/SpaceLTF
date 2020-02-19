using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugParamsController
{
    //#if UNITY_EDITOR

    public static bool EngineOff;
    public static bool NoDamage;
    public static bool NoMouseMove;
    public static bool FastRecharge;
    public static bool AllModuls = false;
    public static bool AnyWay = false;

    //#endif

    public DebugParamsController()
    {
        Debug.LogError("DebugParamsController INITED");

    }

    public static void TestHire()
    {
        HireAction();
    }

    protected static StartShipPilotData HireAction(int itemsCount = 1)
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
        configsD.Add(ShipConfig.raiders, 5);
        configsD.Add(ShipConfig.ocrons, 3);
        configsD.Add(ShipConfig.federation, 2);
        configsD.Add(ShipConfig.mercenary, 5);

        WDictionary<ShipConfig> configs = new WDictionary<ShipConfig>(configsD);

        var type = types.Random();
        var cng = configs.Random();
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer, pilot);
        WindowManager.Instance.InfoWindow.Init(null, Namings.Format("You hired a new pilot. Type:{0}  Config:{1}", Namings.ShipConfig(cng), Namings.ShipType(type)));
        var data = new StartShipPilotData(pilot, ship);
        data.Ship.SetRepairPercent(0.1f);
        for (int i = 0; i < itemsCount; i++)
        {
            if (data.Ship.GetFreeWeaponSlot(out var inex))
            {
                var weapon = Library.CreateWeapon(true);
                data.Ship.TryAddWeaponModul(weapon, inex);
            }
        }
        MainController.Instance.MainPlayer.Army.TryHireShip(data);
        return data;
    }
}
