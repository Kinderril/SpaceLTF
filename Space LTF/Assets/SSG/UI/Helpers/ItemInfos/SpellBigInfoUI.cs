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
    // public TextMeshProUGUI CostCountField;
    public TextMeshProUGUI CostDelayField;
    public TextMeshProUGUI WeaponLevelField;
    public TextMeshProUGUI UpgradeMicrochipsCount;
    public MoneySlotUI UpgradeCost;
    private BaseSpellModulInv _spell;
    public Transform ModulesContainer;
    public ObjectWithTextMeshPro ModulPrefab;


    public void Init(BaseSpellModulInv spell, Action callback, bool canChange)
    {
        
        base.Init(callback, spell);
        ModulesContainer.ClearTransform();
        _spell = spell;
        NameField.text = Namings.SpellName(spell.SpellType);
        OnUpgrade(spell);
        _spell.OnUpgrade += OnUpgrade;
        ButtonUpgrade.interactable = canChange;
        var haveModules = CheckSupportModules();
        ModulesContainer.gameObject.SetActive(haveModules);
    }

    private bool CheckSupportModules()
    {
        var shipInv = _spell.CurrentInventory as ShipInventory;
        if (shipInv == null)
        {
            return false;
        }

        if (shipInv.SpellConnectedModules.Length == 0)
        {
            return false;
        }

        var myIndex = _spell.CurrentInventory.GetItemIndex(_spell);
        if (myIndex >= 0 && myIndex < shipInv.SpellConnectedModules.Length)
        {
            var supportModulesContainer = shipInv.SpellConnectedModules[myIndex];
            bool haveUsableMopduls = false;
            foreach (var item in supportModulesContainer.GetAllItems())
            {
                var supItem = item as BaseSupportModul;
                if (supItem != null)
                {
                    haveUsableMopduls = true;
                    ImplementSupportField(supItem );
                }
            }

            return haveUsableMopduls;
        }
        return false;
    }

    private void ImplementSupportField(BaseSupportModul supItem)
    {
        var data = DataBaseController.GetItem(ModulPrefab);
        data.Field.text = supItem.DescSupport();
          data.transform.SetParent(ModulesContainer,false);
    }

    private void OnUpgrade(BaseSpellModulInv obj)
    {
        DrawLevel();
        DescField.text = obj.DescFull();
    }

    private void DrawLevel()
    {

        CostDelayField.text = Namings.Format(Namings.Tag("ChargesDelay"), _spell.CostTime);
        var canUpgrade = _spell.CanUpgradeByLevel();
        ButtonContainer.gameObject.SetActive(canUpgrade);
        MaxLevel.gameObject.SetActive(!canUpgrade);
        if (canUpgrade)
        {
            var cost = MoneyConsts.SpellUpgrade[_spell.Level];
            var upgradeElements = MoneyConsts.SpellMicrochipsElements[_spell.Level];
            UpgradeCost.Init(cost);
            UpgradeMicrochipsCount.text = (upgradeElements).ToString();
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
        ModulesContainer.ClearTransform();
        Unsubscibe();
    }

    void OnDestroy()
    {
        //        Unsubscibe();
    }
}

