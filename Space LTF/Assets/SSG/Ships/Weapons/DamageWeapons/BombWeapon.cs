using UnityEngine;


public class BombWeapon : DamageWeaponInGame
{

    public BombWeapon(WeaponInv w)
        : base(w)
    {
    }

    public override bool IsAimed(IShipData target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
    }

    public override void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        var r1_min = 40f;
        var r2_max = 70f;

        var rnd1_1 = Utils.RotateOnAngUp(dir, MyExtensions.RandomSing() * MyExtensions.Random(r1_min, r2_max));
        var rnd1_2 = Utils.RotateOnAngUp(dir, MyExtensions.RandomSing() * MyExtensions.Random(r1_min, r2_max));
        var rnd1_3 = Utils.RotateOnAngUp(dir, MyExtensions.RandomSing() * MyExtensions.Random(r1_min, r2_max));

        var baseEndPoint1_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f, 1.1f) * rnd1_1;
        var baseEndPoint2_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f, 1.1f) * rnd1_2;
        var baseEndPoint3_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f, 1.1f) * rnd1_3;

        CreateBulletWithModif(baseEndPoint1_1, false);
        CreateBulletWithModif(baseEndPoint2_1, false);
        CreateBulletWithModif(baseEndPoint3_1, false);
    }
    //    public override void BulletCreateByDir(ShipBase target, Vector3 dir)
    //    {
    //        var r1_min = 10f;
    //        var r2_max = 80f;
    //        var rnd1_1 = Utils.RotateOnAngUp(dir, MyExtensions.Random(r1_min, r2_max));
    //        var rnd2_1 = Utils.RotateOnAngUp(dir, -MyExtensions.Random(r1_min, r2_max));  
    //
    //
    //        var baseEndPoint1_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd1_1;
    //        var baseEndPoint2_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd2_1;
    //
    //        CreateBulletWithModif(baseEndPoint1_1);
    //        CreateBulletWithModif(baseEndPoint2_1);
    //    }
}

