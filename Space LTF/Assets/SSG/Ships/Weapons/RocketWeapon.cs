using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class RocketWeapon : WeaponInGame
{
    public RocketWeapon(WeaponInv w) :
        base(w)
    {
//        BulletType = BulletType.single;
    }

    public override bool IsAimed(ShipPersonalInfo target)
    {
        return IsInSector(target.DirNorm);
    }

}

