using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class ArmorModul : BaseModul
{  
    public ArmorModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {

    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        if (Parameters.BulletHitModificators == null)
        {
            Parameters.BulletHitModificators = new List<BulletDamageModif>();
        }
        Parameters.BulletHitModificators.Add(HitModification);
//        Parameters.ShieldModifications.Add();
    }

    private CurWeaponDamage HitModification(CurWeaponDamage damage, Bullet bullet, ShipBase target)
    {
        var copy = damage.Copy();
        copy.BodyDamage = Mathf.Clamp(copy.BodyDamage - ModulData.Level,0,9999);
        return copy;
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

