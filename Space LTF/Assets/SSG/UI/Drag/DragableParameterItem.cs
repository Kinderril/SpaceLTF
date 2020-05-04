using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DragableParameterItem : DragableItem
{
    public ParameterItem ParameterItem => ContainerItem as ParameterItem;
    public GameObject Rarity1;
    public GameObject Rarity2;


    protected override void Init()
    {
        base.Init();
        switch (ParameterItem.Rarity)
        {
            case EParameterItemRarity.normal:
                Icon.color = Color.white;
                break;
            case EParameterItemRarity.improved:
                Icon.color = Utils.CreateColor(1,176,255);
                break;
            case EParameterItemRarity.perfect:
                Icon.color = Utils.CreateColor(236, 209, 37);
                break;
        }
    }

    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetParameterItemIcon(ParameterItem.ItemType,ParameterItem.SubType);
    }

    public override string GetInfo()
    {
        return ParameterItem.GetInfo();
    }

    protected override void OnClickComplete()
    {
//        if (CanShowWindow())
            WindowManager.Instance.ItemInfosController.Init(ParameterItem, CanShowWindow());
    }
}

