using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponChangePattern : CraftPattern
{
    //Из 3 разных делает 1 миниммального левела + 1
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
        if (!allSame)
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

        var types = new HashSet<WeaponType>();

        types.Add(weapon1.WeaponType);
        types.Add(weapon2.WeaponType);
        types.Add(weapon3.WeaponType);
        var lvl = Mathf.Min(weapon1.Level, weapon2.Level, weapon3.Level);

        var weapon = Library.CreateWeaponByType(types.ToList().RandomElement(), lvl + 1);
        return weapon;

    }
    public override GameObject ResultIcon(List<IItemInv> item, out string tooltip)
    {

        var obj = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.Craft.WeaponCraftObj);
        obj.Icon.sprite = DataBaseController.Instance.DataStructPrefabs.Craft.QuestionMark;
        tooltip = Namings.Tag("CraftRndWeapon");
        return obj.gameObject;
    }
    public override string TagDesc()
    {
        return "WeaponChangePattern";
    }
}
