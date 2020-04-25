using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class TutorialGalaxyData : GalaxyData
{
    public TutorialGalaxyData(string name) 
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
        Debug.Log($"global map size: {_sectorsCount} {verticalCount}");



        var sectors = new SectorTutorial[_sectorsCount, verticalCount];
        var id = 0;
        var maxDist = 0;

        //Create start sector       
        var startSector = sectors[0, 0];
        startSector= new SectorTutorial(0, 0, 10, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
            1,0,1, to =>
            {

            });
        startSector.Populate(startPower);
        sectors[0, 0] = startSector;
        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }



        InplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);

        Debug.Log("Population end");

        return startSector.StartCell;
    }

}
