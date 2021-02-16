using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MidChampaingGalaxyDataMap : GalaxyData
{
    private int _act;
    private ShipConfig _campSide;
    public MidChampaingGalaxyDataMap(string name,int act,ShipConfig side) 
        : base(name)
    {
        _campSide = side;
        _act = act;
    }

    protected override StartGlobalCell ImpletemtSectors(int sectorCount, int sizeSector, int startPower, 
         ShipConfig playerShipConfig, int verticalCount, 
         GalaxyEnemiesArmyController enemiesArmyController, ExprolerCellMapType mapType)
    {
        var step = sizeSector + 1;
        _sectorsCount = sectorCount;

        _verticalCount = verticalCount;
        var allSubSectors = new List<SectorData>();
        var sectorsToConnect = new List<SectorData>();
        SizeOfSector = sizeSector;
        Debug.Log($"global map size: {_sectorsCount} {verticalCount}   SizeOfSector:{SizeOfSector}");
        var sectors = new SectorData[_sectorsCount, verticalCount];
        var powerPerTurn = 0.25f;




        var s1 = AddNationSector(0, 1, sectors, ShipConfig.federation, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        var s2= AddNationSector(1, 1, sectors, ShipConfig.mercenary, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        var s3 = AddNationSector(2, 1, sectors, ShipConfig.krios, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        var s4 = AddNationSector(0, 2, sectors, ShipConfig.droid, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        var s5 = AddNationSector(1, 2, sectors, ShipConfig.raiders, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        var s6 = AddNationSector(2, 2, sectors, ShipConfig.ocrons, powerPerTurn, enemiesArmyController, startPower,
            allSubSectors, step);

        sectorsToConnect.Add(s1);
        sectorsToConnect.Add(s2);
        sectorsToConnect.Add(s3);
        sectorsToConnect.Add(s4);
        sectorsToConnect.Add(s5);
        sectorsToConnect.Add(s6);

        var startX = 1;
        var startZ = 0;

        var startSector = sectors[startX, startZ];
        startSector = new SectorStartMidCampaign(startX * step, startZ * step, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), ShipConfig.droid,
            startX, powerPerTurn, to =>
            {

            }, enemiesArmyController,_act, _campSide);
        startSector.Populate(startPower);
        sectors[startX, startZ] = startSector;
        allSubSectors.Add(startSector);

        if (_act >= 2)
        {
            FinalBattle(step, enemiesArmyController, sectors, startPower, allSubSectors);
        }

        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }



        ConnectSectorTop(startSector, s1);
        ConnectSectorTop(startSector, s3);
        AddPortals(_sectorsCount, sectors, verticalCount);
        ImplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);
        BornArmies(sectors, sizeSector, _sectorsCount, verticalCount);

        Debug.Log("Population end");

        return (startSector as SectorStartMidCampaign).StartCell;
    }

    private void FinalBattle(int step, GalaxyEnemiesArmyController enemiesArmyController, 
        SectorData[,] sectors, int startPower, List<SectorData> allSubSectors)
    {

        var startX = 1;
        var startZ = 3;

        var startSector = sectors[startX, startZ];
        startSector = new SectorFinalBattle(startX * step, startZ * step, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), ShipConfig.droid,
            startX, 0, to =>
            {

            }, enemiesArmyController);
        startSector.Populate(startPower);
        sectors[startX, startZ] = startSector;
        allSubSectors.Add(startSector);
    }

    private SectorData AddNationSector(int x,int z, SectorData[,] sectors,ShipConfig config,
        float powerPerTurn,GalaxyEnemiesArmyController enemiesArmyController
        ,int startPower,List<SectorData> allSubSectors,int step)
    {
        var xx = x * step;
        var zz = z * step;
        var player = MainController.Instance.MainPlayer;
        var allies = player.ReputationData.Allies;
        var startSector = sectors[x, z];
        var canBeConquest = MyExtensions.IsTrue01(.33f);
        if (allies.HasValue)
        {
            canBeConquest = canBeConquest && config != allies.Value;
        }

        startSector = new SectorData(xx, zz, SizeOfSector, new Dictionary<GlobalMapEventType, int>(), config,
             x, powerPerTurn, to =>
            {

            }, enemiesArmyController, canBeConquest);

        startSector.Populate(startPower);
        sectors[x, z] = startSector;
        allSubSectors.Add(startSector);
        return startSector;
    }

}
