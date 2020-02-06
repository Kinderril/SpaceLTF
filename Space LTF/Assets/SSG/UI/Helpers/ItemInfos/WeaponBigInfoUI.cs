using System;
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
    public SliderWithTextMeshPro ShootPerTime;

    public Transform Layout;

    public GameObject ButtonContainer;
    public Button ButtonUpgrade;
    public TextMeshProUGUI MaxLevel;
    public TextMeshProUGUI ReqireLevelFeild;
    public MoneySlotUI UpgradeCost;
    private WeaponUIParams dataModif;
    private bool _withModul;


    public void Init(WeaponInv inv, Action callback, bool canClick, bool withModul)
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
        DamageHP.InitBorders(0, 20, true);
        DamageShield.InitBorders(0, 20, true);
        RadiuesField.InitBorders(0, 15, true);
        AngField.InitBorders(0, 180, true);
        ReloadField.InitBorders(1, 20, true);
        BulletSpeedField.InitBorders(0, 20, true);
        ShootPerTime.InitBorders(0, 4, true);

        RadiuesField.InitName(Namings.Tag("Radius"));
        AngField.InitName(Namings.Tag("Sector"));
        ReloadField.InitName(Namings.Tag("Reload"));
        BulletSpeedField.InitName(Namings.Tag("Speed"));
        DamageHP.InitName(Namings.Tag("DamageBody"));
        DamageShield.InitName(Namings.Tag("DamageShield"));
        ShootPerTime.InitName(Namings.Tag("ShootPerTime"));
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
        ReqireLevelFeild.text = Namings.TryFormat(Namings.Tag("ReqireLevelFeild"), _weapon.RequireLevel());
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
        ShootPerTime.Slider.value = modif.ShootPerTime;
    }

    private void DrawModuls()
    {
        dataModif = new WeaponUIParams(_weapon.CurrentDamage,
            _weapon.AimRadius, _weapon.SetorAngle, _weapon.BulletSpeed, _weapon.ReloadSec, _weapon.ShootPerTime);
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
            WeaponLevelField.text = Namings.Tag("WeaponMaxLevel");
        }
        else
        {
            WeaponLevelField.text = Namings.Tag("Level") + ":" + _weapon.Level.ToString();
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

