

using System;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ChangeItemMapEvent : BaseGlobalMapEvent
{
    private IItemInv _itemsToTrade;
    private IItemInv _myItemsToTrade;
    private PlayerInventory _traderInventory;

    public override string Desc()
    {
        return Namings.Tag("Trader");
    }

    public ChangeItemMapEvent(ShipConfig config)
        : base(config)
    {
        _traderInventory = new PlayerInventory(new Player("trader change"));
    }

    public override MessageDialogData GetDialog()
    {
        bool itemToTradeWeapon = false;
        var inventory = MainController.Instance.MainPlayer.Inventory;
        if (inventory.Moduls.Count > 0)
        {
            _myItemsToTrade = inventory.Moduls.RandomElement();
        }
        else if (inventory.Weapons.Count > 0)
        {
            itemToTradeWeapon = true;
            _myItemsToTrade = inventory.Weapons.RandomElement();
        }
        var mianAnswers = new List<AnswerDialogData>();

        if (_myItemsToTrade != null)
        {
            List<ItemType> types;
            if (itemToTradeWeapon)
            {
                types = new List<ItemType>() { ItemType.modul, ItemType.spell };
            }
            else
            {
                types = new List<ItemType>() { ItemType.weapon, ItemType.spell };
            }
            switch (types.RandomElement())
            {
                case ItemType.weapon:
                    _itemsToTrade = Library.CreateWeapon(MyExtensions.IsTrueEqual());
                    break;
                case ItemType.modul:
                    var list = new List<int>() { 1, 2 };
                    _itemsToTrade = Library.CreatSimpleModul(list.RandomElement(), 2);
                    break;
                case ItemType.spell:
                    _itemsToTrade = Library.CreateSpell();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _itemsToTrade.CurrentInventory = _traderInventory;

            var tradeData = Namings.TryFormat(Namings.DialogTag("changeitem_tradeData"), _myItemsToTrade.GetInfo(), _itemsToTrade.GetInfo());
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("changeitem_ok"), DoTrade, null));
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("changeitem_no"), null));
            var mesData = new MessageDialogData(Namings.TryFormat(Namings.DialogTag("changeitem_start"), tradeData), mianAnswers);
            return mesData;
        }
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null, null));
        var mesData2 = new MessageDialogData(Namings.DialogTag("changeitem_nothing"), mianAnswers);
        return mesData2;
    }

    private void DoTrade()
    {
        InventoryOperation.TryItemTransfered(_traderInventory, _myItemsToTrade, b =>
        {
            if (!b)
            {
                Debug.LogError("Can't transferr item a trade event to _traderInventory");
            }

        });

        InventoryOperation.TryItemTransfered(MainController.Instance.MainPlayer.Inventory, _itemsToTrade, b =>
        {
            if (!b)
            {
                Debug.LogError("Can't transferr item a trade event to MainPlayer");
            }

        });
    }
}

