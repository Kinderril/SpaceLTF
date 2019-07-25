using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeWeaponUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI WeaponLevelField;
    public TextMeshProUGUI ShipName;
    public TextMeshProUGUI UpgradeCost;
    public TextMeshProUGUI MaxLevel;
    private WeaponInv _weapon;
    public Button UpgrdeButtonA1;
    public Button UpgrdeButtonB1;
    public TextMeshProUGUI BtnFieldA1;
    public TextMeshProUGUI BtnFieldB1;

    public void Init(WeaponInv inv, ShipInventory onwer = null)
    {
        _weapon = inv;
        Name.text = inv.Name;
        ShipName.text = onwer != null ? onwer.Name : "Inventory";
        CheckCanUpg();
        _weapon.OnUpgrade += OnUpgrade;
    }

    private void OnUpgrade(WeaponInv obj)
    {
        CheckCanUpg();
    }

    private void CheckCanUpg()
    {
        var canUpg = _weapon.CanUpgrade();
        WeaponLevelField.text = _weapon.Level.ToString();
        UpgrdeButtonA1.interactable = canUpg;
        UpgrdeButtonB1.interactable = canUpg;
//        var a1Txt = _weapon.GetDesc(WeaponUpdageType.a1,false);
//        var b1Txt = _weapon.GetDesc(WeaponUpdageType.b1, false);
//        BtnFieldA1.text = a1Txt;
//        BtnFieldB1.text = b1Txt;

        UpgrdeButtonA1.gameObject.SetActive(canUpg);
        UpgrdeButtonB1.gameObject.SetActive(canUpg);
        UpgradeCost.gameObject.SetActive(canUpg);
        MaxLevel.gameObject.SetActive(!canUpg);
        if (!canUpg)
        {
            MaxLevel.text = "Max level";
        }
        if (MoneyConsts.WeaponUpgrade.ContainsKey(_weapon.Level))
        {
            var cost = MoneyConsts.WeaponUpgrade[_weapon.Level];
            UpgradeCost.text = cost.ToString();
        }
        else
        {
            UpgradeCost.text = "max level";
        }
    }

    public void OnClickA1()
    {
        SubUpg(WeaponUpdageType.a1);
    }

    public void OnClickB1()
    {
        SubUpg(WeaponUpdageType.b1);
    }

    private void SubUpg(WeaponUpdageType upgType)
    {
        var owner = _weapon.CurrentInventory.Owner;
        if (_weapon.CanUpgrade())
        {
            if (MoneyConsts.WeaponUpgrade.ContainsKey(_weapon.Level))
            {
                var cost = MoneyConsts.WeaponUpgrade[_weapon.Level];
                if (owner.MoneyData.HaveMoney(cost))
                {
                    var txt = String.Format("You want to upgrade {0}",Namings.Weapon(_weapon.WeaponType));
                    WindowManager.Instance.ConfirmWindow.Init(()=>
                    {
                        owner.MoneyData.RemoveMoney(cost);
                        _weapon.Upgrade();
//                        WindowManager.Instance.InfoWindow.Init(null, "Upgrade completed");
                    },null, txt);
                }
                else
                {
                    WindowManager.Instance.NotEnoughtMoney(cost);
                }
            }
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null,"Weapon have max level");
        }
    }

    private void Unsubscibe()
    {
        _weapon.OnUpgrade -= OnUpgrade;
    }

    public void Dispose()
    {
        Unsubscibe();
    }

    void OnDestroy()
    {
        Unsubscibe();
    }
}

