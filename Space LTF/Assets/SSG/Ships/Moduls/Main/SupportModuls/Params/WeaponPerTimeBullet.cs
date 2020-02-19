[System.Serializable]
public class WeaponPerTimeBullet : BaseSupportModul
{
    private const float dmg_inc = 0.5f;
    private const float per_level = 0.1f;

    private float dmgRes => dmg_inc + Level * per_level;
    public WeaponPerTimeBullet(int level)
        : base(SimpleModulType.WeaponShootPerTime, level)
    {

    }

    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag(Type.ToString()), Utils.FloatToChance(dmgRes));
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.ShootPerTime = weapon.ShootPerTime + 1;
        weapon.CurrentDamage.BodyDamage *= dmgRes;
        weapon.CurrentDamage.ShieldDamage *= dmgRes;
    }
}
