using System.Collections.Generic;

public class ArmyCreatorRnd : ArmyCreatorData
{
    private List<ShipConfig> _configs = new List<ShipConfig>()
    {
        ShipConfig.federation,
        ShipConfig.mercenary,
        ShipConfig.krios,
        ShipConfig.raiders,
        ShipConfig.ocrons,
    };
    public override ShipConfig ArmyConfig
    {
        get
        {
            return _configs.RandomElement();
        }
    }

    protected override List<WeaponType> AllWeaponModuls()
    {
        return AllWeaponModuls();
    }

    public ArmyCreatorRnd(ShipConfig config, bool isAi)
        : base(config, isAi)
    {

    }
}

