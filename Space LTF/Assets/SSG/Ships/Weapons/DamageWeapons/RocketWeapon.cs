public class RocketWeapon : DamageWeaponInGame
{
    public RocketWeapon(WeaponInv w) :
        base(w)
    {
        //        BulletType = BulletType.single;
    }

    public override bool IsAimed(IShipData target)
    {
        return IsInSector(target.DirNorm);
    }

}

