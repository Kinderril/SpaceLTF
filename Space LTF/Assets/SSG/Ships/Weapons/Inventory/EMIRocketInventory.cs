using JetBrains.Annotations;

[System.Serializable]
public class EMIRocketInventory : DamageWeaponInv
{
    public EMIRocketInventory([NotNull] WeaponInventoryParameters parameters, int Level)
        : base(parameters, WeaponType.eimRocket, Level)
    {

    }

    public override WeaponInGame CreateForBattle()
    {
        return new EMIRocketWeaponGame(this);
    }
}

