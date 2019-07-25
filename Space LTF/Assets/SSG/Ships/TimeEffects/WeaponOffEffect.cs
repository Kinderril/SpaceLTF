using JetBrains.Annotations;

public class WeaponOffEffect : BaseTimeEffect
{
    public WeaponOffEffect([NotNull] ShipBase shipToApply, float deltaTimeSec) 
        : base(shipToApply, deltaTimeSec)
    {
    }

    protected override void Apply()
    {
        _shipToApply.WeaponsController.Enable(false);
    }

    protected override void DisApply()
    {
        _shipToApply.WeaponsController.Enable(true);
        base.DisApply();
    }
}