using System.Linq;
using JetBrains.Annotations;

public class DisableModulEffect : BaseTimeEffect
{
    private BaseModul _modul;

    public DisableModulEffect([NotNull] ShipBase shipToApply, float deltaTimeSec) 
        : base(shipToApply, deltaTimeSec)
    {

    }

    protected override void Apply()
    {
        _modul  = _shipToApply.ShipModuls.Moduls.RandomElement();
        if (!_modul.IsApply)
        {
            DisApply();
            return;
        }
        _modul.Delete();
    }

    protected override void DisApply()
    {
        if (_modul != null)
        {
            _modul.Apply(_shipToApply.ShipParameters,_shipToApply);
        }
        base.DisApply();
    }
}