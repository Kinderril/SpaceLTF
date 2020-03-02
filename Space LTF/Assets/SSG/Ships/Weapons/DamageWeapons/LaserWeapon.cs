public class LaserWeapon : DamageWeaponInGame
{
    public LaserWeapon(WeaponInv w) :
        base(w)
    {
        //        BulletType = BulletType.single;
    }

    public override bool IsAimed(IShipData target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
    }
    protected override void CutTargetShootDir(ShipBase target)
    {
        ShootDir(null);
    }
}

