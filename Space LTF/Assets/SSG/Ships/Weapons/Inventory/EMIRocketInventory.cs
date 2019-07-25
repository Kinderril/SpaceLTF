using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class EMIRocketInventory : WeaponInv
{


    //    public RoundInventory([NotNull] IInventory currentInventory) : base(currentInventory)
    //    {
    //    }

    public EMIRocketInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.eimRocket, Level)
    {

    }

    public override WeaponInGame CreateForBattle()
    {
        return new EMIRocketWeaponGame(this);
    }
}

