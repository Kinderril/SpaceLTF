using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorStartMidCampaign : SectorData
{
    private StartGlobalCell _startGlobalCell;
    public StartGlobalCell StartCell  => _startGlobalCell;

    private int _act;
    private ShipConfig _campSide;
    public SectorStartMidCampaign(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController,int act,ShipConfig campSide)
        : base(startX, startZ, size, maxCountEvents, shipConfig,  xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {

        _act = act;
        _campSide = campSide;
    }

    protected override void PrePopulate()
    {
        _startGlobalCell = CreateStartCell(true, _act, _campSide);
        base.PrePopulate();
    }
}