using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]
public class AutoRepairModul : BaseModul
{
    private bool heapNextFrame = false;
    private bool heapNextFrame2 = false;

    public AutoRepairModul(BaseModulInv b) 
        : base(b)
    {
        Period = 65f - ModulData.Level * 22;
    }
    
    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters,owner);
        _owner.ShipParameters.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float curent, float max, float delta, ShipBase shipowner)
    {
        var c = curent / max < 0.3f;
        if (c)
        {
            heapNextFrame = true;
        }
    }

    public override void UpdateBattle()
    {

        if (heapNextFrame2)
        {
            heapNextFrame2 = false;
            if (IsReady())
            {
                var hpToHeal = _owner.ShipParameters.MaxHealth * 0.5f;
                _owner.ShipParameters.HealHp(hpToHeal);
                UpdateTime();
            }
        }
        if (heapNextFrame)
        {
            heapNextFrame = false;
            heapNextFrame2 = true;
        }
        base.UpdateBattle();
    }

    public override void Dispose()
    {
//        isCharged = false;
        _owner.ShipParameters.OnHealthChanged -= OnHealthChanged;
        base.Dispose();
    }

    public override void Delete()
    {
//        isCharged = false;
        _owner.ShipParameters.OnHealthChanged -= OnHealthChanged;
        base.Delete();
    }
}

