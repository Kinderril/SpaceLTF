using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ArmyCreatorAOE : ArmyCreatorData
{
//    protected override List<SimpleModulType> AllSimpleModuls()
//    {
//        return new List<SimpleModulType>() { SimpleModulType.antiPhysical, SimpleModulType.engineLocker,
//            SimpleModulType.damageMines,SimpleModulType.systemMines,SimpleModulType.bombUpgrade };
//    }

    protected override List<WeaponType> AllWeaponModuls()
    {
        return new List<WeaponType>() {WeaponType.casset,WeaponType.beam};
    }

    public ArmyCreatorAOE(ShipConfig config,bool isAi)
        : base(config, isAi)
    {

    }
}

