using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ArmyCreatorRocket : ArmyCreatorData
{
    protected override List<SimpleModulType> AllSimpleModuls()
    {
        return new List<SimpleModulType>() { SimpleModulType.antiPhysical, SimpleModulType.autoRepair ,SimpleModulType.rocketUpgrade,SimpleModulType.EMIUpgrade};
    }

    protected override List<WeaponType> AllWeaponModuls()
    {
        return new List<WeaponType>() {WeaponType.rocket,WeaponType.eimRocket};
    }

    public ArmyCreatorRocket(ShipConfig config, bool isAi) : base(config, isAi)
    {
    }
}

