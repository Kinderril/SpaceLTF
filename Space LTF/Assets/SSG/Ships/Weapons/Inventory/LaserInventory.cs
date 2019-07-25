using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class LaserInventory :WeaponInv
{
    public LaserInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.laser, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new LaserWeapon(this);
    }
}

