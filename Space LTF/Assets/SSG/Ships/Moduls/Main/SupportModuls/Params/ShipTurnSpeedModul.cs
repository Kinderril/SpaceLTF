using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipTurnSpeedModul : BaseSupportModul
{
//    private const float spd_inc = 0.3f;
    public const float PER_LEVEL = 0.1f;
    public ShipTurnSpeedModul( int level) 
        : base(SimpleModulType.ShipTurnSpeed, level)
    {

    }

    float d => Level * PER_LEVEL;
    public override string DescSupport()
    {
        return $"Increase ship turn speed by {Utils.FloatToChance(d)}%";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
    }

    public override void ChangeParamsShip(IShipAffectableParams Parameters)
    {
        var _delta = d * Parameters.TurnSpeed;
        Parameters.TurnSpeed += _delta;
    }
}
