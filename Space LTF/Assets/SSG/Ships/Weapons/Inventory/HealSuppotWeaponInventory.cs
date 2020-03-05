﻿using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class HealSuppotWeaponInventory : SupportWeaponInv
{
    public int DROIDS_COUNT = 4;
    private float min_dist = .4f;
    private float max_dist = .7f;
    public HealSuppotWeaponInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.healBodySupport, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new HealSupportWeaponInGame(this);
    }


    public override void BulletCreate(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootPos,
        BulleStartParameters bulleStartParameters)
    {
        var dir = weapon.Owner.LookDirection;
        var deltaAng = 360 / DROIDS_COUNT;
        for (int i = 0; i < DROIDS_COUNT; i++)
        {
            var d = MyExtensions.Random(min_dist, max_dist);
            var dng = MyExtensions.GreateRandom(i * deltaAng);
            var angDir = Utils.RotateOnAngUp(dir, dng);
            var pos = shootPos + angDir * d;
            var b = Bullet.Create(origin, weapon, angDir, pos, target.target, bulleStartParameters);
        }
    }
}

