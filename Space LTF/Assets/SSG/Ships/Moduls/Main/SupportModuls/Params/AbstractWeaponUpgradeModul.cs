﻿[System.Serializable]
public class AbstractWeaponUpgradeModul : BaseSupportModul
{
    private WeaponType _type;
    private const float dmg_inc = 1.0f;
    private const float dmg_c = 0.3f;

    public AbstractWeaponUpgradeModul(WeaponType type, SimpleModulType typeModul, int level)
        : base(typeModul, level)
    {
        _type = type;
    }

    private float DmgLevel => dmg_inc + Level * dmg_c;

    public override void ChangeParams(IAffectParameters weapon)
    {
        var weaponInv = weapon as WeaponInv;
        if (weaponInv != null && weaponInv.WeaponType == _type)
        {
            weapon.CurrentDamage.BodyDamage *= DmgLevel;
            weapon.CurrentDamage.ShieldDamage *= DmgLevel;
        }
    }


    public override string DescSupport()
    {
        return Namings.Format(Namings.Tag("WeaponIncreaseDamage"), Utils.FloatToChance(Level * dmg_c),
            Namings.Weapon(_type));
    }
}

