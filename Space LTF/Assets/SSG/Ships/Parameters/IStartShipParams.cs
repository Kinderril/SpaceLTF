using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ShipType
{
    Light,
    Middle,
    Heavy,
    Base,
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
    float ShiledRegen { get; }
    float HealthPercent { get; }
    ShipType ShipType { get; }
    ShipConfig ShipConfig { get; }
    int WeaponModulsCount { get; }
    int SimpleModulsCount { get; }
    int SpellModulsCount { get; }
    string Name { get;}
    //    int BodyVisualType { get; }
}
