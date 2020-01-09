using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LaserWeapon : WeaponInGame
{
    public LaserWeapon(WeaponInv w) :
        base(w)
    {
//        BulletType = BulletType.single;
    }

    public override bool IsAimed(ShipPersonalInfo target)
    {                                                                       
        return IsAimedStraight(target, Owner, GetShootPos,_radiusShoot);
    }
    protected override void CutTargetShootDir(ShipBase target)
    {
        ShootDir(null);
    }
}

