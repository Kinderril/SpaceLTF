[System.Serializable]
public class WeaponDamageTimeEffect : BaseSupportModul
{
    private const float Chance = 0.3f;
    private const float PerLevel = 0.25f;
    private const float Period = 8f;
    private ShipDamageType _damageType;
    public WeaponDamageTimeEffect(ShipDamageType damageType, int level)
        : base(ByDmg(damageType), level)
    {
        _damageType = damageType;
    }

    //protected virtual bool AffectImplement => true;
    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        if (MyExtensions.IsTrue01(ChanceLevel()))
            ship.DamageData.ApplyEffect(_damageType, 8f);
    }

    protected void AffectTargetDelegateSupport(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        ship.DamageData.Repair(_damageType);
    }

    private float ChanceLevel()
    {
        return Chance + Level * PerLevel;
    }

    public override string DescSupport()
    {
        string period = Namings.Format(Namings.Tag("DamageTimeEffect"), Utils.FloatToChance(ChanceLevel()), Period);
        switch (_damageType)
        {
            case ShipDamageType.engine:
                return Namings.Format(Namings.Tag("DamageTimeEffectEngine"), period);
            case ShipDamageType.shiled:
                return Namings.Format(Namings.Tag("DamageTimeEffectShield"), period);
            case ShipDamageType.fire:
                return Namings.Format(Namings.Tag("DamageTimeEffectFire"), period);
            default:
                return "Error";
        }
    }

    public override string DescSupport(WeaponInv inv)
    {
        if (inv.TargetType == TargetType.Enemy)
        {
            return DescSupport();
        }
        else
        {
            return Namings.Tag("DamageTimeEffectRepair");
        }

    }

    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        if (affections.TargetType == TargetType.Enemy)
        {
            affections.Add(AffectTargetDelegate);
        }
        else
        {
            affections.Add(AffectTargetDelegateSupport);
        }
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
