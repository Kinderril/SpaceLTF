using System;
using UnityEngine;


public static class InventoryOperation
{

    public static DragableItemSlot GetDragableItemSlot()
    {
        return DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.DragableItemSlotPrefab);
    }

    public static MoneySlotUI GetMoneySlot()
    {
        return DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.MoneySlotUIPrefab);
    }

    public static ItemTransfer OnItemTransfer;

    public static bool TryItemChnageTransfered(IItemInv item1, IItemInv item2)
    {
        if (item1.CurrentInventory != item2.CurrentInventory)
        {

        }
        return false;
    }

    public static void TryItemTransfered(IInventory to, IItemInv item, Action<bool> callback)
    {
        IInventory from = item.CurrentInventory;
        int index;
        switch (item.ItemType)
        {
            case ItemType.weapon:
                var w = item as WeaponInv;
                if (to.GetFreeWeaponSlot(out index))
                {
                    CanDo(() =>
                    {
                        if (to.TryAddWeaponModul(w, index))
                        {
                            from.RemoveWeaponModul(w);
                            if (OnItemTransfer != null)
                            {
                                OnItemTransfer(from, to, w);
                            }
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item);
                }
                else
                {
                    callback(false);
                }
                break;
            case ItemType.modul:
                var m = item as BaseModulInv;
                if (to.GetFreeSimpleSlot(out index))
                {
                    CanDo(() =>
                    {
                        if (to.TryAddSimpleModul(m, index))
                        {
                            from.RemoveSimpleModul(m);
                            if (OnItemTransfer != null)
                            {
                                OnItemTransfer(from, to, m);
                            }
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item);
                }
                else
                {
                    callback(false);
                }
                break;
            case ItemType.spell:
                var s = item as BaseSpellModulInv;
                if (to.GetFreeSpellSlot(out index))
                {
                    CanDo(() =>
                    {
                        if (to.TryAddSpellModul(s, index))
                        {
                            from.RemoveSpellModul(s);
                            if (OnItemTransfer != null)
                            {
                                OnItemTransfer(from, to, s);
                            }
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item);
                }
                else
                {
                    callback(false);
                }
                break;
        }
    }

    private static void CanDo(Action CallbackSuccsess, Action failCallback, IInventory to, IItemInv item)
    {
        if (to.IsShop())         //Игрок продает в магазин
        {
            //Selling item to shop
            var valuableCoef = to.ValuableItem(item);
            var preSellPrice = (int)((float)item.CostValue * Library.SELL_COEF * valuableCoef);
#if UNITY_EDITOR
            if (preSellPrice <= 1)
            {
                Debug.LogError($"Sell price is bad: base:{item.CostValue}. preSellPrice:{preSellPrice}. item:{item.GetInfo()}");
            }
#endif
            int sellPrice;
            if (item.CurrentInventory.Owner != null)
            {
                sellPrice = Mathf.Clamp(preSellPrice, 1, 999999);
                // sellPrice = item.CurrentInventory.Owner.ReputationData.ModifSellValue(,preSellPrice);
            }
            else
            {
                sellPrice = Mathf.Clamp(preSellPrice, 1, 999999);
            }
            var msg = Namings.TryFormat("Do you want sell item for {0}?", sellPrice);
            WindowManager.Instance.ConfirmWindow.Init(
                () =>
                {
                    if (item.CurrentInventory.Owner != null)
                    {
                        var sellValue = (int)(sellPrice);
                        item.CurrentInventory.Owner.MoneyData.AddMoney(sellValue);
                    }
                    WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.BuySell);
                    CallbackSuccsess();
                }
                , failCallback, msg);
            return;
        }
        if (!to.CanMoveToByLevel(item, -1))
        {
            WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("CantByLevel"));
            failCallback();
            return;
        }
        if (item.CurrentInventory.IsShop())    //Игрок покупает у магазина
        {
            var valuableCoef = item.CurrentInventory.ValuableItem(item);
            var preBuyPrice = (int)(item.CostValue * valuableCoef);
            int buyPrice;
            if (item.CurrentInventory.Owner != null)
            {
                buyPrice = Mathf.Clamp(preBuyPrice, 1, 999999);
                // buyPrice = item.CurrentInventory.Owner.ReputationData.ModifBuyValue(preBuyPrice);
            }
            else
            {
                buyPrice = Mathf.Clamp(preBuyPrice, 1, 999999);
            }
            if (to.Owner.MoneyData.HaveMoney(buyPrice))
            {
                //Buying item from shop
                var msg = Namings.TryFormat("Do you want buy item for {0}?", buyPrice);
                WindowManager.Instance.ConfirmWindow.Init(
                () =>
                {
                    if (to.Owner != null)
                    {
                        to.Owner.MoneyData.RemoveMoney(buyPrice);
                    }
                    WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.BuySell);
                    CallbackSuccsess();
                }
                , failCallback, msg);
            }
            else
            {
                WindowManager.Instance.NotEnoughtMoney(buyPrice);
                failCallback();
            }

            return;
        }


        CallbackSuccsess();
    }
}
