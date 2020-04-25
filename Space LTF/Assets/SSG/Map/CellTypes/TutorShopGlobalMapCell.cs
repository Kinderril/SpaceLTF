using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TutorShopGlobalMapCell : ShopGlobalMapCell
{
    public TutorShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData sector, ShipConfig config) 
        : base(power, id, intX, intZ, sector, config)
    {
    }


    protected virtual void InitShop(float power, SectorData sector, ShipConfig config)
    {
        _shopInventory = new ShopTutorInventory(null);
        _shopInventory.FillItems(power, config, sector.XIndex);
    }
    protected override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData(Namings.Tag("dialog_shopTrade"), new List<AnswerDialogData>
        {
            new AnswerDialogData(Namings.Tag("Yes"), Take),
        });
        return mesData;

    }

    public override void Take()
    {
        WindowManager.Instance.OpenWindow(MainState.shop, _shopInventory);
    }
}
