public class BeamWeapon : DamageWeaponInGame
{
    public BeamWeapon(WeaponInv w) :
        base(w)
    {

    }

    public override bool IsAimed(IShipData target)
    {
        return true;
    }

}

