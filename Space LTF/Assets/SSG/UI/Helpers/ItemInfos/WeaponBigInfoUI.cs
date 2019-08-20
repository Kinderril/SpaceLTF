﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponBigInfoUI : AbstractBaseInfoUI
{
    public RectTransform MainLayout;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI WeaponLevelField;
//    public TextMeshProUGUI DamageField;
    public TextMeshProUGUI PrefabText;
    private WeaponInv _weapon;

    public SliderWithTextMeshPro DamageHP;
    public SliderWithTextMeshPro DamageShield;
    public SliderWithTextMeshPro RadiuesField;
    public SliderWithTextMeshPro AngField;
    public SliderWithTextMeshPro ReloadField;       
    public SliderWithTextMeshPro BulletSpeedField;       

    public Transform Layout;

    public GameObject ButtonContainer;
    public Button ButtonUpgrade;
    public TextMeshProUGUI MaxLevel;
    public MoneySlotUI UpgradeCost;
    private WeaponUIParams dataModif;
    private bool _withModul;


    public void Init(WeaponInv inv, Action callback,bool canClick,bool withModul)
    {
        base.Init(callback);
        _withModul = withModul;

        ButtonUpgrade.interactable = canClick;

        _weapon = inv;
        Name.text = inv.Name;
//        ShipName.text = onwer != null ? onwer.Name : "Inventory";
        CheckCanUpg();
        _weapon.OnUpgrade += OnUpgrade;
        DrawModuls();
        DrawLevel();


        LayoutRebuilder.ForceRebuildLayoutImmediate(MainLayout);
    }

    private void DrawParams(IAffectParameters modif)
    {
        DamageHP.InitBorders(0, 10, true);
        DamageShield.InitBorders(0, 10, true);
        RadiuesField.InitBorders(0, 15, true);
        AngField.InitBorders(0, 180, true);
        ReloadField.InitBorders(1, 20, true);
        BulletSpeedField.InitBorders(0, 20, true);

        RadiuesField.InitName("Radius");
        AngField.InitName("Sector");
        ReloadField.InitName("Reload");
        BulletSpeedField.InitName("Speed");
        DamageHP.InitName("Damage body");
        DamageShield.InitName("Damage shield");
        DrawCurrentUpgrades(modif);
    }

    private void DrawLevel()
    {
        var canUpgrade = _weapon.CanUpgrade();
        ButtonContainer.gameObject.SetActive(canUpgrade);
        MaxLevel.gameObject.SetActive(!canUpgrade);
        if (canUpgrade)
        {
            var cost = MoneyConsts.WeaponUpgrade[_weapon.Level];
            UpgradeCost.Init(cost);
        }
    }

    public void OnClickUpgrade()
    {
        _weapon.TryUpgrade();
    }

    private void DrawCurrentUpgrades(IAffectParameters modif)
    {
//        var dataMain = new WeaponUIParams(_weapon.CurrentDamage, _weapon.AimRadius, _weapon.SetorAngle, _weapon.BulletSpeed);
//        DamageField.text = $"Damage. Shield:{modif.CurrentDamage.ShieldDamage}  Body:{modif.CurrentDamage.BodyDamage}";
        RadiuesField.Slider.value = modif.AimRadius;
        AngField.Slider.value = modif.SetorAngle;
        ReloadField.Slider.value = modif.ReloadSec;
        BulletSpeedField.Slider.value = modif.BulletSpeed;
        DamageHP.Slider.value = modif.CurrentDamage.BodyDamage;
        DamageShield.Slider.value = modif.CurrentDamage.ShieldDamage;
    }

    private void DrawModuls()
    {
        dataModif = new WeaponUIParams(_weapon.CurrentDamage,
            _weapon.AimRadius, _weapon.SetorAngle, _weapon.BulletSpeed, _weapon.ReloadSec);
        Layout.ClearTransform();
        if (!_withModul)
        {
            Layout.gameObject.SetActive(false);
            DrawParams(dataModif);
            return;
        }
        var allItems = _weapon.CurrentInventory.GetAllItems();
        bool haveModuls = false;
        foreach (var item in allItems)
        {
            var support = item as BaseSupportModul;
            if (support != null)
            {
                support.ChangeParams(dataModif);
                haveModuls = true;
                var supportField = DataBaseController.GetItem(PrefabText);
                supportField.transform.SetParent(Layout);
                supportField.text = support.DescSupport();
            }
        }
        Layout.gameObject.SetActive(haveModuls);
        DrawParams(dataModif);
    }

    private void OnUpgrade(WeaponInv obj)
    {
        DrawModuls();
        CheckCanUpg();
        DrawLevel();
        DrawCurrentUpgrades(dataModif);
    }

    private void CheckCanUpg()
    {
        var canUpg = _weapon.CanUpgrade();
        WeaponLevelField.text = _weapon.Level.ToString();
        if (!canUpg)
        {
            WeaponLevelField.text = Namings.WeaponMaxLevel;
        }
        else
        {
            WeaponLevelField.text = Namings.Level + ":" + _weapon.Level.ToString();
        }
        var canUpgrade = MoneyConsts.WeaponUpgrade.ContainsKey(_weapon.Level);
//        WeaponLevelField.gameObject.SetActive(!canUpgrade);

        if (canUpgrade)
        {
        }
        else
        {
        }
    }

    private void Unsubscibe()
    {
        _weapon.OnUpgrade -= OnUpgrade;
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

