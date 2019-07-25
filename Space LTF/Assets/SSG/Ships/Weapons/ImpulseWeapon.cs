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
        return IsAimedStraight(target, Owner, GetShootPos, BulletSpeed, _fixedDelta);
    }

//    public override void Affect(ShipParameters shipParameters, ShipBase target, Bullet bullet)
//    {
//        //target.HitData.HitTo(ShipHitVisual.soft);
//        //shipParameters.Damage(ShieldDamage, BodyDamage, DamageDoneCallback);
//        //var shallDestroyEngine = MyExtensions.IsTrue01(0.1f);
//        //if (shallDestroyEngine)
//        //{
//        //    target.DamageData.ApplyEffect(ShipDamageType.shiled, 5f);
//        //}
//    }

//    public override void BulletDestroyed(Vector3 position, Bullet bullet)
//    {
//        if (_chainStrike)
//        {
//            var index = BattleController.OppositeIndex(TeamIndex);
//            var c2 = BattleController.Instance.GetAllShipsInRadius(position, index, AOE_DAMAGE);
//            if (c2.Count > 0)
//            {
//                var rnd = c2.RandomElement();
//                WeaponData.BulletCreate(rnd, bulletOrigin, this, position);
//            }
//
//        }
//    }
}

