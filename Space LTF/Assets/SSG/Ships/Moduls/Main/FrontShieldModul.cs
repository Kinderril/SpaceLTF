using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class FrontShieldModul : BaseModul
{

    public FrontShieldModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Period = 40 - ModulData.Level * 5;
    }



    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        Parameters.BulletHitModificators.Add(HitModification);
//        Parameters.ShieldModifications.Add();
    }

    private CurWeaponDamage HitModification(CurWeaponDamage damage, Bullet bullet, ShipBase target)
    {
        if (IsReady())
        {
            var dot = Utils.FastDot(bullet.LookDirection, target.LookDirection) < 0;
            if (dot)
            {
                damage.BodyDamage = 0;
                damage.ShieldDamage = 0;
                Use();
            }
        }
        return damage;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override void Delete()
    {
        base.Delete();
    }
}

