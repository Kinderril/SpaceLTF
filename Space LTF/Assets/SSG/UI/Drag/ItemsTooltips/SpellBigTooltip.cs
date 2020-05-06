using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class SpellBigTooltip : ItemBigTooltip
{

    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    //    public TextMeshProUGUI LevelField;

//    public Transform ContainerSingleUpg;
//    public Transform ContainerDoubleUpg;
    // public Button ButtonUpgradeA1;
    // public Button ButtonUpgradeB2;
//    public TextMeshProUGUI UpgradeA1Field;


//    public TextMeshProUGUI MaxLevel;
    public TextMeshProUGUI CostCountField;
    public TextMeshProUGUI CostDelayField;
    public TextMeshProUGUI WeaponLevelField;
    private BaseSpellModulInv _spell;

    public void Init(BaseSpellModulInv spell,int? sellCost, GameObject causeTransform)
    {
        Init(causeTransform);
        _spell = spell;
        NameField.text = Namings.SpellName(spell.SpellType);
        DrawLevel();
        SetSellCost(sellCost,spell);
        DescField.text = spell.DescFull();
    }

    private void DrawLevel()
    {

        CostCountField.text = Namings.Format(Namings.Tag("ChargesCount"), _spell.CostCount);
        CostDelayField.text = Namings.Format(Namings.Tag("ChargesDelay"), _spell.CostTime);
        var canUpgrade = _spell.CanUpgradeByLevel();
//        MaxLevel.gameObject.SetActive(!canUpgrade);


//        switch (_spell.UpgradeType)
//        {
//            case ESpellUpgradeType.None:
//                UpgradeA1Field.gameObject.SetActive(false);
//                break;
//            case ESpellUpgradeType.A1:
//                UpgradeA1Field.text = _spell.GetUpgradeName(ESpellUpgradeType.A1);
//                UpgradeA1Field.gameObject.SetActive(true);
//                break;
//            case ESpellUpgradeType.B2:
//                UpgradeA1Field.text = _spell.GetUpgradeName(ESpellUpgradeType.B2);
//                UpgradeA1Field.gameObject.SetActive(true);
//                break;
//        }

        WeaponLevelField.text = _spell.Level.ToString();
        if (!canUpgrade)
        {
            WeaponLevelField.text = Namings.Format(Namings.Tag("SpellMaxLevel"), _spell.Level.ToString());
        }
        else
        {

            WeaponLevelField.text = Namings.Format(Namings.Tag("Level")) + ":" + _spell.Level.ToString();
        }
    }
}
