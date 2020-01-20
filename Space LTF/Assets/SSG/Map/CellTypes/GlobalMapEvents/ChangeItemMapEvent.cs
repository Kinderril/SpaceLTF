

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



[System.Serializable]
public class ChangeItemMapEvent : BaseGlobalMapEvent
{
    private IItemInv _itemsToTrade;
    private IItemInv _myItemsToTrade;
    private PlayerInventory _traderInventory;

    public override string Desc()
    {
        return "Trader";
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
                    _itemsToTrade = Library.CreatSimpleModul(list.RandomElement(),2);
                    break;
                case ItemType.spell:
                    _itemsToTrade = Library.CreateSpell();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _itemsToTrade.CurrentInventory = _traderInventory;

            var tradeData = $"\nThey want to change your {_itemsToTrade.GetInfo()} to {_itemsToTrade.GetInfo()}";
            mianAnswers.Add(new AnswerDialogData($"Ok. Lets change.", DoTrade, null));
            mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
            var mesData = new MessageDialogData($"This ship wants to trade with you. {tradeData}", mianAnswers);
            return mesData;
        }
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("leave"), null, null));
        var mesData2 = new MessageDialogData($"You have nothing to trade", mianAnswers);
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

