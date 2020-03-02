using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class ShieldSuppotWeaponInventory : WeaponInv
{
    public int DROIDS_COUNT = 5;
    private float min_dist = .4f;
    private float max_dist = .7f;
    public ShieldSuppotWeaponInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.healShieldSupport, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new ShieldlSupportWeaponInGame(this);
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
            var ang = Utils.RotateOnAngUp(dir, dng);
            var pos = shootPos + ang * d;
            var dirToShoot = target.IsDir ? target.Position : target.Position - pos;
            var b = Bullet.Create(origin, weapon, dirToShoot, pos, target.target, bulleStartParameters);
        }
    }
}

