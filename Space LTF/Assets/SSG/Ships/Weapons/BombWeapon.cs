using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BombWeapon : WeaponInGame
{
//    private const float MINE_AOE =3;
//    private const float MINE_COUNT =4;

    private Bullet subBullet;

    public BombWeapon(WeaponInv w) 
        : base(w)
    {
        subBullet = DataBaseController.Instance.GetBullet(WeaponType.subMine);
        if (subBullet == null)
        {
            Debug.LogError("sub mine bullets is null ");
        }
        if (subBullet.ID == bulletOrigin.ID)
        {
            Debug.LogError("sub mine same ids:" + subBullet.ID);
        }
    }
    
    public override bool IsAimed(ShipPersonalInfo target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, BulletSpeed, _fixedDelta);
    }

    public override void ApplyToShip(ShipParameters shipParameters, ShipBase target, Bullet bullet)
    {
        if (bullet.ID == subBullet.ID)
        {
            target.HitData.HitTo(ShipHitVisual.medium);
            AffectTotal(shipParameters, target, bullet,new WeaponAffectionAdditionalParams());
        }
    }
    
    public override void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        if (bullet.ID == bulletOrigin.ID)
        {
            var half = MyExtensions.IsTrueEqual()?5f:-5f;
            var startPos = bullet.Position;
            var dirToShoot = bullet.CurDir();
            for (int i = 0; i < 4; i++)
            {
                half = -half;
                var r1 = Utils.RotateOnAngUp(dirToShoot, half*i);
                CreateBulletAction(new BulletTarget(startPos + r1* AimRadius), subBullet, this, startPos,
                    new BulleStartParameters(BulletSpeed, _bulletTurnSpeed, AimRadius, AimRadius));
            }



            base.BulletDestroyed(position,bullet);
        }
    }
}

