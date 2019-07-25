using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ArmyCreatorLaser : ArmyCreatorData
{
    protected override List<SimpleModulType> AllSimpleModuls()
    {
        return new List<SimpleModulType>() { SimpleModulType.shieldRegen, SimpleModulType.engineLocker };
    }

    protected override List<WeaponType> AllWeaponModuls()
    {
        return new List<WeaponType>() {WeaponType.laser,WeaponType.impulse};
    }

    public ArmyCreatorLaser(ShipConfig config, bool isAi) : base(config, isAi)
    {
    }
}

