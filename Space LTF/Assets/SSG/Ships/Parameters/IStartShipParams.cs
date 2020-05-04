﻿public enum ShipType
{
    Light,
    Middle,
    Heavy,
    Base,
    Turret,
}

public enum ShipConfig
{
    raiders,
    federation,
    mercenary,
    ocrons,//Толстые
    krios,//Щитовики
    droid,//Мелкие для АОЕ
}

public interface IStartShipParams
{
    float MaxHealth { get; }
    float MaxShiled { get; }
    float MaxSpeed { get; }
    float TurnSpeed { get; }
    float ShieldRegen { get; }
    float BodyArmor { get; }
    float ShiledArmor { get; }
    float HealthPercent { get; }
    ShipType ShipType { get; }
    ShipConfig ShipConfig { get; }
    int WeaponModulsCount { get; }
    int SimpleModulsCount { get; }
    int SpellModulsCount { get; }
    string Name { get; }
    float BoostChargeTime { get; }
    ParameterItem CocpitSlot { get; }
    ParameterItem EngineSlot { get;  }
    ParameterItem WingSlot { get;}

    //    PilotTactic Tactic { get; }
}
