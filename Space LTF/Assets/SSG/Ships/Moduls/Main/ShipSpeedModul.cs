using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class ShipSpeedModul : BaseModul
{
    public const float  PER_LEVEL = 0.1f;

    public ShipSpeedModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        var d = ModulData.Level * PER_LEVEL;
        _delta = d * Parameters.MaxSpeed;
        Parameters.MaxSpeed += _delta;
        base.Apply(Parameters,owner);
    }

    public override void Delete()
    {
        _parameters.MaxSpeed -= _delta;
        base.Delete();
    }
}

