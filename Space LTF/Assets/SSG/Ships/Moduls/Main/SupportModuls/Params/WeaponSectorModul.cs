using UnityEngine;

[System.Serializable]
public class WeaponSectorModul : BaseSupportModul
{
    private const float spd_inc = 0.3f;
    public WeaponSectorModul(int level)
        : base(SimpleModulType.WeaponSector, level)
    {

    }
    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag(Type.ToString()), Utils.FloatToChance(spd_inc));
    }

    public override void ChangeParams(IAffectParameters weapon)
    {
        var oldSecotr = weapon.SetorAngle * (1 + Level * spd_inc);
        oldSecotr = Mathf.Clamp(oldSecotr, 1, 180);
        weapon.SetorAngle = oldSecotr;
    }
}
