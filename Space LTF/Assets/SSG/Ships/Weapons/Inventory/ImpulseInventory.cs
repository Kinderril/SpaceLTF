using JetBrains.Annotations;

public enum WeaponUpdageType
{
    random,
    a1,
    b1,
}

[System.Serializable]
public class ImpulseInventory : WeaponInv
{


    public ImpulseInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.impulse, Level)
    {

    }




    public override WeaponInGame CreateForBattle()
    {
        return new ImpulseWeapon(this);
    }

}

