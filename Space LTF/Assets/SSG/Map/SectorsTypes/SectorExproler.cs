using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorExproler : SectorExprolerData
{

    private StartGlobalCell _startGlobalCell;
    public override StartGlobalCell StartCell => _startGlobalCell;
    public override bool AnyEvent => true;

    private bool _withStart = true;
    private bool _withEnd = true;
    public SectorExproler(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int index, int xIndex, float powerPerTurn
        , DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController, bool withStart = true, bool withEnd = true)
        : base(startX, startZ, size, maxCountEvents, shipConfig, index, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {
        _withStart = withStart;
        _withEnd = withEnd;
    }

    protected override SectorDataEventsCounts GetCountOfEvents(int remainCells, int sectorSize)
    {
        return SectorDataEventsCounts.Exproler(remainCells, sectorSize);
    }
    protected override void PrePopulate()
    {
        if (_withStart)
            _startGlobalCell = CreateStart();
        if (_withEnd)
            CreateEnd();
    }
}