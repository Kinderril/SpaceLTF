using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponSpeedModul : BaseSupportModul
{
    private const float spd_inc = 0.3f;
    public WeaponSpeedModul( int level) 
        : base(SimpleModulType.WeaponSpeed, level)
    {

    }

    public override string DescSupport()
    {
        return $"Increase bullet speed by {Utils.FloatToChance(spd_inc)}% per level";
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.BulletSpeed = weapon.BulletSpeed * (1 + Level * spd_inc);
    }
}
