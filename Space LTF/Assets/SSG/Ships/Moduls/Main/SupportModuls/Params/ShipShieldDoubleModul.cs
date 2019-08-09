using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipShieldDoubleModul : BaseSupportModul
{
//    private const float spd_inc = 0.3f;
//    public const float PER_LEVEL = 0.8f;
    private const float dmg_inc = 0.5f;
    public ShipShieldDoubleModul( int level) 
        : base(SimpleModulType.ShieldDouble, level)
    {

    }
    private float PER_LEVEL => dmg_inc + Level * 0.2f;

    public override string DescSupport()
    {
        return $"Increase shield power by {Utils.FloatToChance(PER_LEVEL)}% . Decrease ship body points by {Utils.FloatToChance(PER_LEVEL)}%";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {

    }

    public override void ChangeParamsShip(IShipAffectableParams Parameters)
    {
        var _delta = PER_LEVEL * Parameters.MaxShield;
        Parameters.MaxShield += _delta;   
        var _deltaHp = PER_LEVEL * Parameters.MaxHealth;
        Parameters.MaxHealth = Mathf.Clamp(_deltaHp + Parameters.MaxHealth,5,99999);
    }
}
