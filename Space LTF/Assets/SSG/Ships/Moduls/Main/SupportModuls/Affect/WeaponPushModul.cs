using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponPushModul : BaseSupportModul
{
    public WeaponPushModul(int level)
        : base(SimpleModulType.WeaponPush, level)
    {
    }

    protected override bool AffectTargetImplement
    {
        get { return true; }
    }

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate,WeaponAffectionAdditionalParams additional)
    {
       ship.ExternalForce.Init(8, 1f, bullet.LookDirection);
    }

    public override string DescSupport()
    {
        return String.Format("Push target when hit");
    }
    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }
}
