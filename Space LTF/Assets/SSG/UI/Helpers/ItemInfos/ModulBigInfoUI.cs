using System;
using TMPro;


public class ModulBigInfoUI : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI SupportField;
    public TextMeshProUGUI ReqireLevelFeild;

    public void Init(BaseModulInv modul, Action callback)
    {
        base.Init(callback);
        NameField.text = Namings.SimpleModulName(modul.Type);
        //        string desc;
        var supprt = modul as BaseSupportModul;
        if (supprt != null)
        {
            SupportField.text = Namings.SupportModul;
            DescField.text = supprt.DescSupport();
        }
        else
        {
            SupportField.text = Namings.ActionModul;
            DescField.text = modul.GetDesc();
        }
        LevelField.text = (modul.Level.ToString("0"));
        ReqireLevelFeild.text = String.Format(Namings.ReqireLevelFeild, modul.RequireLevel());
    }

}

