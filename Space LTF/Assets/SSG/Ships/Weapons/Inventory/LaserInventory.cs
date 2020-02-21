using JetBrains.Annotations;

[System.Serializable]
public class LaserInventory : WeaponInv
{
    public LaserInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.laser, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new LaserWeapon(this);
    }
}

