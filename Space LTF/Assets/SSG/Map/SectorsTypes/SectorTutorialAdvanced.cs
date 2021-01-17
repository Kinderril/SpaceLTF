using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorTutorialAdvanced : SectorData
{
    private StartGlobalCell _startGlobalCell;
    public StartGlobalCell StartCell  => _startGlobalCell;

    public SectorTutorialAdvanced(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback,GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {


    }

    private void AddWays(GlobalMapCell c1, GlobalMapCell c2)
    {
        c1.AddWay(c2.Container);
        c2.AddWay(c1.Container);
    }



    public override void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        Name = Namings.ShipConfig(_shipConfig);
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;//CalcCellPower(0, Size, startPowerGalaxy, 0);
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

        //UPGRADE INVE CELL    
        var upgradeMainShipTutorGlobalCell = new UpgradeMainShipTutorGlobalCell(1, StartX + 1, 0, this, ShipConfig);
        var cellContainerBattleMainShipUpgrade = Cells[StartX + 1, 1];
        cellContainerBattleMainShipUpgrade.SetData(upgradeMainShipTutorGlobalCell);
        ListCells.Add(cellContainerBattleMainShipUpgrade);

        AddWays(upgradeMainShipTutorGlobalCell, startCEll);

        //BATTLE CELL1     
        var armyTutorAutoFightGlobalCell = new ArmyTutorAutoFightGlobalCell(2, ShipConfig, 2, StartX + 2, 0, this);
        var containerArmyTutorAutoFightGlobalCell = Cells[StartX + 2, 1];
        containerArmyTutorAutoFightGlobalCell.SetData(armyTutorAutoFightGlobalCell);
        ListCells.Add(containerArmyTutorAutoFightGlobalCell);

        AddWays(upgradeMainShipTutorGlobalCell, armyTutorAutoFightGlobalCell);


        //WEAR DETAILS   
        var wearDetailsTutorGlobalCell = new WearDetailsTutorGlobalCell(3, StartX + 3, 0, this, ShipConfig);
        var containerwearDetailsTutorGlobalCell = Cells[StartX +3, 1];
        containerwearDetailsTutorGlobalCell.SetData(wearDetailsTutorGlobalCell);
        ListCells.Add(containerwearDetailsTutorGlobalCell);

        AddWays(wearDetailsTutorGlobalCell, armyTutorAutoFightGlobalCell);

        //BATTLE CELL2     
        var tutorIgnoreShieldGlobalCell = new ArmyTutorIgnoreShieldGlobalCell(2, ShipConfig, 4, StartX + 4, 0, this);
        var containertutorIgnoreShieldGlobalCell = Cells[StartX + 4, 1];
        containertutorIgnoreShieldGlobalCell.SetData(tutorIgnoreShieldGlobalCell);
        ListCells.Add(containertutorIgnoreShieldGlobalCell);

        AddWays(tutorIgnoreShieldGlobalCell, wearDetailsTutorGlobalCell);
        
        //END TUTOR    
        var end = new EndTutorGlobalCell(1, StartX + 5, 0,  this);
        var cellContainerEnd = Cells[StartX + 5, 1];
        cellContainerEnd.SetData(end);
        ListCells.Add(cellContainerEnd);

        AddWays(tutorIgnoreShieldGlobalCell, end);
    }

    public override void CacheWays()
    {

    }
}