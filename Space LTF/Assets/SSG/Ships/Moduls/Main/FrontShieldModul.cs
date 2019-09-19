using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


[System.Serializable]
public class FrontShieldModul : BaseModul
{
    private bool _isActivated;
    private float _activationTimeEnd;

    public FrontShieldModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Period = 40 - ModulData.Level * 5;
    }


//    protected override float Delay()
//    {
//        return Period;
//    }
//
//    protected override void TimerAction()
//    {
//
//    }

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
        if (IsReady())
        {
            _isActivated = true;
            _activationTimeEnd = Time.time + 1f;
        }

        if (_isActivated)
        {
            if (_activationTimeEnd < Time.time)
            {
                _isActivated = false;
            }
            var dot = Utils.FastDot(bullet.LookDirection, target.LookDirection) < 0;
            if (dot)
            {
                copy.BodyDamage = 0;
                copy.ShieldDamage = 0;
                Use();
            }
        }

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

