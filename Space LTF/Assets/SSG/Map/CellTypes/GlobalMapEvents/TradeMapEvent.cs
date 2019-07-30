

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public TradeMapEvent()
    {
        _inventory = new PlayerInventory(new Player("trader"));
    }

    public override MessageDialogData GetDialog()
    {
        List<ItemType> types = new List<ItemType>(){ItemType.modul,ItemType.weapon,ItemType.spell};
        switch (types.RandomElement())
        {
            case ItemType.weapon:
                _itemsToTrade = Library.CreateWeapon(MyExtensions.IsTrueEqual());
                break;
            case ItemType.modul:
                var list = new List<int>(){1,2};
                _itemsToTrade = Library.CreatSimpleModul(list.RandomElement(),MyExtensions.IsTrueEqual());
                break;
            case ItemType.spell:
                _itemsToTrade = Library.CreateSpell();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _TradeType = MyExtensions.IsTrueEqual() ? TradeType.traderBuy : TradeType.traderSell;

        _itemsToTrade.CurrentInventory = _inventory;
        string tradeData;
        switch (_TradeType)
        {
            case TradeType.traderSell:
                _cost = (int)(_itemsToTrade.CostValue * 0.8f);
                tradeData = $"He want to sell {_itemsToTrade.GetInfo()} for {_cost} credits.";
                break;
            case TradeType.traderBuy:
                _cost = (int)(_itemsToTrade.CostValue * 1.2f);
                tradeData = $"He want to buy your item {_itemsToTrade.GetInfo()} for {_cost} credits.";
                break;
            default:
                _cost = (int)(_itemsToTrade.CostValue );
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
        var mesData = new MessageDialogData($"This ship wants to trade with you. {tradeData}", mianAnswers);
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

