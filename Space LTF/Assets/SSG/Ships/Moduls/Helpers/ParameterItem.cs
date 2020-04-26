using UnityEngine;
using System.Collections;


public enum EParameterItemRarity
{
    normal,
    improved,
    perfect
}public enum EParameterItemSubType
{
    light,
    middle,
    heavy
}

[System.Serializable]
public class ParameterItem : IItemInv
{
    public ItemType ItemType => _itemType;
    public EParameterItemRarity Rarity { get; private set; }
    public EParameterItemSubType SubType { get; private set; }
    private ItemType _itemType;
    public IInventory CurrentInventory { get; set; }
    public int CostValue { get; }
    public string Name => "TODOname";        //TODO LOCALIZTION

    public ParameterItem(ItemType ItemType, EParameterItemRarity rarity, EParameterItemSubType subType)
    {
        SubType = subType;
        Rarity = rarity;
        _itemType = ItemType;
    }

    public int RequireLevel(int posibleLevel = -1)
    {
        return 1;
    }

    public string GetInfo()
    {
        return $"{Namings.Tag($"itemType_ItemType.ToString(")}({Namings.Tag(Rarity.ToString())})";  //TODO LOCALIZTION
    }

    public string WideInfo()
    {
        return $"Parameter item {_itemType.ToString()} Rarity:{Rarity.ToString()}";  //TODO LOCALIZTION
    }
}
