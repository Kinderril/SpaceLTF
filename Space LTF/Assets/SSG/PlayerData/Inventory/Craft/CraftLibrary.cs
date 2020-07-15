using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CraftLibrary
{
    private static List<CraftPattern> _patterns = new List<CraftPattern>();

    static CraftLibrary()
    {
        _patterns.Add(new WeaponChangePattern());
        _patterns.Add(new WeaponImprovePattern());
        _patterns.Add(new ModulChangePattern());
        _patterns.Add(new ModulImprovePattern());
        _patterns.Add(new ParamItemPattern());
    }

    public static bool CanUsePatter(List<IItemInv> items,out CraftPattern pattern)
    {
        foreach (var craftPattern in _patterns)
        {
            if (craftPattern.CanDo(items))
            {
                pattern = craftPattern;
                return true;
            }
        }

        pattern = null;
        return false;
    }

    public static bool TryApplyPatter(List<IItemInv> items, IInventory inventoryToGet, CraftPattern pattern)
    {
       
        if (pattern.CanDo(items))
        {
            var item0 = items[0];
            var item1 = items[1];
            var item2 = items[2];
            var item = pattern.Craft(item0, item1, item2);
            item.CurrentInventory = inventoryToGet;
            bool canRemove = false;
            if (inventoryToGet.GetFreeSlot(out var index, item.ItemType))
            {
                switch (item.ItemType)
                {
                    case ItemType.weapon:
                        var w = item as WeaponInv;
                        canRemove = inventoryToGet.TryAddWeaponModul(w, index);
                        break;
                    case ItemType.modul:
                        var m = item as BaseModulInv;
                        canRemove = inventoryToGet.TryAddSimpleModul(m, index);
                        break;
                    case ItemType.spell:
                        var s = item as BaseSpellModulInv;
                        canRemove = inventoryToGet.TryAddSpellModul(s, index);
                        break;
                    case ItemType.cocpit:
                    case ItemType.engine:
                    case ItemType.wings:
                        var p = item as ParameterItem;
                        canRemove = inventoryToGet.TryAddItem(p);
                        break;
                }
            }

            if (canRemove)
            {
                RemoveItem(item0);
                RemoveItem(item1);
                RemoveItem(item2);
                return true;
            }
        }

        return false;

    }

    private static void RemoveItem(IItemInv item)
    {
        switch (item.ItemType)
        {
            case ItemType.weapon:
                var w = item as WeaponInv;
                item.CurrentInventory.RemoveWeaponModul(w);
                break;
            case ItemType.modul:
                var m = item as BaseModulInv;
                item.CurrentInventory.RemoveSimpleModul(m);
                break;
            case ItemType.spell:
                var s = item as BaseSpellModulInv;
                item.CurrentInventory.RemoveSpellModul(s);
                break;
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                var p = item as ParameterItem;
                item.CurrentInventory.RemoveItem(p);
                break;
        }
    }

    public static List<string> GetAllPatternsTag()
    {
        var str = new List<string>();
        foreach (var craftPattern in _patterns)
        {
                   str.Add(craftPattern.TagDesc());
        }

        return str;
    }
}
