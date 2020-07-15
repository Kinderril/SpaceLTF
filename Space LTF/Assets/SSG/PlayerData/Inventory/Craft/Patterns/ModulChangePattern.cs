using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ModulChangePattern : CraftPattern
{
    //Из 3 разеных делает 1 миниммального левела
    protected override bool CheckInner(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        var baseModulInv = item1 as BaseModulInv;
        var baseModulInv2 = item2 as BaseModulInv;
        var baseModulInv3 = item3 as BaseModulInv;

        if (baseModulInv == null || baseModulInv2 == null || baseModulInv3 == null)
        {
            return false;
        }
        bool allSame = (baseModulInv.Type == baseModulInv2.Type && baseModulInv3.Type == baseModulInv2.Type);
        if (!allSame)
        {
            return true;
        }

        return false;

    }

    public override IItemInv Craft(IItemInv item1, IItemInv item2, IItemInv item3)
    {
        var baseModulInv = item1 as BaseModulInv;
        var baseModulInv2 = item2 as BaseModulInv;
        var baseModulInv3 = item3 as BaseModulInv;

        var lvl = Mathf.Min(baseModulInv.Level, baseModulInv2.Level, baseModulInv3.Level);

        var weapon = Library.CreatSimpleModul(lvl);
        return weapon;

    }

    public override GameObject ResultIcon(List<IItemInv> item, out string tooltip)
    {

        var obj = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.Craft.ModulCraftObj);
        obj.Icon.sprite = DataBaseController.Instance.DataStructPrefabs.Craft.QuestionMark;
        tooltip = Namings.Tag("CraftRndModul");
        return obj.gameObject;
    }

    public override string TagDesc()
    {
        return "ModulChangePattern";
    }
}
