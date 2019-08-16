using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public TextMeshProUGUI MaxLevel;
    public TextMeshProUGUI CostCountField;
    public TextMeshProUGUI CostDelayField;
    public TextMeshProUGUI WeaponLevelField;
    public MoneySlotUI UpgradeCost;
    private BaseSpellModulInv _spell;
    public void Init(BaseSpellModulInv spell,Action callback, bool canChange)
    {
        base.Init(callback);
        _spell = spell;
        NameField.text = Namings.SpellName(spell.SpellType);
        OnUpgrade(spell);
        _spell.OnUpgrade += OnUpgrade;
        ButtonUpgrade.interactable = canChange;
        CostCountField.text = $"Charges count {spell.CostCount}";
        CostDelayField.text = $"Charges delay {spell.CostTime}";
        //        LevelField.text =(modul.Level.ToString("0"));
    }

    private void OnUpgrade(BaseSpellModulInv obj)
    {
        DrawLevel();
        DescField.text = obj.Desc();
    }

    private void DrawLevel()
    {
        var canUpgrade = _spell.CanUpgrade();
        ButtonContainer.gameObject.SetActive(canUpgrade);
        MaxLevel.gameObject.SetActive(!canUpgrade);
        if (canUpgrade)
        {
            var cost = MoneyConsts.SpellUpgrade[_spell.Level];
            UpgradeCost.Init(cost);
        }
        WeaponLevelField.text = _spell.Level.ToString();
        if (!canUpgrade)
        {
            WeaponLevelField.text = "Max level";
        }
        else
        {

            WeaponLevelField.text = $"Level {_spell.Level.ToString()}";
        }
    }

    public void OnClickUpgrade()
    {
        _spell.TryUpgrade();
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

