public abstract class DamageWeaponInGame : WeaponInGame
{
    protected DamageWeaponInGame(WeaponInv weaponInv) : base(weaponInv)
    {

    }

    public override void AffectBulletOnShip(ShipParameters shipParameters, ShipBase target, Bullet bullet, DamageDoneDelegate callback,
        WeaponAffectionAdditionalParams additional)
    {
        // if (_nextShootMorePower)
        // {
        //     _nextShootMorePower = false;
        //     CurrentDamage.ShieldDamage *= 2f;
        //     CurrentDamage.BodyDamage *= 2f;
        // }

        shipParameters.DamageByWeaponBullet(bullet, CurrentDamage, callback, target);
    }
}

