using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BeamWeapon : WeaponInGame
{
    public BeamWeapon(WeaponInv w) :
        base(w)
    {
//        BulletType = BulletType.single;
    }

    public override bool IsAimed(ShipPersonalInfo target)
    {
        return true;
    }

}

