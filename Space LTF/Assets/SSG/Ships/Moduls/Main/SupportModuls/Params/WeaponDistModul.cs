using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponDistModul : BaseSupportModul
{
    private const float spd_inc = 0.3f;
    public WeaponDistModul( int level) 
        : base(SimpleModulType.WeaponDist, level)
    {

    }

    public override string DescSupport()
    {
        return String.Format("Increase aim radius by {0}% per level", Utils.FloatToChance(spd_inc));
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.AimRadius = weapon.AimRadius * (1 + Level * spd_inc);
    }
}
