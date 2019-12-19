using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponDamageTimeEffect : BaseSupportModul
{
    private const float Chance = 0.3f;
    private const float PerLevel = 0.25f;
    private const float Period = 8f;
    private ShipDamageType _damageType;
    public WeaponDamageTimeEffect(ShipDamageType damageType, int level)
        : base(  ByDmg(damageType), level)
    {
        _damageType = damageType;
    }

    //protected virtual bool AffectImplement => true;
    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate,WeaponAffectionAdditionalParams additional)
    {
        if (MyExtensions.IsTrue01(ChanceLevel()))
            ship.DamageData.ApplyEffect(_damageType,8f);
    }

    private float ChanceLevel()
    {
        return Chance + Level * PerLevel;
    }

    public override string DescSupport()
    {
        string period = $"with a {Utils.FloatToChance(ChanceLevel())}% chance for {Period:0} sec.";
        switch (_damageType)
        {
            case ShipDamageType.engine:
                return $"Turn off engine {period}";
            // case ShipDamageType.weapon:
            //     return $"Off all weapons {period}";
            case ShipDamageType.shiled:
                return $"Off all shield {period}";
            case ShipDamageType.fire:
                return $"Start fire {period}";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }

    private static SimpleModulType ByDmg(ShipDamageType damageType)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return SimpleModulType.WeaponEngine;
            // case ShipDamageType.weapon:
            //     return SimpleModulType.WeaponWeapon;
            case ShipDamageType.shiled:
                return SimpleModulType.WeaponShield;
            case ShipDamageType.fire:
                return SimpleModulType.WeaponFire;
        }

        return SimpleModulType.WeaponEngine;
    }
}
