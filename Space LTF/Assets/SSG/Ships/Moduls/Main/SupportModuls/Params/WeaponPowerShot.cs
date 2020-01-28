[System.Serializable]
public class WeaponPowerShot : BaseSupportModul
{
    private const float relocad_inc = 2.0f;
    private const float dmg_inc = 1.0f;
    public WeaponPowerShot(int level)
        : base(SimpleModulType.WeaponPowerShot, level)
    {

    }

    private float DmgLevel => dmg_inc + Level * 0.35f;


    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.CurrentDamage.BodyDamage *= DmgLevel;
        weapon.CurrentDamage.ShieldDamage *= DmgLevel;
        weapon.ReloadSec = weapon.ReloadSec * relocad_inc;
    }
    public override string DescSupport()
    {
        return $"Increase reload time by {Utils.FloatToChance(relocad_inc)}%. Increase damage by {Utils.FloatToChance(DmgLevel)}%.";
    }

}
