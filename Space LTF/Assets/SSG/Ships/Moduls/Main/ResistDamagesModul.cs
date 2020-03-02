using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class ResistDamagesModul : ActionModulInGame
{

    public ResistDamagesModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        owner.DamageData.IsReflecOn = true;
    }


}

