using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class BombInventoryWeapon : WeaponInv
{

    [NonSerialized]
    private Bullet suBullet;
    private int MINE_COUNT = 3;

//    public BombInventoryWeapon([NotNull] IInventory currentInventory) : base(currentInventory)
//    {
//    }

    public BombInventoryWeapon([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.casset, Level)
    {
    }

 

    public override WeaponInGame CreateForBattle()
    {
        suBullet = DataBaseController.Instance.GetBullet(WeaponType.subMine);
        DataBaseController.Instance.Pool.RegisterBullet(suBullet);
        return new BombWeapon(this);
    }
}

