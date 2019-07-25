using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class ShipTurnModul : BaseModul
{
    public const float  PER_LEVEL = 0.1f;

    public ShipTurnModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        var d = ModulData.Level * PER_LEVEL;
        _delta = d * Parameters.TurnSpeed;
        Parameters.TurnSpeed += _delta;
        base.Apply(Parameters,owner);
    }

    public override void Delete()
    {
        _parameters.TurnSpeed -= _delta;
        base.Delete();
    }
}

