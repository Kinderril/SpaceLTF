[System.Serializable]
public class ArmorModul : ActionModulInGame
{
    public ArmorModul(BaseModulInv baseModulInv)
        : base(baseModulInv)
    {

    }

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        base.Apply(Parameters, owner);
        Parameters.BodyArmor += ModulData.Level * 2;
        // Parameters.ShieldArmor += ModulData.Level;
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override void Delete()
    {
        base.Delete();
    }
}

