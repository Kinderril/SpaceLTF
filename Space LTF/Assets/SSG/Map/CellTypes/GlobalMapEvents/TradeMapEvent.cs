

using System;
using System.Collections.Generic;
using UnityEngine;

public enum TradeType
{
    traderSell,
    traderBuy,
}

[System.Serializable]
public class TradeMapEvent : BaseGlobalMapEvent
{
    private IItemInv _itemsToTrade;
    private PlayerInventory _inventory;
    private TradeType _TradeType;
    private int _cost;

    public override string Desc()
    {
        return "Trader";
    }

    public TradeMapEvent(ShipConfig config)
        : base(config)
    {
        _inventory = new PlayerInventory(new Player("trader"));
    }

    public override MessageDialogData GetDialog()
    {

        var inventory = MainController.Instance.MainPlayer.Inventory;

        if ((inventory.Moduls.Count > 0) || (inventory.Weapons.Count > 0))
        {
            _TradeType = MyExtensions.IsTrueEqual() ? TradeType.traderBuy : TradeType.traderSell;
        }
        else
        {
            _TradeType = TradeType.traderSell;
        }

        if (_TradeType == TradeType.traderSell)
        {
            List<ItemType> types = new List<ItemType>() { ItemType.modul, ItemType.weapon, ItemType.spell };
            switch (types.RandomElement())
            {
                case ItemType.weapon:
                    _itemsToTrade = Library.CreateWeapon(MyExtensions.IsTrueEqual());
                    break;
                case ItemType.modul:
                    var list = new List<int>() { 1, 2 };
                    _itemsToTrade = Library.CreatSimpleModul(list.RandomElement(), 3);
                    break;
                case ItemType.spell:
                    _itemsToTrade = Library.CreateSpell();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _itemsToTrade.CurrentInventory = _inventory;
        }
        else
        {


        }
        string tradeData;
        switch (_TradeType)
        {
            case TradeType.traderSell:
                _cost = (int)(_itemsToTrade.CostValue * 0.7f);
                tradeData = Namings.TryFormat("He want to sell {0} for {1} credits.", _itemsToTrade.GetInfo(), _cost);
                break;
            case TradeType.traderBuy:
                if (inventory.Moduls.Count > 0)
                {
                    _itemsToTrade = inventory.Moduls.RandomElement();
                }
                else if (inventory.Weapons.Count > 0)
                {
                    _itemsToTrade = inventory.Weapons.RandomElement();
                }
                _cost = (int)(_itemsToTrade.CostValue * 1f);
                tradeData = Namings.TryFormat("He want to buy your item {0} for {1} credits.", _itemsToTrade.GetInfo(),
                    _cost);
                break;
            default:
                _cost = (int)(_itemsToTrade.CostValue);
                tradeData = "error";
                break;
        }
        //        List<>

        var mianAnswers = new List<AnswerDialogData>();
        var canUse = _TradeType == TradeType.traderBuy || (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_cost) &&
                     _TradeType == TradeType.traderSell);
        if (canUse)
        {
            mianAnswers.Add(new AnswerDialogData($"Ok. Lets trade.", DoTrade, null));
        }
        mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
        var mesData = new MessageDialogData(Namings.TryFormat("This ship wants to trade with you. {0}", tradeData), mianAnswers);
        return mesData;
    }

    private void DoTrade()
    {
        switch (_TradeType)
        {
            case TradeType.traderSell:
                MainController.Instance.MainPlayer.MoneyData.RemoveMoney(_cost);
                InventoryOperation.TryItemTransfered(MainController.Instance.MainPlayer.Inventory, _itemsToTrade, b =>
                {
                    if (!b)
                    {
                        Debug.LogError("Can't transferr item a trade event");
                    }
                });
                break;
            case TradeType.traderBuy:
                MainController.Instance.MainPlayer.MoneyData.AddMoney(_cost);
                InventoryOperation.TryItemTransfered(_inventory, _itemsToTrade, b =>
                {
                    if (!b)
                    {
                        Debug.LogError("Can't transferr item a trade event");
                    }

                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

