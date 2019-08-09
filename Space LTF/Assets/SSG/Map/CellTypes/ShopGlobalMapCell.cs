using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    private readonly ShopInventory _shopInventory;
    private const int MIN_ITEMS = 5;
    private const int MAX_ITEMS = 25;
    private const float MODULS_PERCENT = 0.4f; 
    private const float WEAPON_PERCENT = 0.4f; 
    private const float SPELL_PERCENT = 0.2f; 
    public ShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData secto) : base(id, intX, intZ, secto)
    {
        _shopInventory = new ShopInventory(null);
        int totalItems =(int)((power + 5) / 2);
        totalItems = Mathf.Clamp(totalItems, MIN_ITEMS, MAX_ITEMS);
        var weaponsCount = (int)(totalItems * WEAPON_PERCENT);
        var countModuls = (int)(totalItems * MODULS_PERCENT);
        var spells = (int)(totalItems * SPELL_PERCENT);

        bool goodPower = power > 22;
        for (var i = 0; i < weaponsCount; i++)
        {
            var w = Library.CreateWeapon(goodPower);
            w.CurrentInventory = _shopInventory;
            _shopInventory.Weapons.Add(w);
        }

       
        WDictionary<int> levels = new WDictionary<int>(new Dictionary<int, float>()
            {
                {1,4f},
                {2,4f},
                {3, 1f},
            });
        for (var i = 0; i < countModuls; i++)
        {
            var m = Library.CreatSimpleModul(levels.Random(),MyExtensions.IsTrueEqual());
            m.CurrentInventory = _shopInventory;
            _shopInventory.Moduls.Add(m);
        }

        for (int i = 0; i < spells; i++)
        {
            var s = Library.CreateSpell();
            s.CurrentInventory = _shopInventory;
            _shopInventory.Spells.Add(s);
        }
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("You see shop. Maybe you want buy something?", new List<AnswerDialogData>
        {
            new AnswerDialogData("Yes", Take),
            new AnswerDialogData("No", null)
        });
        return mesData;
    }

    public override Color Color()
    {
        return new Color(153f/255f, 255f/255f, 204f/255f);
    }

    public override bool OneTimeUsed()
    {
        return false;
    }

    public override string Desc()
    {
        return "Shop";
    }

    public override void Take()
    {
        WindowManager.Instance.OpenWindow(MainState.shop, _shopInventory);
    }
}