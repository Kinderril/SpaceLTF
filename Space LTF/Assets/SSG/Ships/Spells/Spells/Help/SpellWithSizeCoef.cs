using System;
using System.Collections;
using UnityEngine;


[Serializable]
public abstract class SpellWithSizeCoef : BaseSpellModulInv
{
    protected SpellWithSizeCoef(IInventory currentInventory) : base(currentInventory)
    {
    }

    protected SpellWithSizeCoef(SpellType spell, int costTime,
        BulleStartParameters bulleStartParameters, bool isHoming,
        TargetType targetType) : base(spell, costTime, bulleStartParameters, isHoming, targetType)
    {

    }
    protected float CoefPower()
    {
        return PowerInc();
    }

    protected float SizeCoef()
    {
        return PowerInc();
    }
}
