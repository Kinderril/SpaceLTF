using JetBrains.Annotations;
using System;

[System.Serializable]
public class BombInventoryWeapon : WeaponInv
{

    [NonSerialized]
    private Bullet suBullet;
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

