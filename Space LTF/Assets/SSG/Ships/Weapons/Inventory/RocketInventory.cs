using JetBrains.Annotations;

[System.Serializable]
public class RocketInventory : DamageWeaponInv
{

    public RocketInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.rocket, Level)
    {


    }
    public override WeaponInGame CreateForBattle()
    {
        return new RocketWeapon(this);
    }
}

