using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BombWeapon : WeaponInGame
{
//    private const float MINE_AOE =3;
    private const float MINE_COUNT =4;
    private const float MIN_DISP = 2;
    private const float MAX_DISP = 5;

//    private Bullet subBullet;

    public BombWeapon(WeaponInv w) 
        : base(w)
    {
//        subBullet = DataBaseController.Instance.GetBullet(WeaponType.subMine);
//        if (subBullet == null)
//        {
//            Debug.LogError("sub mine bullets is null ");
//        }
//        if (subBullet.ID == bulletOrigin.ID)
//        {
//            Debug.LogError("sub mine same ids:" + subBullet.ID);
//        }
    }
    
    public override bool IsAimed(ShipPersonalInfo target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
    }

//    public override void ApplyToShip(ShipParameters shipParameters, ShipBase target, Bullet bullet)
//    {
//        if (bullet.ID == subBullet.ID)
//        {
//            target.HitData.HitTo(ShipHitVisual.medium);
//        }
//        AffectTotal(shipParameters, target, bullet, new WeaponAffectionAdditionalParams());
//    }

    public override void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        var r1_min = 60f;
        var r2_max = 80f;
        var rnd1_1 = Utils.RotateOnAngUp(dir, MyExtensions.Random(r1_min, r2_max));
        var rnd2_1 = Utils.RotateOnAngUp(dir, -MyExtensions.Random(r1_min, r2_max));  
        var r1_2_min = 100f;
        var r2_2_max = 120f;
//        var rnd1_2 = Utils.RotateOnAngUp(dir, MyExtensions.Random(r1_2_min, r2_2_max));
//        var rnd2_2 = Utils.RotateOnAngUp(dir, -MyExtensions.Random(r1_2_min, r2_2_max));


        var baseEndPoint1_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd1_1;
        var baseEndPoint2_1 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd2_1;
        //        var baseEndPoint1_2 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd1_2;
        //        var baseEndPoint2_2 = ShootPos.position + _radiusShoot * MyExtensions.Random(0.9f,1.1f) * rnd2_2;

        CreateBulletWithModif(baseEndPoint1_1);
        CreateBulletWithModif(baseEndPoint2_1);
//        CreateBulletAction(new BulletTarget(baseEndPoint1_1), bulletOrigin, this, ShootPos.position,
//            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));    
//        CreateBulletAction(new BulletTarget(baseEndPoint2_1), bulletOrigin, this, ShootPos.position,
//            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));    
//        CreateBulletAction(new BulletTarget(baseEndPoint1_2), bulletOrigin, this, ShootPos.position,
//            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));    
//        CreateBulletAction(new BulletTarget(baseEndPoint2_2), bulletOrigin, this, ShootPos.position,
//            new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, _radiusShoot, _radiusShoot));
    }

//    public override void BulletDestroyed(Vector3 position, Bullet bullet)
//    {
//        if (bullet.ID == bulletOrigin.ID)
//        {
//            var yTarget = ShootPos.position.y;
////            float minDisp = 2;
////            float maxDisp = 5;
//
////            var half = MyExtensions.IsTrueEqual()?5f:-5f;
//            var startPos = bullet.Position;
////            var dirToShoot = bullet.CurDir();
//            for (int i = 0; i < MINE_COUNT; i++)
//            {
//                var xx = MyExtensions.RandomSing() * MyExtensions.Random(MIN_DISP, MAX_DISP);
//                var zz = MyExtensions.RandomSing() * MyExtensions.Random(MIN_DISP, MAX_DISP);
//                var targetPos = startPos + new Vector3(xx, 0, zz);
//                targetPos.y = yTarget;
////                half = -half;
////                var r1 = Utils.RotateOnAngUp(dirToShoot, half*i);
//                CreateBulletAction(new BulletTarget(targetPos), subBullet, this, startPos,
//                    new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, AimRadius, AimRadius));
//            }
//
//
//
//            base.BulletDestroyed(position,bullet);
//        }
//    }
}

