
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class BeamWeaponInventory : WeaponInv
{
    public BeamWeaponInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.beam, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new BeamWeapon(this);
    }
}

