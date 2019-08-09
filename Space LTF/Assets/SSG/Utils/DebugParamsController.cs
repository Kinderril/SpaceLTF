using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugParamsController
{
#if UNITY_EDITOR  
    public static bool EngineOff { get; private set; }
    public static bool NoDamage { get; private set; }
    public static bool NoMouseMove { get; private set; }
    public static bool FastRecharge { get; private set; }

    public static void SwitchEngine()
    {
        EngineOff = !EngineOff;
    }
    public static void SwitchNoDamage()
    {
        NoDamage = !NoDamage;
    }    
    public static void SwitchNoMouseMove()
    {
        NoMouseMove = !NoMouseMove;
    }

    public static void SwitchFastRecharge()
    {
        FastRecharge = !FastRecharge;
    }
#endif

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
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer);
        WindowManager.Instance.InfoWindow.Init(null, String.Format("You hired a new pilot. Type:{0}  Config:{1}", Namings.ShipConfig(cng), Namings.ShipType(type)));
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
        MainController.Instance.MainPlayer.TryHireShip(data);
        return data;
    }
}
