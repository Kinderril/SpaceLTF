using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SimpleTutorialGalaxyData : GalaxyData
{
    public SimpleTutorialGalaxyData(string name) 
        : base(name)
    {

    }
    protected override StartGlobalCell ImpletemtSectors(int sectorCount, int sizeSector, int startPower, int coreCells,
  ShipConfig playerShipConfig, int verticalCount, GalaxyEnemiesArmyController enemiesArmyController)
    {
        _verticalCount = 1;
        verticalCount = 1;
        var allSubSectors = new List<SectorData>();
        SizeOfSector = sizeSector;
        _sectorsCount = 1;
        Debug.Log($"global map size: {_sectorsCount} {verticalCount}");
        var sectors = new SectorTutorial[_sectorsCount, verticalCount];
        var startSector = sectors[0, 0];
        startSector= new SectorTutorial(0, 0, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
            1,0,1, to =>
            {

            },enemiesArmyController);
        startSector.Populate(startPower);
        sectors[0, 0] = startSector;
        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }



        ImplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);

        Debug.Log("Population end");

        return startSector.StartCell;
    }

}
