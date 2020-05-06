using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class ModulBigTooltip : ItemBigTooltip
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI SupportField;
    public TextMeshProUGUI ReqireLevelFeild;

    public void Init(BaseModulInv modul, int? sellCost,GameObject causeTransform)
    {
        NameField.text = Namings.SimpleModulName(modul.Type);
        //        string desc;
        var supprt = modul as BaseSupportModul;
        if (supprt != null)
        {
            SupportField.text = Namings.Tag("SupportModul");
            DescField.text = supprt.DescSupport();
        }
        else
        {
            SupportField.text = Namings.Tag("ActionModul");
            DescField.text = modul.GetDesc();
        }
        LevelField.text = $"{Namings.Tag("Level")}: {modul.Level}";
        ReqireLevelFeild.text = Namings.Format(Namings.Tag("ReqireLevelFeild"), modul.RequireLevel());
        Init(causeTransform);
        SetSellCost(sellCost, modul);
    }

}
