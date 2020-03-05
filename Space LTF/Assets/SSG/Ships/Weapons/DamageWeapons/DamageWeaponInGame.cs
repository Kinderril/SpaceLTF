public abstract class DamageWeaponInGame : WeaponInGame
{
    protected DamageWeaponInGame(WeaponInv weaponInv) : base(weaponInv)
    {

    }

    public override void AffectBulletOnShip(ShipParameters shipParameters, ShipBase target, Bullet bullet, DamageDoneDelegate callback,
        WeaponAffectionAdditionalParams additional)
    {
        shipParameters.DamageByWeaponBullet(bullet, CurrentDamage, callback, target);
    }
}

