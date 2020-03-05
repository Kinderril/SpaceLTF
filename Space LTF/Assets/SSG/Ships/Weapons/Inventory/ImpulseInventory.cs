using JetBrains.Annotations;


[System.Serializable]
public class ImpulseInventory : DamageWeaponInv
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

