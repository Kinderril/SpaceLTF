using System;

[System.Serializable]
public class WeaponBulletSpeedModul : BaseSupportModul
{
    private const float spd_inc = 0.5f;
    public WeaponBulletSpeedModul(int level)
        : base(SimpleModulType.WeaponSpeed, level)
    {

    }

    public override string DescSupport()
    {
        return Namings.TryFormat(Namings.Tag("WeaponSpeedModulDesc"), Utils.FloatToChance(spd_inc));
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        weapon.BulletSpeed = weapon.BulletSpeed * (1 + Level * spd_inc);
    }
}
