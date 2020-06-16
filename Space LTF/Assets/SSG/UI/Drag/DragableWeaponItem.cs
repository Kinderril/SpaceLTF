using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DragableWeaponItem : DragableItem 
{
    public WeaponInv Weapon { get { return ContainerItem as WeaponInv;} }
    public TextMeshProUGUI LevelField;
    public Button UpgradeButton;
    private bool _isSubscribed = false;



    protected override void Init()
    {
        if (!_isSubscribed)
        {
            ContainerItem.CurrentInventory.Owner.OnCreditsChange += OnMoneyChange;
            Weapon.OnUpgrade += OnUpgrade;
            _isSubscribed = true;
        }
        else
        {
            Debug.LogError("try to _isSubscribed Second time DragableWeaponItem");
        }
        base.Init();
        OnUpgrade(Weapon);
    }

    private void OnMoneyChange(int obj)
    {
        OnUpgrade(Weapon);
    }

    protected override void Refresh()
    {
        OnUpgrade(Weapon);
        base.Refresh();
    }

    protected override void Dispose()
    {
        if (Weapon != null)
        {
            ContainerItem.CurrentInventory.Owner.OnCreditsChange -= OnMoneyChange;
            Weapon.OnUpgrade -= OnUpgrade;
        }
        _isSubscribed = false;
        base.Dispose();
    }

    private void OnUpgrade(WeaponInv obj)
    {
        if (Weapon == null)
        {
            return;
        }
        var cost = MoneyConsts.WeaponUpgrade[Weapon.Level];
        var haveMoney = ContainerItem.CurrentInventory.Owner.HaveMoney(cost);
        var isMy = ContainerItem.CurrentInventory.Owner == Weapon.CurrentInventory.Owner;
        var canUse = Weapon.CanUpgrade() && haveMoney && Usable && isMy;
        try
        { 
            UpdateLevelsInfo(Weapon.Level);
            UpgradeButton.gameObject.SetActive(canUse);
        }
        catch (Exception e)
        {
            Debug.LogError("Upgrade button error");

        }

    }

    public void OnTryUpgradeClick()
    {        
        if (Usable)
            Weapon.TryUpgrade();
    }
    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(Weapon.WeaponType);
    }
    public override string GetInfo()
    {
        LevelField.text = Weapon.Level.ToString();
        return Weapon.GetInfo();
    }

    protected override void OnClickComplete()
    {
        var shipInv = (Weapon.CurrentInventory as ShipInventory) != null;
//        if (CanShowWindow())
            WindowManager.Instance.ItemInfosController.Init(Weapon, CanShowWindow(), shipInv);
    }
}

