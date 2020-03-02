public class EMIRocketWeaponGame : DamageWeaponInGame
{
    public EMIRocketWeaponGame(WeaponInv w) :
        base(w)
    {
        //        BulletType = BulletType.single;
    }

    public override bool IsAimed(IShipData target)
    {
        return IsAimedStraight(target, Owner, GetShootPos, _radiusShoot);
        // return target.IsInFrontSector();
    }

    protected override void CutTargetShootDir(ShipBase target)
    {
        ShootDir(null);
    }
}

