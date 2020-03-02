public enum EWeaponBuffType
{
    Body,
    Shield,
    Buff
}

public abstract class SupportWeaponInGame : WeaponInGame
{
    public SupportWeaponInGame(WeaponInv w) :
        base(w)
    {

    }

    public abstract EWeaponBuffType BuffType { get; }

    public override bool IsAimed(IShipData target)
    {
        return true;
    }

}

