using UnityEngine;
using System.Collections;

[System.Serializable]
public class CraftPlayerInventory : PlayerInventory
{
    public CraftPlayerInventory(PlayerSafe player, int maxSlots) 
        : base(player, maxSlots)
    {

    }
    public override bool CanRemoveModulSlots(int slotsInt)
    {
        return true;

    }
    public override bool CanRemoveWeaponSlots(int slotsInt)
    {
        return true;

    }

}
