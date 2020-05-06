using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class ItemBigTooltip : BaseTooltip
{


    public TextMeshProUGUI SellCosField;

    protected void SetSellCost(int? sellCos,IItemInv item)
    {
        if (sellCos.HasValue)
        {

            SellCosField.text = $"{Namings.Tag("SellItemCost")}: {sellCos} ({item.CostValue})";
        }
        else
        {
            SellCosField.text = $"{Namings.Tag("ItemCost")}: {item.CostValue}";
        }
    }


}
