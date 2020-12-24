using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    public ShopInventory ShopInventory=>_shopInventory;
    protected ShopInventory _shopInventory;
    PlayerSafe _playerSafe = new PlayerSafe(false,SaveMode.none);
    public ShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData sector, ShipConfig config) : base(id, intX, intZ, sector, config)
    {
        InitShop(power,sector,config);
    }

    protected virtual void InitShop(float power, SectorData sector, ShipConfig config)
    {
        _shopInventory = new ShopInventory(_playerSafe);
        _shopInventory.FillItems(power, config, sector.XIndex);
    }

    public void ClearShop()
    {
        _shopInventory.ClearShop();

    }
    public void AddItem(WeaponType type)
    {
        _shopInventory.AddItem(type);
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    protected override MessageDialogData GetDialog()
    {
        var rep = MainController.Instance.MainPlayer.ReputationData.GetStatus(ConfigOwner);

        if (rep == EReputationStatus.enemy && ConfigOwner != ShipConfig.droid && !Sector.IsSectorMy)
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