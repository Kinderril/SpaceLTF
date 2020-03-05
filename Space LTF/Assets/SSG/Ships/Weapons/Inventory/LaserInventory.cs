using JetBrains.Annotations;

[System.Serializable]
public class LaserInventory : DamageWeaponInv
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

