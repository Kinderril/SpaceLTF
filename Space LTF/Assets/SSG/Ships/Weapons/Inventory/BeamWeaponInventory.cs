using JetBrains.Annotations;

[System.Serializable]
public class BeamWeaponInventory : DamageWeaponInv
{
    public BeamWeaponInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.beam, Level)
    {

    }


    public override WeaponInGame CreateForBattle()
    {
        return new BeamWeapon(this);
    }
}

