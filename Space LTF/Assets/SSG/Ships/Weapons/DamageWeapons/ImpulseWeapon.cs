public class ImpulseWeapon : DamageWeaponInGame
{
    //    private const float AOE_DAMAGE = 5f;
    //    private bool _chainStrike;

    public ImpulseWeapon(WeaponInv w)

        : base(w)
    {
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

