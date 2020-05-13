using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AdvTutorialGalaxyData : GalaxyData
{
    public AdvTutorialGalaxyData(string name) 
        : base(name)
    {

    }
    protected override StartGlobalCell ImpletemtSectors(int sectorCount, int sizeSector, int startPower, int coreCells,
  ShipConfig playerShipConfig, int verticalCount)
    {
        _verticalCount = 1;
        verticalCount = 1;
        var allSubSectors = new List<SectorData>();
        SizeOfSector = sizeSector;
        _sectorsCount = 1;
//        Debug.Log($"AdvTutorialGalaxyData map size: {_sectorsCount} {verticalCount}");
        var sectors = new SectorTutorialAdvanced[_sectorsCount, verticalCount];
        var startSector = sectors[0, 0];
        startSector= new SectorTutorialAdvanced(0, 0, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
            1,0,1, to =>
            {

            });
        allSubSectors.Add(startSector);
        startSector.Populate(startPower);
        sectors[0, 0] = startSector;
        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }
//        Debug.Log($"3startSector {startSector.Size}");
        InplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);
        Debug.Log("AdvTutorialGalaxyData Population end");
        return startSector.StartCell;
    }

}
