using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    private readonly ShopInventory _shopInventory;

    public ShopGlobalMapCell(bool goodPower, int id, int intX, int intZ, SectorData secto) : base(id, intX, intZ, secto)
    {
        _shopInventory = new ShopInventory(null);
        var weaponsCount = MyExtensions.Random(1, 3);
        for (var i = 0; i < weaponsCount; i++)
        {
            var w = Library.CreateWeapon(goodPower);
            w.CurrentInventory = _shopInventory;
            _shopInventory.Weapons.Add(w);
        }

        var minModuls = 1;
        if (weaponsCount == 0)
        {
            minModuls = 3;
        }
        var countModuls = MyExtensions.Random(minModuls, 6);
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
        var spells = MyExtensions.Random(0, 2);

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