using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EParameterShip
{
    speed,
    turn,
    bodyPoints,
    shieldPoints,
    bodyArmor,
    modulsSlots,
    weaponSlots,

}

public enum EParameterItemRarity
{
    normal,
    improved,
    perfect
}

public enum EParameterItemSubType
{
    Light,
    Middle,
    Heavy
}

[System.Serializable]
public class ParameterItem : IItemInv
{
    public ItemType ItemType => _itemType;
    public EParameterItemRarity Rarity { get; private set; }
    public EParameterItemSubType SubType { get; private set; }
    private ItemType _itemType;
    public IInventory CurrentInventory { get; set; }

    public int CostValue
    {
        get
        {
            switch (Rarity)
            {
                case EParameterItemRarity.normal:
                    return Library.PARAMETER_ITEM_COST_NORMAL;
                case EParameterItemRarity.improved:
                    return Library.PARAMETER_ITEM_COST_IMPROVED;
                case EParameterItemRarity.perfect:
                    return Library.PARAMETER_ITEM_COST_PERFECT;
                default:
                    return 1;
            }
        }
    }

    public Dictionary<EParameterShip, float> ParametersAffection = new Dictionary<EParameterShip, float>();

    public static Color GetColor(EParameterItemRarity rarity)
    {
        switch (rarity)
        {
            case EParameterItemRarity.normal:
                default:
                return Color.white;
                break;
            case EParameterItemRarity.improved:
                return Utils.CreateColor(1, 176, 255);
                break;
            case EParameterItemRarity.perfect:
                return Utils.CreateColor(236, 209, 37);
                break;
        }
        return Color.white;
    }

    public ParameterItem(ItemType ItemType, EParameterItemRarity rarity,
        EParameterItemSubType subType, Dictionary<EParameterShip, float> parametersAffection)
    {
        this.ParametersAffection = parametersAffection;
        SubType = subType;
        Rarity = rarity;
        _itemType = ItemType;
    }

    public int RequireLevel(int posibleLevel = -1)
    {
        return 1;
    }

    public string GetName()
    {
        return Namings.Tag($"{_itemType.ToString()}");

    }

    public string GetInfo()
    {
        var name = GetName();
        return $"{name}({Namings.Tag($"EParameterItemRarity{Rarity.ToString()}")})";  //TODO LOCALIZTION
    }

    public string WideInfo()
    {
        return $"Parameter item {_itemType.ToString()} Rarity:{Rarity.ToString()}";  //TODO LOCALIZTION
    }
}
