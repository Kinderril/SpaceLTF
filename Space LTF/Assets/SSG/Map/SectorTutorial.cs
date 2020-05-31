using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorTutorial : SectorData
{
    private StartGlobalCell _startGlobalCell;
    public StartGlobalCell StartCell  => _startGlobalCell;

    public SectorTutorial(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int index, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig, index, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {


    }

    private void AddWays(GlobalMapCell c1, GlobalMapCell c2)
    {
        c1.AddWay(c2);
        c2.AddWay(c1);
    }



    public override void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        Name = Namings.ShipConfig(_shipConfig);
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;// CalcCellPower(0, Size, startPowerGalaxy, 0);
        // RandomizeBorders();
        PopulateToSide();
    }

    private void PopulateToSide()
    {
        ListCells = new HashSet<SectorCellContainer>();

        //START CELL
        var startCEll = new StartGlobalCell(1,StartX,0,this,ShipConfig);
        var cellContainer = Cells[StartX, 1];
        cellContainer.SetData(startCEll);
        ListCells.Add(cellContainer);
        _startGlobalCell = startCEll;

      //BATTLE CELL     
      var battle1CEll = new ArmyTutorGlobalCell(1,true,1, ShipConfig,2,StartX + 1, 0, this );
        var cellContainerBattle1 = Cells[StartX + 1, 1];
        cellContainerBattle1.SetData(battle1CEll);
        ListCells.Add(cellContainerBattle1);

        AddWays(battle1CEll, startCEll);

        //SHOP CELL
        var shopCEll = new TutorShopGlobalMapCell(30,3,StartX+2,0,this,ShipConfig);
        var cellContainerShop = Cells[StartX + 2, 1];
        shopCEll.ClearShop();
        shopCEll.AddItem(WeaponType.laser);
        cellContainerShop.SetData(shopCEll);
        ListCells.Add(cellContainerShop);

        AddWays(battle1CEll, shopCEll);

        //BATTLE CELL2     
        var battle2CEll = new ArmyTutorGlobalCell(2,false,1, ShipConfig, 4, StartX + 3, 0, this);
        var cellContainerBattle2 = Cells[StartX + 3, 1];
        cellContainerBattle2.SetData(battle2CEll);
        ListCells.Add(cellContainerBattle2);

        AddWays(battle2CEll, shopCEll);

        //END TUTOR    
        var end = new EndTutorGlobalCell(1, 5, 0,  this);
        var cellContainerEnd = Cells[StartX + 4, 1];
        cellContainerEnd.SetData(end);
        ListCells.Add(cellContainerEnd);


        AddWays(battle2CEll, end);
    }

    public override void CacheWays()
    {

    }
}