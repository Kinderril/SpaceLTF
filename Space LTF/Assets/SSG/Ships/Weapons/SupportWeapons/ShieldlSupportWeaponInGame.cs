public class ShieldlSupportWeaponInGame : SupportWeaponInGame
{
    public ShieldlSupportWeaponInGame(WeaponInv w) : base(w)
    {
    }

    public override void AffectBulletOnShip(ShipParameters shipParameters, ShipBase target, Bullet bullet, DamageDoneDelegate callback,
        WeaponAffectionAdditionalParams additional)
    {
        shipParameters.ShieldParameters.HealShield(CurrentDamage.ShieldDamage);
    }
    public override EWeaponBuffType BuffType => EWeaponBuffType.Shield;
}

