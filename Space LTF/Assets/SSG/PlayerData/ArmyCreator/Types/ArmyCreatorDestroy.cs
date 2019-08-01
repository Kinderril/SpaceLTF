using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ArmyCreatorDestroy : ArmyCreatorData
{
//    protected override List<SimpleModulType> AllSimpleModuls()
//    {
//        return new List<SimpleModulType>()
//        {
//            SimpleModulType.antiPhysical, SimpleModulType.antiEnergy ,SimpleModulType.closeStrike
//        ,SimpleModulType.rocketUpgrade
//        };
//    }

    protected override List<WeaponType> AllWeaponModuls()
    {
        return new List<WeaponType>() {WeaponType.rocket};
    }

    public ArmyCreatorDestroy(ShipConfig config, bool isAi) : base(config, isAi)
    {
    }
}

