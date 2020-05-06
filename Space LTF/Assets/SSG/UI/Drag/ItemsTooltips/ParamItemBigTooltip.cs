using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class ParamItemBigTooltip : ItemBigTooltip
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI RarityField;

    public void Init(ParameterItem parameterItem,int? costVal, GameObject causeTransform)
    {
        Init(causeTransform);
        NameField.text = Namings.ParameterModulName(parameterItem.ItemType);
        RarityField.text = Namings.Tag($"EParameterItemRarity{parameterItem.Rarity}");
        
        RarityField.color = ParameterItem.GetColor(parameterItem.Rarity);
        string desc = "";
        foreach (var f in parameterItem.ParametersAffection)
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
                desc = $"{desc}\n{name}: +{ParameterItemBigInfoUI.GetVal(val)}\n";
            }
            else
            {
                desc = $"{desc}\n{name}: -{ParameterItemBigInfoUI.GetVal(val)}\n";
            }
        }
        DescField.text = desc;
        SetSellCost(costVal,parameterItem);
    }


}
