using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParamItemPattern : CraftPattern
{
    protected override bool CheckInner(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        var paramItem1 = item1 as ParameterItem;
        var paramItem2 = item2 as ParameterItem;
        var paramItem3 = item3 as ParameterItem;
        if (paramItem1 != null && paramItem2 != null && paramItem3 != null)
        {
            if (paramItem1.ItemType == paramItem2.ItemType && paramItem2.ItemType == paramItem3.ItemType)
            {
                if (paramItem1.Rarity == EParameterItemRarity.perfect)
                {
                    return false;
                }
                if (paramItem1.Rarity == paramItem2.Rarity && paramItem2.Rarity == paramItem3.Rarity)
                {
                    return true;
                }
            } 
        }

        return false;
    }

    public override IItemInv Craft(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        if (CheckInner(item1, item2, item3))
        {
            var rnd = new List<EParameterItemSubType>(){EParameterItemSubType.Heavy,EParameterItemSubType.Light,EParameterItemSubType.Middle};
            var paramItem1 = item1 as ParameterItem;
            EParameterItemRarity rarity = EParameterItemRarity.normal;
            switch (paramItem1.Rarity)
            {
                case EParameterItemRarity.normal:
                    rarity = EParameterItemRarity.improved;
                    break;
                case EParameterItemRarity.improved:
                    rarity = EParameterItemRarity.perfect;
                    break;
                case EParameterItemRarity.perfect:
                    rarity = EParameterItemRarity.perfect;
                    break;
            }

            var paramItem = Library.CreateParameterItem(rnd.RandomElement(), rarity, paramItem1.ItemType);
            return paramItem;
        }

        return null;
    }

    public override GameObject ResultIcon(List<IItemInv> item, out string tooltip)
    {
        if (item.Count < 3)
        {
            tooltip = "none";
            return null;
        }


        var paramItem1 = item[0] as ParameterItem;
        var paramItem2 = item[1] as ParameterItem;
        var paramItem3 = item[2] as ParameterItem;
        if (paramItem1 != null && paramItem2 != null && paramItem3 != null)
        {
            if (paramItem1.ItemType == paramItem2.ItemType && paramItem2.ItemType == paramItem3.ItemType)
            {
                if (paramItem1.Rarity == EParameterItemRarity.perfect)
                {
                    tooltip = "none";
                    return null;
                }
                if (paramItem1.Rarity == paramItem2.Rarity && paramItem2.Rarity == paramItem3.Rarity)
                {
                    EParameterItemRarity rarity = EParameterItemRarity.normal;
                    switch (paramItem1.Rarity)
                    {
                        case EParameterItemRarity.normal:
                            rarity = EParameterItemRarity.improved;
                            break;
                        case EParameterItemRarity.improved:
                            rarity = EParameterItemRarity.perfect;
                            break;
                        case EParameterItemRarity.perfect:
                            rarity = EParameterItemRarity.perfect;
                            break;
                    }
                    var obj = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.Craft.ParamItemCraftObj);
                    obj.Icon.sprite = DataBaseController.Instance.DataStructPrefabs.GetParameterItemIcon(paramItem1.ItemType);
                    obj.Icon.color = ParameterItem.GetColor(rarity);
                    tooltip = Namings.Tag("CraftImprovedItem");
                    return obj.gameObject;
                }
            }
        }

        tooltip = "none";
        return null;

    }
    public override string TagDesc()
    {
        return "ParamItemPattern";
    }

}
