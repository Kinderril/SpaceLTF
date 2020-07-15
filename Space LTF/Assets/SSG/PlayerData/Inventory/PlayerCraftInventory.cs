using UnityEngine;
using System.Collections;

public class PlayerCraftInventory : PlayerInventory
{
    public const int CraftSlots = 3;
    public PlayerCraftInventory(PlayerSafe player, int maxSlots) 
        : base(player, CraftSlots)
    {

    }

    public bool CanCraft()
    {
        if (GetAllItems().Count == CraftSlots)
        {
            return true;
        }

        return false;
    }




}
