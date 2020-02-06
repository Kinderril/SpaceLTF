using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponShieldIgnore : BaseSupportModul
{
    private const float decrase = 0.7f; 
    public WeaponShieldIgnore(int level)
        : base(SimpleModulType.WeaponShieldIgnore, level)
    {

    }

    protected override bool AffectTargetImplement => true;
    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Main = ChangeMain;
        return base.AffectTarget(affections);
    }

    public override string DescSupport()
    {
        if (Level == 1)
        {
            return ($"Ignore shield. Decrease damage by {Utils.FloatToChance(decrase)}%.");
        }
        else
        {
            return Namings.TryFormat("Ignore shield");
        }
    }

    private void ChangeMain(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        shipparameters.DamageIgnoreShield(bullet.Weapon.CurrentDamage.BodyDamage, damagedone);
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        if (Level == 1)
        {
            weapon.CurrentDamage.BodyDamage *= decrase;
            weapon.CurrentDamage.ShieldDamage *= decrase;
        }
    }

//    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate,WeaponAffectionAdditionalParams additional)
//    {
//        if (MyExtensions.IsTrue01(.25f))
//        {
//            paramsTargte.Damage(0,10,doneDelegate);
//        }
//    }


}
