using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipSpeedModul : BaseSupportModul
{
//    private const float spd_inc = 0.3f;
    public const float PER_LEVEL = 0.1f;
    public ShipSpeedModul( int level) 
        : base(SimpleModulType.ShipSpeed, level)
    {

    }
    float d => Level * PER_LEVEL;

    public override string DescSupport()
    {
        return $"Increase ship max speed by {Utils.FloatToChance(d)}%";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
    }

    public override void ChangeParamsShip(IShipAffectableParams Parameters)
    {
        var _delta = d * Parameters.MaxSpeed;
        Parameters.MaxSpeed += _delta;
    }
}
