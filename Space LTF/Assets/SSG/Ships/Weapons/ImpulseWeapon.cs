using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ImpulseWeapon : WeaponInGame
{
//    private const float AOE_DAMAGE = 5f;
//    private bool _chainStrike;

    public ImpulseWeapon(WeaponInv w) 

        : base(w)
    {
    }
    
    public override bool IsAimed(ShipPersonalInfo target)
    {

        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
    }

    protected override void CutTargetShootDir(ShipBase target)
    {
        ShootDir(null);
    }
}

