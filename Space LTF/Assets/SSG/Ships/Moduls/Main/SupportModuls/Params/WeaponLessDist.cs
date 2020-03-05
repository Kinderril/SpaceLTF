[System.Serializable]
public class WeaponLessDist : BaseSupportModul
{
    private const float spd_inc = 0.5f;
    private const float dmg_inc = 1.0f;
    public WeaponLessDist(int level)
        : base(SimpleModulType.WeaponLessDist, level)
    {

    }

    private float DmgLevel => dmg_inc + Level * 0.12f;

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= DmgLevel;
        weapon.AimRadius = weapon.AimRadius * spd_inc;
    }

    public override string DescSupport()
    {
        return
            Namings.Format(Namings.Tag(Type.ToString()), Utils.FloatToChance(1f - spd_inc),
                Utils.FloatToChance(DmgLevel));
    }
    public override string DescSupport(WeaponInv inv)
    {
        if (inv.TargetType == TargetType.Enemy)
        {
            return DescSupport();
        }
        else
        {
            return Namings.Format(Namings.Tag("WeaponLessDistSupport"), Utils.FloatToChance(1f - spd_inc),
                Utils.FloatToChance(DmgLevel));
        }

    }
}
