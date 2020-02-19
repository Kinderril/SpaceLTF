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

    protected void AffectTargetDelegate(ShipParameters paramsTargte, ShipBase ship, Bullet bullet, DamageDoneDelegate doneDelegate, WeaponAffectionAdditionalParams additional)
    {
        ship.ExternalForce.Init(8, 1f, bullet.LookDirection);
    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponPush"));
    }
    protected override WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        affections.Add(AffectTargetDelegate);
        return base.AffectTarget(affections);
    }
}
