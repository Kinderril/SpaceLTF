using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public  class DebugShopCatcher : MonoBehaviour
{
    DebugStartBattleWindow _debugStartBattleWindow;
    public ButtonWithText PrefabButton;
    public Transform Layout;

    public void Init(DebugStartBattleWindow debugStartBattleWindow)
    {
        _debugStartBattleWindow = debugStartBattleWindow;
        InitBodies();
        InitModuls();
        InitWeapons();
        InitSpells();
    }

    private void InitBodies()
    {
        foreach (ShipType s in (ShipType[])Enum.GetValues(typeof(ShipType)))
        {
            var b = DataBaseController.GetItem(PrefabButton);
            b.Background.color = Color.red;
            b.transform.SetParent(Layout, false);
            b.Text.text = s.ToString();
            var shipType = s;
            b.Button.onClick.AddListener(() =>
            {
                _debugStartBattleWindow.SetBodyType(shipType,ShipConfig.mercenary);
            });
        }
    }

    private void InitModuls()
    {
        foreach (SimpleModulType s in (SimpleModulType[])Enum.GetValues(typeof(SimpleModulType)))
        {
            var b = DataBaseController.GetItem(PrefabButton);
            b.Background.color = new Color(0.6f,0.6f,0.9f,1f);
            b.transform.SetParent(Layout, false);
            b.Text.text = s.ToString();
            var ModulSlot = s;
            b.Button.onClick.AddListener(() =>
            {
                var slot = Library.CreatSimpleModul(ModulSlot,2);
                _debugStartBattleWindow.SetModul(slot);
            });
        }
    }

    private void InitWeapons()
    {
        var weapons = new List<WeaponType>()
        {
            WeaponType.laser,WeaponType.impulse,WeaponType.casset,
            WeaponType.rocket,WeaponType.eimRocket,WeaponType.beam
        };
        foreach (WeaponType s in weapons)
        {
            var b = DataBaseController.GetItem(PrefabButton);
            b.Background.color = Color.yellow;
            b.transform.SetParent(Layout, false);
            b.Text.text = s.ToString();
            var WeaponType1 = s;
            b.Button.onClick.AddListener(() =>
            {
                var weapon = Library.CreateWeaponByType(WeaponType1);
                _debugStartBattleWindow.SetWeapon(weapon);
            });
        }
    }

    private void InitSpells()
    {
        foreach (SpellType s in (SpellType[])Enum.GetValues(typeof(SpellType)))
        {
            var b = DataBaseController.GetItem(PrefabButton);
            b.Background.color = Color.cyan;
            b.transform.SetParent(Layout, false);
            b.Text.text = s.ToString();
            var WeaponType = s;
            b.Button.onClick.AddListener(() =>
            {
                var weapon = Library.CreateSpell(WeaponType);
                _debugStartBattleWindow.SetSpell(weapon);
            });
        }
    }

//    public void OnClickBody(string info)
//    {
//        ShipType shipType = (ShipType)Enum.Parse(typeof(ShipType), info);
//        _debugStartBattleWindow.SetBodyType(shipType);
//    }
//
//    public void OnClickModul(string info)
//    {
//        ModulSlot ModulSlot = (ModulSlot)Enum.Parse(typeof(ModulSlot), info);
//        var slot = Library.CreatSimpleModul(ModulSlot);
//        _debugStartBattleWindow.SetModul(slot);
//    }
//
//    public void OnClickWeapon(string info)
//    {
//        WeaponType WeaponType = (WeaponType)Enum.Parse(typeof(WeaponType), info);
//        var weapon = Library.CreateWeapon(WeaponType, TeamIndex.red);
//        _debugStartBattleWindow.SetWeapon(weapon);
//    }
}

