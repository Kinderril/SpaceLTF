
[System.Serializable]
public abstract class SupportWeaponInv : WeaponInv
{
    protected SupportWeaponInv(WeaponInventoryParameters parameters, WeaponType WeaponType, int Level)
        : base(parameters, WeaponType, Level)
    {
    }
}

