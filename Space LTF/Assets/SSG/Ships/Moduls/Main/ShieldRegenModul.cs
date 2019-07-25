using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class ShieldRegenModul : BaseModul
{
    private const float  SHILD_PER_MIN_BASE = 5f;
    private const float  SHILD_PER_MIN_LEVEL = 4f;

    public ShieldRegenModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        var d = ModulData.Level * SHILD_PER_MIN_LEVEL + SHILD_PER_MIN_BASE;
        _delta = d/60f;
        Parameters.ShieldParameters.ShieldRegenPerSec += _delta;
        base.Apply(Parameters,owner);
    }

    public override void Delete()
    {
        _parameters.ShieldParameters.ShieldRegenPerSec -= _delta;
        base.Delete();
    }
}

