using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ShipModuls
{

    public List<BaseSupportModul> SupportModuls;
    public List<BaseModul> Moduls;
    private ShipBase _owner;
    private BaseModul UpdatableModul;

    public ShipModuls(ShipBase owner, BaseModulInv[] moduls)
    {
        _owner = owner;
        Moduls = new List<BaseModul>();
        SupportModuls = new List<BaseSupportModul>();
        for (int i = 0; i < moduls.Length; i++)
        {
            var m = moduls[i];
            if (m != null)
            {
                var isSupport = m.IsSupport;
                if (isSupport)
                {
                    if (m is BaseSupportModul sup)
                    {
                        SupportModuls.Add(sup);
                    }

                    continue;
                }
                var modul = BaseModul.Create(m);
                Moduls.Add(modul);
                var mine = modul as MineAbstractModul;
                if (mine != null)
                {
                    UpdatableModul = mine;
                }

            }
        }
    }

    public void InitModuls()
    {
        foreach (var baseModul in Moduls)
        {
            baseModul.Apply(_owner.ShipParameters, _owner);
        }

        foreach (var baseSupportModul in SupportModuls)
        {
            baseSupportModul.ChangeParamsShip(_owner.ShipParameters);
        }
    }

    public void Dispose()
    {
        foreach (var baseModul in Moduls)
        {
            baseModul.Dispose();

        }
    }

    public void Update()
    {
        if (UpdatableModul != null)
        {
            UpdatableModul.UpdateBattle();
        }

    }
}

