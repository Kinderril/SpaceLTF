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
        Icon.color = ParameterItem.GetColor(ParameterItem.Rarity);
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

