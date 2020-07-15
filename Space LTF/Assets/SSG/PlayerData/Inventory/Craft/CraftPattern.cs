using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public abstract class CraftPattern
{
    public bool CanDo(List<IItemInv> item)
    {
        if (item.Count != 3)
        {
            return false;
        }
        foreach (var itemInv in item)
        {
            if (itemInv == null)
            {
                return false;
            }
        }

        return CheckInner(item[0], item[1], item[2]);
    }

    protected abstract bool CheckInner([NotNull]IItemInv item1, [NotNull] IItemInv item2, [NotNull]IItemInv item3);
    public abstract IItemInv Craft([NotNull]IItemInv item1, [NotNull]IItemInv item2, [NotNull]IItemInv item3);

    public abstract GameObject ResultIcon(List<IItemInv> item,out string tooltip);
    public abstract string TagDesc();
}
