using System;
using TMPro;


public class ParameterItemBigInfoUI : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI RarityField;

    public void Init(ParameterItem modul, Action callback)
    {
        base.Init(callback);
        NameField.text = Namings.ParameterModulName(modul.ItemType);
        RarityField.text = (modul.Rarity.ToString("0"));
    }

}

