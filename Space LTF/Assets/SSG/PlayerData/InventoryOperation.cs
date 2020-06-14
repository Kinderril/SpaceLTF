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



    public static void ChnageItemsItemTransfered(IItemInv item1, IItemInv item2, Action<bool> callback)
    {
        if (item1.CurrentInventory == item2.CurrentInventory)
        {
            callback(false);
            return;
        }
        if (item1.ItemType != item2.ItemType)
        {
            callback(false);
            return;
        }

        if (SubCanDo(item2.CurrentInventory,item1) && SubCanDo(item1.CurrentInventory, item2))
        {
            RemoveItemFromSelfInventory(item1);
            RemoveItemFromSelfInventory(item2);
            var oldInv1 = item1.CurrentInventory;
            var oldInv2 = item2.CurrentInventory;
            TryItemTransfered(oldInv1, item2, b =>
            {
                if (!b)
                {
                    Debug.LogError("transfer item error");
                }
                TryItemTransfered(oldInv2, item1, b1 =>
                    { callback(b); },false);
            },false);
            return;
        }
        callback(false);

        //        item1.CurrentInventory.RemoveItem(item1);

    }

    private static void RemoveItemFromSelfInventory(IItemInv item)
    {

        switch (item.ItemType)
        {
            case ItemType.weapon:
                item.CurrentInventory.RemoveWeaponModul(item as WeaponInv);
                break;
            case ItemType.modul:
                item.CurrentInventory.RemoveSimpleModul(item as BaseModulInv);
                break;
            case ItemType.spell:
                item.CurrentInventory.RemoveSpellModul(item as BaseSpellModulInv);
                break;
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                item.CurrentInventory.RemoveItem(item as ParameterItem);
                break;
        }
    }

    public static void TryItemTransfered(IInventory to, IItemInv item, Action<bool> callback,bool withRemove = true)
    {
        IInventory from = item.CurrentInventory;
        int index;
        switch (item.ItemType)
        {
            case ItemType.engine:
            case ItemType.cocpit:
            case ItemType.wings:
                var itemParam = item as ParameterItem;
                if (itemParam == null)
                {
                    callback(false);
                    return;
                }
                if (to.GetFreeSlot(out index,itemParam.ItemType))
                {
                    CanDo(() =>
                    {
                        if (to.TryAddItem(itemParam))
                        {
                            if (withRemove)
                                from.RemoveItem(itemParam);
                            OnItemTransfer?.Invoke(@from, to, itemParam);
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item, withRemove);
                }
                else
                {
                    callback(false);
                }

                break;
            case ItemType.weapon:
                var w = item as WeaponInv;
                if (to.GetFreeWeaponSlot(out index))
                {
                    CanDo(() =>
                    {
                        if (to.TryAddWeaponModul(w, index))
                        {
                            if (withRemove)
                                from.RemoveWeaponModul(w);
                            OnItemTransfer?.Invoke(@from, to, w);
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item, withRemove);
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
                            if (withRemove)
                                from.RemoveSimpleModul(m);
                            OnItemTransfer?.Invoke(@from, to, m);
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item, withRemove);
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
                            if (withRemove)
                                from.RemoveSpellModul(s);
                            OnItemTransfer?.Invoke(@from, to, s);
                            callback(true);
                        }
                        else
                        {
                            callback(false);
                        }

                    }, () =>
                    {
                        callback(false);
                    }, to, item,withRemove);
                }
                else
                {
                    callback(false);
                }
                break;
        }
    }

    private static bool SubCanDo(IInventory to, IItemInv item)
    {
        var paramItem = item as ParameterItem;
        if (paramItem != null)
        {
            float slots;
            if (paramItem.ParametersAffection.TryGetValue(EParameterShip.modulsSlots, out slots))
            {
                var slotsInt = (int)(slots + 0.1f);
                if (!paramItem.CurrentInventory.CanRemoveModulSlots(slotsInt))
                {
                    return false;
                }
            }
            if (paramItem.ParametersAffection.TryGetValue(EParameterShip.weaponSlots, out slots))
            {
                var slotsInt = (int)(slots + 0.1f);
                if (!paramItem.CurrentInventory.CanRemoveWeaponSlots(slotsInt))
                {
                    return false;
                }
            }
        }

        if (to.IsShop()) //Игрок продает в магазин
        {
            return false;
        }

        if (!to.CanMoveToByLevel(item, -1))
        {
            return false;
        }
        if (item.CurrentInventory.IsShop())    //Игрок покупает у магазина
        {
            return false;
        }

        return true;
    }
    public static bool CanDo(IInventory to, IItemInv item)
    {
        int index;
        switch (item.ItemType)
        {
            case ItemType.engine:
            case ItemType.cocpit:
            case ItemType.wings:
                var itemParam = item as ParameterItem;
                if (itemParam == null)
                {
                    return false;
                }
                if (to.GetFreeSlot(out index, itemParam.ItemType))
                {
                    return SubCanDo(to, item);
                }
                else
                {
                    return (false);
                }

                break;
            case ItemType.weapon:
                var w = item as WeaponInv;
                if (to.GetFreeWeaponSlot(out index))
                {
                    return SubCanDo(to, item);
                }
                else
                {
                    return (false);
                }
                break;
            case ItemType.modul:
                var m = item as BaseModulInv;
                if (to.GetFreeSimpleSlot(out index))
                {
                    return SubCanDo(to, item);
                }
                else
                {
                    return (false);
                }
                break;
            case ItemType.spell:
                var s = item as BaseSpellModulInv;
                if (!to.GetFreeSpellSlot(out index))
                {
                    return SubCanDo( to, item);
                }
                else
                {
                    return (false);
                }
                break;
        }

        return false;
    }

    public static int CalcBuyPrice(IItemInv item)
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

        return buyPrice;
    }

    public static int CalcSellPrice(IInventory to,IItemInv item)
    {
        var valuableCoef = to.ValuableItem(item);
        var preSellPrice = (int)((float)item.CostValue * Library.SELL_COEF * valuableCoef * item.CurrentInventory.Owner.CreditsCoef);
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

        return sellPrice;
    }
    private static void CanDo(Action CallbackSuccsess, Action failCallback, IInventory to, IItemInv item,bool withRemove)
    {
        var paramItem = item as ParameterItem;
        if (paramItem != null)
        {
            float slots;
            if (paramItem.ParametersAffection.TryGetValue(EParameterShip.modulsSlots, out slots))
            {
                var slotsInt = (int) (slots + 0.1f);
                if (withRemove && !paramItem.CurrentInventory.CanRemoveModulSlots(slotsInt))
                {
                    failCallback();
                    WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("cantRemoveCauseSlots"));
                    return;
                }
            }  
            if (paramItem.ParametersAffection.TryGetValue(EParameterShip.weaponSlots, out slots))
            {
                var slotsInt = (int) (slots + 0.1f);
                if (withRemove && !paramItem.CurrentInventory.CanRemoveWeaponSlots(slotsInt))
                {
                    failCallback();
                    WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("cantRemoveCauseSlots"));
                    return;
                }
            }
        }

        if (to.IsShop())         //Игрок продает в магазин
        {
            //Selling item to shop
            int sellPrice = CalcSellPrice(to, item);
            var msg = Namings.Format(Namings.Tag("wantSell"), sellPrice);
            WindowManager.Instance.ConfirmWindow.Init(
                () =>
                {
                    if (item.CurrentInventory.Owner != null)
                    {
                        var sellValue = (int)(sellPrice);
                        item.CurrentInventory.Owner.AddMoneyAfterSell(sellValue);
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
            int buyPrice = CalcBuyPrice(item);
            if (to.Owner.HaveMoney(buyPrice))
            {
                //Buying item from shop
                var msg = Namings.Format(Namings.Tag("wantBuyItem"), buyPrice);
                WindowManager.Instance.ConfirmWindow.Init(
                () =>
                {
                    if (to.Owner != null)
                    {
                        to.Owner.RemoveMoney(buyPrice);
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
