﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShopInventory : PlayerInventory
{
    private const int MIN_WEAPONS = 3;
    private const int MAX_WEAPONS = 6;
    private const int MIN_MODULS = 3;
    private const int Max_MODULS = 4; 
    private const int MIN_PARAMETER = 2;
    private const int Max_PARAMETER = 4;
    private const int MIN_SPELLS = 1;
    private const int Max_SPELLS = 2;
    private const float CHANCE_SUPPORT_1 = .4f;

    private const float VALUABLE_COEF = 1.65f;
    private const float UNVALUABLE_COEF = 0.4f;

    private List<WeaponType> _valuableTypesWeapon = new List<WeaponType>();
    private List<WeaponType> _notValuableTypesWeapon = new List<WeaponType>();
    private List<SimpleModulType> _valuableTypesModuls = new List<SimpleModulType>();
    private List<SimpleModulType> _notValuableTypesModuls = new List<SimpleModulType>();

    public List<WeaponType> ValuableTypesWeaponList => _valuableTypesWeapon;
    public List<WeaponType> NotValuableTypesWeaponList => _notValuableTypesWeapon;
    public List<SimpleModulType> ValuableTypesModulsList => _valuableTypesModuls;
    public List<SimpleModulType> NotValuableTypesModulsList => _notValuableTypesModuls;

    public ShopInventory([NotNull] PlayerSafe player)
        : base(player,30)
    {

    }

    public override bool CanRemoveModulSlots(int slotsInt)
    {
        return true;
    }

    public static List<WeaponType> ValuableTypesWeapon(ShipConfig config)
    {
        List<WeaponType> valuableTypesWeapon = new List<WeaponType>();
        switch (config)
        {
            case ShipConfig.raiders:
                valuableTypesWeapon.Add(WeaponType.rocket);
                valuableTypesWeapon.Add(WeaponType.casset);
                break;
            case ShipConfig.federation:
                valuableTypesWeapon.Add(WeaponType.eimRocket);
                valuableTypesWeapon.Add(WeaponType.laser);
                break;
            case ShipConfig.mercenary:
                valuableTypesWeapon.Add(WeaponType.rocket);
                valuableTypesWeapon.Add(WeaponType.laser);
                break;
            case ShipConfig.ocrons:
                valuableTypesWeapon.Add(WeaponType.beam);
                valuableTypesWeapon.Add(WeaponType.casset);
                valuableTypesWeapon.Add(WeaponType.laser);
                break;
            case ShipConfig.krios:
                valuableTypesWeapon.Add(WeaponType.impulse);
                valuableTypesWeapon.Add(WeaponType.eimRocket);
                break;
            case ShipConfig.droid:
                valuableTypesWeapon.Add(WeaponType.rocket);
                valuableTypesWeapon.Add(WeaponType.casset);
                break;
        }

        return valuableTypesWeapon;
    }
    public static List<SimpleModulType> ValuableTypesModuls(ShipConfig config)
    {
        List<SimpleModulType> valuableTypesWeapon = new List<SimpleModulType>();
        switch (config)
        {
            case ShipConfig.raiders:
                valuableTypesWeapon.Add(SimpleModulType.WeaponFire);
                valuableTypesWeapon.Add(SimpleModulType.fireMines);
                valuableTypesWeapon.Add(SimpleModulType.damageMines);
                break;
            case ShipConfig.federation:
                valuableTypesWeapon.Add(SimpleModulType.shieldLocker);
                valuableTypesWeapon.Add(SimpleModulType.frontShield);
                valuableTypesWeapon.Add(SimpleModulType.WeaponShieldIgnore);
                break;
            case ShipConfig.mercenary:
                valuableTypesWeapon.Add(SimpleModulType.ShipSpeed);
                valuableTypesWeapon.Add(SimpleModulType.ShipTurnSpeed);
                valuableTypesWeapon.Add(SimpleModulType.WeaponDist);
                break;
            case ShipConfig.ocrons:
                valuableTypesWeapon.Add(SimpleModulType.shieldRegen);
                valuableTypesWeapon.Add(SimpleModulType.WeaponCrit);
                valuableTypesWeapon.Add(SimpleModulType.WeaponLessDist);
                break;
            case ShipConfig.krios:
                valuableTypesWeapon.Add(SimpleModulType.WeaponPowerShot);
                valuableTypesWeapon.Add(SimpleModulType.WeaponSelfDamage);
                valuableTypesWeapon.Add(SimpleModulType.WeaponSpray);
                break;
        }

        return valuableTypesWeapon;
    }
    public static List<SimpleModulType> NotValuableTypesModuls(ShipConfig config)
    {
        List<SimpleModulType> valuableTypesWeapon = new List<SimpleModulType>();
        switch (config)
        {
            case ShipConfig.raiders:
                valuableTypesWeapon.Add(SimpleModulType.WeaponCrit);
                valuableTypesWeapon.Add(SimpleModulType.WeaponLessDist);
                valuableTypesWeapon.Add(SimpleModulType.WeaponShieldIgnore);
                break;
            case ShipConfig.federation:
                valuableTypesWeapon.Add(SimpleModulType.ShipSpeed);
                valuableTypesWeapon.Add(SimpleModulType.ShipTurnSpeed);
                valuableTypesWeapon.Add(SimpleModulType.WeaponDist);
                break;
            case ShipConfig.mercenary:
                valuableTypesWeapon.Add(SimpleModulType.WeaponPowerShot);
                valuableTypesWeapon.Add(SimpleModulType.WeaponSelfDamage);
                valuableTypesWeapon.Add(SimpleModulType.WeaponSpray);
                break;
            case ShipConfig.ocrons:
                valuableTypesWeapon.Add(SimpleModulType.damageMines);
                valuableTypesWeapon.Add(SimpleModulType.shieldRegen);
                valuableTypesWeapon.Add(SimpleModulType.frontShield);
                break;
            case ShipConfig.krios:
                valuableTypesWeapon.Add(SimpleModulType.WeaponFire);
                valuableTypesWeapon.Add(SimpleModulType.fireMines);
                valuableTypesWeapon.Add(SimpleModulType.shieldLocker);
                break;
        }

        return valuableTypesWeapon;
    }
    public static List<WeaponType> NotValuableTypesWeapon(ShipConfig config)
    {
        List<WeaponType> notValuableTypesWeapon = new List<WeaponType>();
        switch (config)
        {
            case ShipConfig.raiders:
                notValuableTypesWeapon.Add(WeaponType.beam);
                notValuableTypesWeapon.Add(WeaponType.eimRocket);
                break;
            case ShipConfig.federation:
                notValuableTypesWeapon.Add(WeaponType.beam);
                notValuableTypesWeapon.Add(WeaponType.casset);
                break;
            case ShipConfig.mercenary:
                notValuableTypesWeapon.Add(WeaponType.beam);
                notValuableTypesWeapon.Add(WeaponType.eimRocket);
                notValuableTypesWeapon.Add(WeaponType.impulse);
                break;
            case ShipConfig.ocrons:
                notValuableTypesWeapon.Add(WeaponType.impulse);
                notValuableTypesWeapon.Add(WeaponType.eimRocket);
                break;
            case ShipConfig.krios:
                notValuableTypesWeapon.Add(WeaponType.beam);
                notValuableTypesWeapon.Add(WeaponType.rocket);
                notValuableTypesWeapon.Add(WeaponType.casset);
                break;
        }

        return notValuableTypesWeapon;
    }

    public override bool IsShop()
    {
        return true;
    }

    public void FillItems(int level, ShipConfig config)
    {
        try
        {

            //        int totalItems = (int)((power + 5) / 2);
            //        totalItems = Mathf.Clamp(totalItems, MIN_ITEMS, MAX_ITEMS);
            var weapLevelCoef = Mathf.Clamp(level * 0.8f,1,3);
            var weaponsCount = MyExtensions.Random(MIN_WEAPONS * weapLevelCoef, MAX_WEAPONS * weapLevelCoef);
            var clampedIndex = Mathf.Clamp(level * 1.5f,0, 7 );
            var baseModuls = MyExtensions.Random(MIN_MODULS, Max_MODULS);
            var parameterItems = MyExtensions.Random(MIN_PARAMETER, Max_PARAMETER);

            var countModuls = baseModuls + clampedIndex;
//            Debug.LogError($"Moduls;{countModuls}    baseModuls:{baseModuls}   intdex:{clampedIndex}    intdex:{intdex}");
            List<EParameterItemSubType> itemsTypes = Library.ParameterItemTypes;
            WDictionary<EParameterItemRarity> rarities = Library.GetParitiesCacheShop;
            for (int i = 0; i < parameterItems; i++)
            {
                var paramType = itemsTypes.RandomElement();
                var raruty = rarities.Random();
                var paramItem = Library.CreateParameterItem(paramType, raruty);
                paramItem.CurrentInventory = this;
                ParamItems.Add(paramItem);
            }
            var spells = MyExtensions.Random(MIN_SPELLS, Max_SPELLS);
            var dd = new Dictionary<int, float>();
            switch (level)
            {
                case 2:
                    dd.Add(1,3);
                    dd.Add(2,4);
                    dd.Add(3,3);
                    dd.Add(4,1);
                    break;
                case 3:
                    dd.Add(2, 3);
                    dd.Add(3, 2);
                    dd.Add(4, 1);
                    break;
                default:
                    dd.Add(1, 5);
                    dd.Add(2, 4);
                    dd.Add(3, 1);
                    break;
            }
            WDictionary<int> levle = new WDictionary<int>(dd);

            for (var i = 0; i < weaponsCount; i++)
            {
                var w = Library.CreateDamageWeapon(levle.Random());
                w.CurrentInventory = this;
                Weapons.Add(w);
            }

            if (MyExtensions.IsTrue01(CHANCE_SUPPORT_1))
            {
                var w = Library.CreateSupportWeapon(1);
                w.CurrentInventory = this;
                Weapons.Add(w);
            }


            WDictionary<int> levels = new WDictionary<int>(new Dictionary<int, float>()
            {
                {1,5f},
                {2,4f},
                {3, 3f},
                {4, 2f},
                {5, 2f},
            });
            for (var i = 0; i < countModuls; i++)
            {
                var m = Library.CreatSimpleModul(levels.Random());
                m.CurrentInventory = this;
                Moduls.Add(m);
            }

            for (int i = 0; i < spells; i++)
            {
                var s = Library.CreateSpell();
                s.CurrentInventory = this;
                Spells.Add(s);
            }

            _valuableTypesWeapon = ShopInventory.ValuableTypesWeapon(config);
            _notValuableTypesWeapon = ShopInventory.NotValuableTypesWeapon(config);
            _valuableTypesModuls = ValuableTypesModuls(config);
            _notValuableTypesModuls = NotValuableTypesModuls(config);
        }
        catch (Exception e)
        {
            Debug.LogError($"Fill itesm error: {e}");
        }
    }
    public void ClearShop()
    {
        Weapons.Clear();
        Moduls.Clear();
        Spells.Clear();
        ParamItems.Clear();
    }
    public void AddItem(WeaponType type)
    {
        var w = Library.CreateWeaponByType(type);
        w.CurrentInventory = this;
        Weapons.Add(w);
    }



    public override float ValuableItem(IItemInv item)
    {
        bool isValuable = false;
        switch (item.ItemType)
        {
            case ItemType.weapon:
                var weapon = item as WeaponInv;
                if (weapon != null)
                {
                    isValuable = _valuableTypesWeapon.Contains(weapon.WeaponType);
                    if (isValuable)
                    {
                        return VALUABLE_COEF;
                    }
                    isValuable = _notValuableTypesWeapon.Contains(weapon.WeaponType);
                    if (isValuable)
                    {
                        return UNVALUABLE_COEF;
                    }
                }

                break;
            case ItemType.modul:
                var modul = item as BaseModulInv;
                if (modul != null)
                {
                    isValuable = _valuableTypesModuls.Contains(modul.Type);
                    if (isValuable)
                    {
                        return VALUABLE_COEF;
                    }
                    isValuable = _notValuableTypesModuls.Contains(modul.Type);
                    if (isValuable)
                    {
                        return UNVALUABLE_COEF;
                    }
                }
                break;
            case ItemType.spell:
                break;
        }

        return 1f;
    }

}

