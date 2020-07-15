using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponImprovePattern : CraftPattern
{
    //Из 3 одинаковых делает 1 миниммального левела с уникальной опцией
    protected override bool CheckInner(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        var weapon1 = item1 as WeaponInv;
        var weapon2 = item2 as WeaponInv;
        var weapon3 = item3 as WeaponInv;

        if (weapon1 == null || weapon2 == null || weapon3 == null)
        {
            return false;
        }
        bool allSame = (weapon1.WeaponType == weapon2.WeaponType && weapon3.WeaponType == weapon2.WeaponType);
        if (allSame)
        {
            return true;
        }

        return false;

    }

    public override IItemInv Craft(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        var weapon1 = item1 as WeaponInv;
        var weapon2 = item2 as WeaponInv;
        var weapon3 = item3 as WeaponInv;

        var lvl = Mathf.Min(weapon1.Level, weapon2.Level, weapon3.Level);
        var upgMin = Mathf.Min(weapon1.SpecialUpgradeds, weapon2.SpecialUpgradeds, weapon3.SpecialUpgradeds);

        var weapon = Library.CreateWeaponByType(weapon1.WeaponType, lvl);

        upgMin++;
        upgMin = Mathf.Clamp(upgMin, 0, 3);

        for (int i = 0; i < upgMin; i++)
        {
            weapon.UpgradeWithOption();
        }
        return weapon;

    }

    public override GameObject ResultIcon(List<IItemInv> item, out string tooltip)
    {
        if (item.Count < 3)
        {
            tooltip = "none";
            return null;
        }
        var weapon1 = item[0] as WeaponInv;
        var weapon2 = item[1] as WeaponInv;
        var weapon3 = item[2] as WeaponInv;
        if (weapon1 == null)
        {
            tooltip = "none";
            return null;
        }
        var obj = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.Craft.WeaponCraftObj);
        obj.Icon.sprite = DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(weapon1.WeaponType);

        var upgMin = Mathf.Min(weapon1.SpecialUpgradeds, weapon2.SpecialUpgradeds, weapon3.SpecialUpgradeds);
        upgMin++;
        upgMin = Mathf.Clamp(upgMin, 0, 3);
        obj.Icon.color = WeaponInv.LevelUpgrades(upgMin);
        tooltip = Namings.Tag("CraftUpgradedWeapon");
        return obj.gameObject;
    }
    public override string TagDesc()
    {
        return "WeaponImprovePattern";
    }
}
