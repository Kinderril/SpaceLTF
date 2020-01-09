using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EMIRocketWeaponGame : WeaponInGame
{
    public EMIRocketWeaponGame(WeaponInv w) :
        base(w)
    {
//        BulletType = BulletType.single;
    }

    public override bool IsAimed(ShipPersonalInfo target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
        // return target.IsInFrontSector();
    }

    protected override void CutTargetShootDir(ShipBase target)
    {
        ShootDir(null);
    }
}

