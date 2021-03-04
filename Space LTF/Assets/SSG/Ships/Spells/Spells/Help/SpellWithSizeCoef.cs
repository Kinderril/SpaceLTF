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
        var deltaTime = Time.time - _castStartTime;
        var coef = (0.2f * deltaTime + 1f);
        float p = Mathf.Clamp(coef, 1, 5);
        return p;
    }

    protected float SizeCoef()
    {

        var deltaTime = Time.time - _castStartTime;
        var coef = Mathf.Pow(deltaTime, 0.9f) + 1;
        float p = Mathf.Clamp(coef, 1, 5);
        return p;
    }
}
