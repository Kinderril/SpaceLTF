using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    private readonly ShopInventory _shopInventory;
    public ShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData sector, ShipConfig config) : base(id, intX, intZ, sector, config)
    {
        _shopInventory = new ShopInventory(null);
        _shopInventory.FillItems(power, config);
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    protected override MessageDialogData GetDialog()
    {
        var rep = MainController.Instance.MainPlayer.ReputationData.GetStatus(ConfigOwner);
        if (rep == EReputationStatus.enemy)
        {
            var mesData = new MessageDialogData(Namings.Tag("dialog_shopEnemy"), new List<AnswerDialogData>
            {
                new AnswerDialogData(Namings.Tag("Ok"), null)
            });
            return mesData;
        }
        else
        {

            var mesData = new MessageDialogData(Namings.Tag("dialog_shopTrade"), new List<AnswerDialogData>
            {
                new AnswerDialogData(Namings.Tag("Yes"), Take),
                new AnswerDialogData(Namings.Tag("No"), null)
            });
            return mesData;
        }

    }

    public override Color Color()
    {
        return new Color(153f / 255f, 255f / 255f, 204f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return false;
    }

    public override string Desc()
    {
        return Namings.Tag("Shop");
    }

    public override void Take()
    {
        WindowManager.Instance.OpenWindow(MainState.shop, _shopInventory);
    }
}