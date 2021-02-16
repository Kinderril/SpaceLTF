using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopGlobalMapCell : GlobalMapCell
{
    public const int MAX_LEVEL = 3;
    public ShopInventory ShopInventory=>_shopInventory;      
    public int Level { get; private set; } = 1;
    public float CollectedMoney { get; private set; } = 0;


    protected ShopInventory _shopInventory;
    PlayerSafe _playerSafe = new PlayerSafe(false,SaveMode.none);
    public ShopGlobalMapCell(float power, int id, int intX, int intZ, SectorData sector, ShipConfig config) : base(id, intX, intZ, sector, config)
    {
        Level = 1;
        InitShop(power,sector,config);
    }

    protected void InitShop(float power, SectorData sector, ShipConfig config)
    {
        _shopInventory = new ShopInventory(_playerSafe);
        int lvl = 1;
        if (sector.XIndex > _sector.Size * 2)
        {
            lvl = 2;
        }
        else if (sector.XIndex > _sector.Size * 4)
        {
            lvl = 3;
        }

        Level = lvl;
        _shopInventory.FillItems(Level, config);
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

        if (rep == EReputationStatus.enemy && ConfigOwner != ShipConfig.droid && !Sector.IsMy)
        {
            var mesData = new MessageDialogData(Namings.Tag("dialog_shopEnemy"), new List<AnswerDialogData>
            {
                new AnswerDialogData(Namings.Tag("Ok"), null)
            });
            return mesData;
        }
        else
        {
            var ans = new List<AnswerDialogData>
            {
                new AnswerDialogData(Namings.Tag("Yes"), Take),
                new AnswerDialogData(Namings.Tag("No"), null)
            };
            var cost = (int)(Level * 40f);
            if (Sector.IsMy)
            {
                if (Level < MAX_LEVEL)
                {
                    var b = new AnswerDialogData(
                        Namings.Format(Namings.Tag("cellUpgradeShop"), cost, Level), null,
                        () => TryRequestUpgradeShop(cost, this));
                    ans.Add(b);
                }

                var a = new AnswerDialogData(Namings.Format(Namings.Tag("TakeReward"), (int)CollectedMoney), TakeReward);
                ans.Add(a);
            }
            var mesData = new MessageDialogData(Namings.Format(Namings.Tag("dialog_shopTrade"), Level), ans);
            return mesData;

        }


    }

    private MessageDialogData TryRequestUpgradeShop(int cost, ShopGlobalMapCell shop)
    {
        return TryRequestSmt(cost, UpgradeShop(shop), "armyBornCenterShopUpgraded");
    }


    private Action UpgradeShop(ShopGlobalMapCell shop)
    {
        var cell = shop;
        void Act()
        {
            cell.UpgradeLevel();
        }
        return Act;
    }

    private void TakeReward()
    {
        CollectedMoney = 0f;
        MainController.Instance.MainPlayer.MoneyData.AddMoney((int)CollectedMoney);
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
        return $"{Namings.Tag("Shop")} {Namings.Tag("Level")}:{Level}";
    }

    public override void Take()
    {
        WindowManager.Instance.OpenWindow(MainState.shop, _shopInventory);
    }

    public override void UpdateStep(int step)
    {
        base.UpdateStep(step);
        if (_sector.IsMy)
        {
            float perTurn = 0f;
            switch (Level)
            {
                case 1:
                    perTurn = 3 * MoneyConsts.CELL_MONEY_COEF;
                    break;
                case 2:
                    perTurn = 4 * MoneyConsts.CELL_MONEY_COEF;
                    break;
                case 3:
                    perTurn = 5 * MoneyConsts.CELL_MONEY_COEF;
                    break;
            }

            CollectedMoney += perTurn;
        }

    }

    public void UpgradeLevel()
    {
        if (Level < MAX_LEVEL)
        {
            _shopInventory.ClearShop();
            _shopInventory.FillItems(Level,ConfigOwner);
            Level++;
        }

    }
}