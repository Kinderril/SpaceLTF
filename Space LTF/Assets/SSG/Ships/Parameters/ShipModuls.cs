using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ShipModuls
{

    public List<BaseSupportModul> SupportModuls;
    public List<ActionModulInGame> Moduls;
    private ShipBase _owner;
    private ActionModulInGame UpdatableModul;

    public ShipModuls(ShipBase owner, List<BaseModulInv> moduls)
    {
        _owner = owner;
        Moduls = new List<ActionModulInGame>();
        SupportModuls = new List<BaseSupportModul>();
        for (int i = 0; i < moduls.Count; i++)
        {
            var m = moduls[i];
            var isSupport = m.IsSupport;
            if (isSupport)
            {
                if (m is BaseSupportModul sup)
                {
                    SupportModuls.Add(sup);
                }

                continue;
            }
            var modul = ActionModulInGame.Create(m);
            Moduls.Add(modul);
            var mine = modul as MineAbstractModul;
            if (mine != null)
            {
                UpdatableModul = mine;
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

