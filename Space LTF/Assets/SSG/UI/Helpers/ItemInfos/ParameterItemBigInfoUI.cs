using System;
using TMPro;
using UnityEngine;


public class ParameterItemBigInfoUI : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI RarityField;

    public void Init(ParameterItem modul, Action callback)
    {
        base.Init(callback);
        NameField.text = Namings.ParameterModulName(modul.ItemType);
        RarityField.text = Namings.Tag($"EParameterItemRarity{modul.Rarity}");
        string desc = "";
        foreach (var f in modul.ParametersAffection)
        {
            var name = Namings.Tag($"EParameterShip{f.Key}");
            var val = f.Value;
            if (f.Key == EParameterShip.speed)
            {
                var a = 1;
//                val *= 10f;
            }


            if (val > 0)
            {
                desc = $"{desc}\n{name}: +{GetVal(val)}\n";
            }
            else
            {
                desc = $"{desc}\n{name}: -{GetVal(val)}\n";
            }
        }
        DescField.text = desc;
    }

    private string GetVal(float v)
    {
        var bas = Mathf.Abs(v);
        var d = bas % (int)bas;
        if (d > 0 || (bas > 0 && bas < 1))
        {
            return bas.ToString("0.0");
        }

        return bas.ToString("0");



    }

}

