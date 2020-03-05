
[System.Serializable]
public abstract class DamageWeaponInv : WeaponInv
{
    protected DamageWeaponInv(WeaponInventoryParameters parameters, WeaponType WeaponType, int Level)
        : base(parameters, WeaponType, Level)
    {
    }
}

