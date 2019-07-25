using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AbstractWeaponUpgradeModul : BaseModul
{
    private WeaponType _type;

    public AbstractWeaponUpgradeModul(BaseModulInv baseModulInv, WeaponType type) 
        : base(baseModulInv)
    {
        _type = type;
    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {

        base.Apply(Parameters,owner);
    }

    public override void Delete()
    {
        //_parameters.ShieldParameters.ShieldRegenPerSec -= _delta;
        base.Delete();
    }
}

