using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponPowerShot : BaseSupportModul
{
    private const float spd_inc = 2.0f;
    private const float dmg_inc = 1.0f;
    public WeaponPowerShot( int level) 
        : base(SimpleModulType.WeaponPowerShot, level)
    {

    }

    private float DmgLevel => dmg_inc + Level * 0.7f;
    

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= DmgLevel;
        weapon.ReloadSec = weapon.ReloadSec * spd_inc;
    }
    public override string DescSupport()
    {
        return $"Increase reload time by {Utils.FloatToChance(spd_inc)}%. Increase damage by {Utils.FloatToChance(DmgLevel)}%.";
    }

}
