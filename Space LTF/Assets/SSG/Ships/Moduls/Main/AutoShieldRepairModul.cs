using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class AutoShieldRepairModul : ActionModulInGame
{
    private bool isCharged;

    public AutoShieldRepairModul(BaseModulInv b) 
        : base(b)
    {
        isCharged = true;
        Period = 70f - ModulData.Level * 30;
    }
    
    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        _owner.ShipParameters.ShieldParameters.OnShildChanged += OnShildChanged;
    }

    private void OnShildChanged(float curent, float max, float delta, ShieldChangeSt state, ShipBase shipowner)
    {
        if (IsReady())
        {
            var c = curent/max < 0.2f;
            if (c)
            {
                var sp2heal = shipowner.ShipParameters.ShieldParameters.MaxShield*.5f;
                shipowner.ShipParameters.ShieldParameters.HealShield(sp2heal);
                UpdateTime();
            }
        }
    }
    

    public override void Dispose()
    {
        isCharged = false;
        _owner.ShipParameters.ShieldParameters.OnShildChanged -= OnShildChanged;
        base.Dispose();
    }

    public override void Delete()
    {
        isCharged = false;
        _owner.ShipParameters.ShieldParameters.OnShildChanged -= OnShildChanged;
        base.Delete();
    }
}

