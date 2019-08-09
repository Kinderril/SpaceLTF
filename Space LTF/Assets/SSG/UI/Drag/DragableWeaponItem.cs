using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;


public class DragableWeaponItem : DragableItem 
{
    public WeaponInv Weapon { get { return ContainerItem as WeaponInv;} }
    public TextMeshProUGUI LevelField;

    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(Weapon.WeaponType);
    }
    public override string GetInfo()
    {
        LevelField.text = Weapon.Level.ToString();
        return Weapon.GetInfo();
    }

    protected override void OnClickComplete()
    {
        var shipInv = (Weapon.CurrentInventory as ShipInventory) != null;
//        if (CanShowWindow())
            WindowManager.Instance.ItemInfosController.Init(Weapon, CanShowWindow(), shipInv);
    }
}

