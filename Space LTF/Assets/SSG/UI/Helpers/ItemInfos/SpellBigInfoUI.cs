using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpellBigInfoUI : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    //    public TextMeshProUGUI LevelField;
    public GameObject ButtonContainer;
    public Button ButtonUpgrade;

    public Transform ContainerSingleUpg;
    public Transform ContainerDoubleUpg;
    // public Button ButtonUpgradeA1;
    // public Button ButtonUpgradeB2;
    public TextMeshProUGUI UpgradeA1Field;
    public TextMeshProUGUI UpgradeB2Field;
    public UIElementWithTooltipCache UpgradeA1Tooltip;
    public UIElementWithTooltipCache UpgradeB2Tooltip;


    public TextMeshProUGUI MaxLevel;
    public TextMeshProUGUI CostCountField;
    public TextMeshProUGUI CostDelayField;
    public TextMeshProUGUI WeaponLevelField;
    public MoneySlotUI UpgradeCost;
    private BaseSpellModulInv _spell;


    public void Init(BaseSpellModulInv spell, Action callback, bool canChange)
    {
        base.Init(callback);
        _spell = spell;
        NameField.text = Namings.SpellName(spell.SpellType);
        OnUpgrade(spell);
        _spell.OnUpgrade += OnUpgrade;
        ButtonUpgrade.interactable = canChange;

    }

    private void OnUpgrade(BaseSpellModulInv obj)
    {
        DrawLevel();
        DescField.text = obj.DescFull();
    }

    private void DrawLevel()
    {

        CostCountField.text = Namings.Format(Namings.Tag("ChargesCount"), _spell.CostCount);
        CostDelayField.text = Namings.Format(Namings.Tag("ChargesDelay"), _spell.CostTime);
        var canUpgrade = _spell.CanUpgrade();
        ButtonContainer.gameObject.SetActive(canUpgrade);
        MaxLevel.gameObject.SetActive(!canUpgrade);
        if (canUpgrade)
        {
            var cost = MoneyConsts.SpellUpgrade[_spell.Level];
            UpgradeCost.Init(cost);
            var shallChoose = _spell.ShallAddSpecialNextLevel();
            ContainerSingleUpg.gameObject.SetActive(!shallChoose);
            ContainerDoubleUpg.gameObject.SetActive(shallChoose);
            if (shallChoose)
            {
                UpgradeA1Field.text = _spell.GetUpgradeName(ESpellUpgradeType.A1);
                UpgradeB2Field.text = _spell.GetUpgradeName(ESpellUpgradeType.B2);
                UpgradeA1Tooltip.Cache = _spell.GetUpgradeDesc(ESpellUpgradeType.A1);
                UpgradeB2Tooltip.Cache = _spell.GetUpgradeDesc(ESpellUpgradeType.B2);
            }
        }
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

    public void OnClickUpgradeB2()
    {
        _spell.TryUpgrade(ESpellUpgradeType.B2);
    }
    public void OnClickUpgradeA1()
    {
        _spell.TryUpgrade(ESpellUpgradeType.A1);
    }
    public void OnClickUpgrade()
    {
        _spell.TryUpgrade(ESpellUpgradeType.None);
    }


    private void Unsubscibe()
    {
        _spell.OnUpgrade -= OnUpgrade;
    }

    public override void Dispose()
    {
        Unsubscibe();
    }

    void OnDestroy()
    {
        //        Unsubscibe();
    }
}

