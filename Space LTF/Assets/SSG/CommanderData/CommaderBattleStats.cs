using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct ShipDestroyedData
{
    public ShipType Type;
    public ShipConfig Config;

    public ShipDestroyedData(ShipType type, ShipConfig config)
    {
        Type = type;
        Config = config;
    }
}

public class CommaderBattleStats
{
    public List<ShipDestroyedData> ShipsDestroyedDatas = new List<ShipDestroyedData>();

    public void ShipDestoyed(ShipType type,ShipConfig config)
    {
        ShipsDestroyedDatas.Add(new ShipDestroyedData(type,config));
    }
}