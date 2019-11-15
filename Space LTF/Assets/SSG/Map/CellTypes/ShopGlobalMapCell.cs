using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    private readonly ShopInventory _shopInventory;
    public ShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData sector,ShipConfig config) : base(id, intX, intZ, sector)
    {
        _shopInventory = new ShopInventory(null);
        _shopInventory.FillItems(power, config);
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