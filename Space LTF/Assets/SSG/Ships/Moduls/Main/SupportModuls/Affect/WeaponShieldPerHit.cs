using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponShieldPerHit : BaseSupportModul
{

    private const float Damage = 0.7f;
    private const int Self = 1;
    public WeaponShieldPerHit(int level)
        : base(SimpleModulType.WeaponShieldPerHit, level)
    {
    }


    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage -= Damage;
        weapon.CurrentDamage.ShieldDamage -= Damage;
    }

    protected override bool AffectTargetImplement => true;

    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(VampireShield);
        return base.AffectTarget(affections);
    }

    private void VampireShield(ShipParameters shipparameters, ShipBase target, Bullet bullet, DamageDoneDelegate damagedone, WeaponAffectionAdditionalParams additional)
    {
        bullet.Weapon.Owner.ShipParameters.ShieldParameters.HealShield(Self*Level);
    }

    public override string DescSupport()
    {
        return $"Decrease damage by {Utils.FloatToChance(Damage)}%. Restore {Self * Level} of shield per hit.";
    }




}
