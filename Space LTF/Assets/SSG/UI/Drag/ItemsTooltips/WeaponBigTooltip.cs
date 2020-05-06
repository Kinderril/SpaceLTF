using UnityEngine;
using System.Collections;
using TMPro;

public class WeaponBigTooltip : ItemBigTooltip
{
    public TextMeshProUGUI DamageHP;
    public TextMeshProUGUI DamageShield;
    public TextMeshProUGUI RadiuesField;
    public TextMeshProUGUI AngField;
    public TextMeshProUGUI ReloadField;
    public TextMeshProUGUI BulletSpeedField;
    public TextMeshProUGUI ShootPerTime;
//    public GameObject ReloadIncrease;
    private WeaponInv _weapon;
    public TextMeshProUGUI ReqireLevelFeild;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    private bool _reloadIncrease;
    public void Init(WeaponInv inv, int? sellCost, GameObject causeTransform)
    {
        _weapon = inv;
        Name.text = inv.Name;
        var originData = new WeaponUIParams(_weapon.CurrentDamage,
            _weapon.AimRadius, _weapon.SetorAngle, _weapon.BulletSpeed, _weapon.ReloadSec, _weapon.ShootPerTime);
        var dataModif = CollectModif();
        DrawParams(originData,dataModif);
        SetSellCost(sellCost,inv);
        Init(causeTransform);
        Level.text = $"{Namings.Tag("Level")}: {inv.Level}";
    }




    private WeaponUIParams CollectModif()
    {
        var dataModif = new WeaponUIParams(_weapon.CurrentDamage,
            _weapon.AimRadius, _weapon.SetorAngle, _weapon.BulletSpeed, _weapon.ReloadSec, _weapon.ShootPerTime);

        if (!(_weapon.CurrentInventory is ShipInventory) )
        {
            return dataModif;
        }

        var allItems = _weapon.CurrentInventory.GetAllItems();
//        bool haveModuls = false;
        bool haveDamageWeapons = false, haveSupportWeapons = false;
        foreach (var item in allItems)
        {
            var support = item as BaseSupportModul;
            var notSupport = item is BaseActionModul;
            if (support != null && !notSupport)
            {
                support.ChangeParams(dataModif);
//                haveModuls = true;
            }

            var isSupport = item is SupportWeaponInv;
            if (isSupport)
            {
                haveSupportWeapons = true;
            }
            var isDamage = item is DamageWeaponInv;
            if (isDamage)
            {
                haveDamageWeapons = true;
            }
        }
        _reloadIncrease = (haveSupportWeapons && haveDamageWeapons);
        return (dataModif);
    }

    private void DrawParams(WeaponUIParams origin, WeaponUIParams modif)
    {

        DrawField(RadiuesField, Namings.Tag("Radius"), origin.AimRadius, modif.AimRadius);
        DrawField(AngField, Namings.Tag("Sector"), origin.SetorAngle, modif.SetorAngle);
        var reloadOrigin = _reloadIncrease ? Library.RELOAD_COEF_DIF_WEAPONS * origin.ReloadSec : origin.ReloadSec;
        var reloadModif = _reloadIncrease ? Library.RELOAD_COEF_DIF_WEAPONS * modif.ReloadSec : modif.ReloadSec;

        DrawField(ReloadField, Namings.Tag("Reload"), reloadOrigin, reloadModif);
        DrawField(BulletSpeedField, Namings.Tag("BulletSpeed"), origin.BulletSpeed, modif.BulletSpeed);
        var dmgBodyName = _weapon is SupportWeaponInv ? Namings.Tag("HealBody") : Namings.Tag("DamageBody");
        var dmgShieldName = _weapon is SupportWeaponInv ? Namings.Tag("HealShield") : Namings.Tag("DamageShield");

        DrawField(DamageHP, dmgBodyName, origin.CurrentDamage.BodyDamage, modif.CurrentDamage.BodyDamage);
        DrawField(DamageShield, dmgShieldName, origin.CurrentDamage.ShieldDamage, modif.CurrentDamage.ShieldDamage);

        DrawField(ShootPerTime, Namings.Tag("ShootPerTime"), origin.ShootPerTime, modif.ShootPerTime);
//        DrawField(RadiuesField, Namings.Tag("Radius"), origin.AimRadius, modif.AimRadius);
//        DrawField(RadiuesField, Namings.Tag("Radius"), origin.AimRadius, modif.AimRadius);


        ReqireLevelFeild.text = Namings.Format(Namings.Tag("ReqireLevelFeild"), _weapon.RequireLevel());
    }

    private void DrawField(TextMeshProUGUI field, string name, float origin, float modif)
    {
        var delta = modif - origin ;
        string modifStr = "";
        string deltaStr;
        string modifStr1;
        if (delta > 0)
        {
            deltaStr = ParameterItemBigInfoUI.GetVal(delta);
            modifStr1 = ParameterItemBigInfoUI.GetVal(modif);
            modifStr = $"{modifStr1} (+{deltaStr})";
        }
        else if (delta < 0)
        {
            deltaStr = ParameterItemBigInfoUI.GetVal(delta);
            modifStr1 = ParameterItemBigInfoUI.GetVal(modif);
            modifStr = $"{modifStr1} (-{(deltaStr)})";
        }
        else
        {
            modifStr1 = ParameterItemBigInfoUI.GetVal(modif);
            modifStr = modifStr1;
        }

        field.text = $"{name}: {modifStr}";
    }

}
