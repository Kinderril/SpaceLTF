using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorCampaignStart : SectorData
{
    private StartGlobalCell _startGlobalCell;
    public StartGlobalCell StartCell  => _startGlobalCell;

    public SectorCampaignStart(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig,  xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {


    }


    public override void BornArmies()
    {
//        foreach (var cell in ListCells)
//        {
//            var born = cell.Data as FreeActionGlobalMapCell;
//            if (born != null)
//            {
//                born.BornArmy();
//            }
//        }
    }

    public override void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        Name = Namings.ShipConfig(_shipConfig);
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;
        PopulateToSide();
    }

    private void PopulateToSide()
    {
        ListCells = new HashSet<SectorCellContainer>();

        var startX = StartX - Size;
        var startZ = StartZ;


        //START CELL
        var startCEll = new CampaingStartGlobalCell(1, startX + Size, startZ, this,ShipConfig,0);
        var cellContainer = Cells[startX , startZ];
        cellContainer.SetData(startCEll);
        ListCells.Add(cellContainer);
        _startGlobalCell = startCEll;

        //BATTLE CELL
        startX++;
        startZ++;
        var battle1CEll = new FreeActionGlobalMapCell(StartPowerGalaxy, ShipConfig, 2, startX + Size, startZ, this ,_enemiesArmyController,_powerPerTurn);
        var cellContainerBattle1 = Cells[startX , startZ];
        cellContainerBattle1.SetData(battle1CEll);
        ListCells.Add(cellContainerBattle1);

        startCEll.AddWay(battle1CEll.Container);


        //BATTLE CELL           
        //        startX++;
        startZ++;
        var battle1CEll2 = new FreeActionGlobalMapCell(StartPowerGalaxy, ShipConfig, 3, startX + Size, startZ, this, _enemiesArmyController, _powerPerTurn);
        var cellContainerBattle2 = Cells[startX, startZ];
        cellContainerBattle2.SetData(battle1CEll2);
        ListCells.Add(cellContainerBattle2);


        AddWays(battle1CEll2, battle1CEll);

        //BATTLE CELL            
        startX++;
        var battle1CEll3 = new FreeActionGlobalMapCell(StartPowerGalaxy, ShipConfig, 4, startX + Size, startZ, this, _enemiesArmyController, _powerPerTurn);
        var cellContainerBattle3 = Cells[startX , startZ];
        cellContainerBattle3.SetData(battle1CEll3);
        ListCells.Add(cellContainerBattle3);

        AddWays(battle1CEll2, battle1CEll3);
    }

    public override void CacheWays()
    {

    }
}