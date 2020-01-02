using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class ArmorModul : BaseModul
{  
    public ArmorModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        Parameters.BodyArmor += ModulData.Level;
        // Parameters.ShieldArmor += ModulData.Level;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override void Delete()
    {
        base.Delete();
    }
}

