[System.Serializable]
public class WeaponLessDist : BaseSupportModul
{
    private const float spd_inc = 0.7f;
    private const float dmg_inc = 1.0f;
    public WeaponLessDist(int level)
        : base(SimpleModulType.WeaponLessDist, level)
    {

    }

    private float DmgLevel => dmg_inc + Level * 0.15f;

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= DmgLevel;
        weapon.AimRadius = weapon.AimRadius * spd_inc;
    }

    public override string DescSupport()
    {
        return
            $"Decrease aim radius by {Utils.FloatToChance(1f - spd_inc)}%. Increase damage by {Utils.FloatToChance(DmgLevel)}%.";
    }
}
