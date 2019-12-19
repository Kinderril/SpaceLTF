﻿using System;

public class StartShipParams : IStartShipParams
{
    public int BodyVisualTypeData;
//    public float EvationData;
    public float MaxHealthData;
    public float HealthPercentData;
    public float MaxShieldData;
    public float MaxSpeedData;
//    public float ShieldRegenData;
    public ShipType ShipTypeData;
    public ShipConfig ShipConfigData;
    public int WeaponModulsCountData;
    public int SimpleModulsCountData;
    public int SpellModulsCountData;
    public float TurnSpeedData;
    public float BodyArmorData = 0f;
    public float ShieldArmorData = 0f;
    public float ShieldRegenData = 0f;
    public float BoostChargeTimeData = 0f;
    public string NameData;

    public StartShipParams(
        ShipType ShipTypeData,
        ShipConfig ShipConfigData,
        float MaxHealthData,
        float maxShieldData,
        float MaxSpeedData,
        float TurnSpeedData,
        int WeaponModulsCountData,
        int SimpleModulsCountData,
        int SpellModulsCount,
        int BodyVisualTypeData,
        float shieldRegenData,
        float HealthPercentData = 1f
        )
    {
        this.ShieldRegenData = shieldRegenData;
        this.ShipTypeData = ShipTypeData;
        this.ShipConfigData = ShipConfigData;
        this.MaxHealthData = MaxHealthData;
        MaxShieldData = maxShieldData;
        this.MaxSpeedData = MaxSpeedData;
        this.HealthPercentData = HealthPercentData;
        this.TurnSpeedData = TurnSpeedData;
        this.WeaponModulsCountData = WeaponModulsCountData;
        this.SpellModulsCountData = SpellModulsCount;
        this.SimpleModulsCountData = SimpleModulsCountData;
        this.BodyVisualTypeData = BodyVisualTypeData;
        NameData = ShipNames.GetRandom();
        switch (ShipTypeData)
        {
            case ShipType.Light:
                BoostChargeTimeData = 12f;
                break;
            case ShipType.Middle:
                BoostChargeTimeData = 17f;
                break;
            case ShipType.Heavy:
                BoostChargeTimeData = 22f;
                break;
            case ShipType.Base:
                BoostChargeTimeData = 40f;
                break;
        }
//        this.ShieldRegenData = 0;
//        this.EvationData = 0;
    }

//    public float Evation
//    {
//        get { return 0; }
//    }

    public float MaxHealth
    {
        get { return MaxHealthData; }
    }

    public float MaxShiled
    {
        get { return MaxShieldData; }
    }

    public float MaxSpeed
    {
        get { return MaxSpeedData; }
    }

    public float TurnSpeed
    {
        get { return TurnSpeedData; }
    }

    public float ShieldRegen
    {
        get { return ShieldRegenData; }
    }

    public float BodyArmor => BodyArmorData;
    public float ShiledArmor => ShieldArmorData;

    public float HealthPercent
    {
        get { return HealthPercentData; }
    }

    public ShipType ShipType
    {
        get { return ShipTypeData; }
    }
    public ShipConfig ShipConfig
{
        get { return ShipConfigData; }
    }

    public int WeaponModulsCount
    {
        get { return WeaponModulsCountData; }
    }

    public int SimpleModulsCount
    {
        get { return SimpleModulsCountData; }
    }

    public int SpellModulsCount
    {
        get { return SpellModulsCountData; }
    }

    public string Name
    {
        get { return NameData; }
    }

    public float BoostChargeTime => BoostChargeTimeData;

    public int BodyVisualType
    {
        get { return BodyVisualTypeData; }
    }
}