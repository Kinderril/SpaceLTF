public class HealSupportWeaponInGame : SupportWeaponInGame
{
    public HealSupportWeaponInGame(WeaponInv w) : base(w)
    {
    }

    public override void AffectBulletOnShip(ShipParameters shipParameters, ShipBase target, Bullet bullet, DamageDoneDelegate callback,
        WeaponAffectionAdditionalParams additional)
    {
        shipParameters.HealHp(CurrentDamage.BodyDamage);
    }
    public override EWeaponBuffType BuffType => EWeaponBuffType.Body;
}

