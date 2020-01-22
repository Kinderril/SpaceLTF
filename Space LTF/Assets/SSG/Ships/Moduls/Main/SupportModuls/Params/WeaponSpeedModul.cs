using System;

[System.Serializable]
public class WeaponSpeedModul : BaseSupportModul
{
    private const float spd_inc = 0.5f;
    public WeaponSpeedModul(int level)
        : base(SimpleModulType.WeaponSpeed, level)
    {

    }

    public override string DescSupport()
    {
        return String.Format(Namings.Tag("WeaponSpeedModulDesc"), Utils.FloatToChance(spd_inc));
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.BulletSpeed = weapon.BulletSpeed * (1 + Level * spd_inc);
    }
}
