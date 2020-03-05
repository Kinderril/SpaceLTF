using UnityEngine;

[System.Serializable]
public class WeaponCritModul : BaseSupportModul
{
    private const float Chance = .15f;
    private const float PerLevel = .10f;
    private const int Damage = 9;
    public WeaponCritModul(int level)
        : base(SimpleModulType.WeaponCrit, level)
    {
    }

    protected override bool AffectTargetImplement => true;
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

    private float ChanceLevel()
    {
        return Chance + Level * PerLevel;
    }
    public override string DescSupport(WeaponInv inv)
    {
        if (inv.TargetType == TargetType.Enemy)
        {
            return DescSupport();
        }
        else
        {
            return Namings.Tag("WeaponCritSupport");
        }

    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponCrit"), Utils.FloatToChance(ChanceLevel()), Damage);
    }

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        if (MyExtensions.IsTrue01(ChanceLevel()))
        {
            FlyNumberWithDependence.Create(ship.transform, Namings.Format(Namings.Tag("Crit"), Damage), Color.red, FlyNumerDirection.right);
            paramsTargte.Damage(0, Damage, doneDelegate, ship);
        }
    }

    protected void AffectTargetDelegateSupport(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        if (MyExtensions.IsTrue01(ChanceLevel()))
        {
            var healCount = Damage * 1.5f;
            FlyNumberWithDependence.Create(ship.transform, Namings.Format(Namings.Tag("Crit"), healCount), Color.red, FlyNumerDirection.right);
            paramsTargte.HealHp(healCount);
        }
    }
}
