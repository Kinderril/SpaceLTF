using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragableSpellItem : DragableItem
{
    public Button UpgradeButton;
    private bool _isSubscribed;
    public TextMeshProUGUI LevelField;
    public BaseSpellModulInv Spell => ContainerItem as BaseSpellModulInv;

    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(Spell.SpellType);
    }

    protected override void Init()
    {
        if (!_isSubscribed)
        {
            ContainerItem.CurrentInventory.Owner.OnCreditsChange += OnMoneyChange;
            Spell.OnUpgrade += OnUpgrade;
            _isSubscribed = true;
        }
        else
        {
            Debug.LogError("try to _isSubscribed Second time DragableSpellItem");
        }
        base.Init();
        OnUpgrade(Spell);
    }

    private void OnMoneyChange(int obj)
    {
        OnUpgrade(Spell);
    }

    public override string GetInfo()
    {
        LevelField.text = Spell.Level.ToString();
        return Spell.GetInfo();
    }
//    protected override string TextToTooltip()
//    {
//        return $"{Spell.GetInfo()} ({Spell.Level.ToString()})";
//    }
    public void OnTryUpgradeClick()
    {
        if (Usable)
            Spell.TryUpgrade(ESpellUpgradeType.None);
    }
    private void OnUpgrade(BaseSpellModulInv obj)
    {
        if (Spell == null)
        {
            return;
        }
        var cost = MoneyConsts.SpellUpgrade[Spell.Level];
        var upgradeElements = MoneyConsts.SpellMicrochipsElements[Spell.Level];
        var haveMoney = ContainerItem.CurrentInventory.Owner.HaveMoney(cost);
        var haveUpgrades = ContainerItem.CurrentInventory.Owner.HaveMicrochips(upgradeElements);
        var isMy = ContainerItem.CurrentInventory.Owner == Spell.CurrentInventory.Owner;
        var canUse = Spell.CanUpgradeByLevel() && haveMoney && Usable && isMy && haveUpgrades;
        UpgradeButton.gameObject.SetActive(canUse);
        try
        {
            UpdateLevelsInfo(Spell.Level);
        }
        catch (Exception e)
        {
        }

    }

    protected override void OnClickComplete()
    {
        WindowManager.Instance.ItemInfosController.Init(Spell, CanShowWindow());
    }

    protected override void Dispose()
    {
        ContainerItem.CurrentInventory.Owner.OnCreditsChange -= OnMoneyChange;
        if (Spell != null)
        {
            Spell.OnUpgrade -= OnUpgrade;
        }
        _isSubscribed = false;
        base.Dispose();
    }
}

