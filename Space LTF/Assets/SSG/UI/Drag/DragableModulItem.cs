using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DragableModulItem : DragableItem
{
    public BaseModulInv Modul
    {
        get { return ContainerItem as BaseModulInv; }
    }

    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetModulIcon(Modul.Type);
    }

    public override string GetInfo()
    {
        return Modul.GetInfo();
    }

    protected override void OnClickComplete()
    {
        if (CanShowWindow())
            WindowManager.Instance.ItemInfosController.Init(Modul);
    }
}

