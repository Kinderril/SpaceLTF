using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class SpellBigInfoUI : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
//    public TextMeshProUGUI LevelField;

    public void Init(BaseSpellModulInv spell,Action callback)
    {
        base.Init(callback);
        NameField.text = Namings.SpellName(spell.SpellType);
        DescField.text = Namings.SpellDesc(spell.SpellType);
//        LevelField.text =(modul.Level.ToString("0"));
    }
    
}

