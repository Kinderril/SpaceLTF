using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

public class ItemBigTooltip : BaseTooltip
{


    public TextMeshProUGUI CostField;
    public TextMeshProUGUI SellCostField;

    protected void SetSellCost(int? sellCos,IItemInv item)
    {
        CostField.text = $"{Namings.Tag("ItemCost")}: {item.CostValue}";
        SellCostField.gameObject.SetActive(sellCos.HasValue);
        if (sellCos.HasValue)
        {
            SellCostField.text = $"{Namings.Tag("SellItemCost")}: {sellCos}";
        }
    }


}
