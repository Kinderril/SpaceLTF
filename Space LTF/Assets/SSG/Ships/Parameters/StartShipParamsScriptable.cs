using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StartShipParamsScriptable  : ScriptableObject , IStartShipParams
{
    public ShipType ShipTypeData;
    public ShipConfig ShipConfigData;
    public float MaxHealthData;
    public float MaxShiledData;
    public float MaxSpeedData;
    public float TurnSpeedData;
    public float ShiledRegenData;
    public float EvationData;
    public int WeaponModulsCountData;
    public int SimpleModulsCountData;
    public int SpellModulsCountData;
    public int BodyVisualTypeData;

    public float MaxHealth
    {
        get { return MaxHealthData; } 
    }

    public float MaxShiled
    {
        get { return MaxShiledData; }
    }
    public float MaxSpeed
    {
        get { return MaxSpeedData; }
    }
    public float TurnSpeed
    {
        get { return TurnSpeedData; }
    }
    public float ShiledRegen
    {
        get { return ShiledRegenData; }
    }

    public float HealthPercent { get; }

    public float Evation
    {
        get { return EvationData; }
    }

    public ShipConfig ShipConfig
    {
        get { return ShipConfigData; }
    }

    public ShipType ShipType
    {
        get { return ShipTypeData; }
    }

    public int WeaponModulsCount
    {
        get { return WeaponModulsCountData; }
    }
    public int SimpleModulsCount
    {
        get { return SimpleModulsCountData; }
    }

    public int SpellModulsCount { get { return SpellModulsCountData; } }
    public string Name { get; }

    public int BodyVisualType { get { return BodyVisualTypeData; } }
}

