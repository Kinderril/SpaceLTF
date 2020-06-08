using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class ShopTutorInventory : ShopInventory
{
    public ShopTutorInventory([NotNull] PlayerSafe player) 
        : base(player)
    {

    }
}
