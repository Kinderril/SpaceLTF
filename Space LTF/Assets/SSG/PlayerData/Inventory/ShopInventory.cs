using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;


[System.Serializable]
public class ShopInventory : PlayerInventory
{
    public ShopInventory([NotNull] Player player) 
        : base(player)
    {

    }

    public override bool IsShop()
    {
        return true;
    }
}

