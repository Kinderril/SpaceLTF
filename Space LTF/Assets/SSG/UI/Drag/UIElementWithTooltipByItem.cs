using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIElementWithTooltipByItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isInited;
    private IItemInv _item;
    private int? _sellCost;
    public void Init(IItemInv item,int? sellCost)
    {
        _sellCost = sellCost;
        _item = item;
        _isInited = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isInited)
        {
            return;
        }
        switch (_item.ItemType)
        {
            case ItemType.weapon:
                var tt1 = DataBaseController.Instance.Pool.Tooltips._WeaponBigTooltip;
                tt1.Init(_item as WeaponInv, _sellCost, gameObject);
                break;
            case ItemType.modul:
                var tt = DataBaseController.Instance.Pool.Tooltips._ModulBigTooltip;
                tt.Init(_item as BaseModulInv, _sellCost, gameObject);
                break;
            case ItemType.spell:
                var tt2 = DataBaseController.Instance.Pool.Tooltips._SpellBigTooltip;
                tt2.Init(_item as BaseSpellModulInv, _sellCost, gameObject);
                break;
            case ItemType.cocpit:
            case ItemType.engine:
            case ItemType.wings:
                var tt3 = DataBaseController.Instance.Pool.Tooltips._ParamItemBigTooltip;
                tt3.Init(_item as ParameterItem, _sellCost, gameObject);
                break;

        }

//        toolTip.Init(TextToTooltip(), gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DataBaseController.Instance.Pool.Tooltips.CloseTooltip();
    }


}
