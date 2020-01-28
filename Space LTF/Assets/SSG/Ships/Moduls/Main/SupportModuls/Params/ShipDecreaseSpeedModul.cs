[System.Serializable]
public class ShipDecreaseSpeedModul : BaseSupportModul
{
    //    private const float spd_inc = 0.3f;
    public const float SPEED_LEVEL = 0.5f;
    public const float PER_LEVEL = 0.1f;
    private const float dmg_inc = 0.6f;


    public ShipDecreaseSpeedModul(int level)
        : base(SimpleModulType.ShipDecreaseSpeed, level)
    {

    }
    private float DmgLevel => dmg_inc + Level * 0.2f;
    private float SpeedDecrease => SPEED_LEVEL + Level * PER_LEVEL;

    public override string DescSupport()
    {
        return $"Increase damage by {Utils.FloatToChance(DmgLevel)}%.  Decrease ship max speed by {Utils.FloatToChance(SpeedDecrease)}%";
    }
    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= 1 + DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= 1 + DmgLevel;
    }

    public override void ChangeParamsShip(IShipAffectableParams Parameters)
    {
        var _delta = SpeedDecrease * Parameters.MaxSpeed;
        Parameters.MaxSpeed -= _delta;
    }
}
