using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ExprolerGalaxyDataMap : GalaxyData
{
    public ExprolerGalaxyDataMap(string name) 
        : base(name)
    {

    }
    protected override StartGlobalCell ImpletemtSectors(int sectorCount, int sizeSector, int startPower, 
         ShipConfig playerShipConfig, int verticalCount, 
         GalaxyEnemiesArmyController enemiesArmyController, ExprolerCellMapType mapType)
    {
        var step = sizeSector + 1;
        switch (mapType)
        {
            default:
            case ExprolerCellMapType.normal:
            case ExprolerCellMapType.milatary:
                verticalCount = 1;
                break;;
            case ExprolerCellMapType.longType:
                verticalCount = 2;
                break;
        }
        _verticalCount = verticalCount;
        var allSubSectors = new List<SectorData>();
        SizeOfSector = sizeSector;
        _sectorsCount = 1;
        Debug.Log($"global map size: {_sectorsCount} {verticalCount}");
        var sectors = new SectorExprolerData[_sectorsCount, verticalCount];
        var startSector = sectors[0, 0];
        var powerPerTurn = 0.25f;
        switch (mapType)
        {
            default:
            case ExprolerCellMapType.normal:
                startSector = new SectorExproler(0, 0, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
                     0, powerPerTurn, to =>
                    {

                    }, enemiesArmyController);
                break;
            case ExprolerCellMapType.milatary:
                startSector = new SectorExprolerMilitary(0, 0, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
                     0, powerPerTurn, to =>
                    {

                    }, enemiesArmyController);
                
                break;
            case ExprolerCellMapType.longType:
                var zz = step;
                startSector = new SectorExproler(0, 0, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
                     0, powerPerTurn, to =>
                    {

                    }, enemiesArmyController,true,false);
                var secondSector = new SectorExproler(0, zz, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), playerShipConfig,
                         0, powerPerTurn, to =>
                        {

                        }, enemiesArmyController, false, true);

                secondSector.Populate(startPower);
                sectors[0, 1] = secondSector;
                allSubSectors.Add(secondSector);
                break;
        }
        startSector.Populate(startPower);
        sectors[0, 0] = startSector;
        allSubSectors.Add(startSector);



        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }



        AddPortals(_sectorsCount, sectors, verticalCount);
        ImplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);
        BornArmies(sectors, sizeSector, _sectorsCount, verticalCount);

        Debug.Log("Population end");

        return startSector.StartCell;
    }

}
