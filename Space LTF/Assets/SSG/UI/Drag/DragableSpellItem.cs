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
            MainController.Instance.MainPlayer.MoneyData.OnMoneyChange += OnMoneyChange;
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
    protected override string TextToTooltip()
    {
        return $"{Spell.GetInfo()} ({Spell.Level.ToString()})";
    }
    public void OnTryUpgradeClick()
    {
        if (Usable)
            Spell.TryUpgrade();
    }
    private void OnUpgrade(BaseSpellModulInv obj)
    {
        if (Spell == null)
        {
            return;
        }
        var cost = MoneyConsts.WeaponUpgrade[Spell.Level];
        var haveMoney = MainController.Instance.MainPlayer.MoneyData.HaveMoney(cost);
        var isMy = MainController.Instance.MainPlayer == Spell.CurrentInventory.Owner;
        var canUse = Spell.CanUpgrade() && haveMoney && Usable && isMy;
        UpgradeButton.gameObject.SetActive(canUse);

    }

    protected override void OnClickComplete()
    {
        WindowManager.Instance.ItemInfosController.Init(Spell, CanShowWindow());
    }

    protected override void Dispose()
    {
        MainController.Instance.MainPlayer.MoneyData.OnMoneyChange -= OnMoneyChange;
        if (Spell != null)
            Spell.OnUpgrade -= OnUpgrade;
        _isSubscribed = false;
        base.Dispose();
    }
}

