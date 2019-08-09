using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipDecreaseSpeedModul : BaseSupportModul
{
//    private const float spd_inc = 0.3f;
    public const float PER_LEVEL = 0.5f;
    private const float dmg_inc = 0.6f;
    public ShipDecreaseSpeedModul( int level) 
        : base(SimpleModulType.ShipDecreaseSpeed, level)
    {

    }
    private float DmgLevel => dmg_inc + Level * 0.3f;

    public override string DescSupport()
    {
        return $"Increase damage by {Utils.FloatToChance(DmgLevel)}%.  Decrease ship max speed by {Utils.FloatToChance(PER_LEVEL)}% per level";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= DmgLevel;
    }

    public override void ChangeParamsShip(IShipAffectableParams Parameters)
    {
        var d = Level * PER_LEVEL;
        var _delta = d * Parameters.MaxSpeed;
        Parameters.MaxSpeed -= _delta;
    }
}
