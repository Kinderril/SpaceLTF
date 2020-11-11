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
        base.Init(callback, modul);
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
        LevelField.text = (modul.Level.ToString("0"));
        ReqireLevelFeild.text = Namings.Format(Namings.Tag("ReqireLevelFeild"), modul.RequireLevel());
    }

}

