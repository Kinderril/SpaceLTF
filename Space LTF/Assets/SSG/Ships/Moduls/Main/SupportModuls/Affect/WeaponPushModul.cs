[System.Serializable]
public class WeaponPushModul : BaseSupportModul
{
    public WeaponPushModul(int level)
        : base(SimpleModulType.WeaponPush, level)
    {
    }

    protected override bool AffectTargetImplement => true;

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        ship.ExternalForce.Init(8, 1f, bullet.LookDirection);
    }

    protected void AffectTargetDelegateSupport(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        ship.ExternalForce.Stop();
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponPush"));
    }
    public override string DescSupport(WeaponInv inv)
    {
        if (inv.TargetType == TargetType.Enemy)
        {
            return DescSupport();
        }
        else
        {
            return Namings.Format(Namings.Tag("WeaponPushSupport"));
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
}
