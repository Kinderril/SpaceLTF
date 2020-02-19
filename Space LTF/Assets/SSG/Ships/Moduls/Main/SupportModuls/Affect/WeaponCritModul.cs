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
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }

    private float ChanceLevel()
    {
        return Chance + Level * PerLevel;
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponCrit"),
            Utils.FloatToChance(ChanceLevel()), Damage);
    }

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        if (MyExtensions.IsTrue01(ChanceLevel()))
        {
            FlyNumberWithDependence.Create(ship.transform, Namings.Format(Namings.Tag("Crit"), Damage), Color.red, FlyNumerDirection.right);
            paramsTargte.Damage(0, Damage, doneDelegate, ship);
        }
    }


}
