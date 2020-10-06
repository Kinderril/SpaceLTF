using System.Collections.Generic;


[System.Serializable]
public class SectorExprolerMilitary : SectorExprolerData
{

    private StartGlobalCell _startGlobalCell;
    public override StartGlobalCell StartCell => _startGlobalCell;
    public override bool AnyEvent => true;
    public SectorExprolerMilitary(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {

    }

    protected override SectorDataEventsCounts GetCountOfEvents(int remainCells, int sectorSize)
    {
        return SectorDataEventsCounts.Exproler(remainCells, sectorSize);
    }
    protected override void PrePopulate()
    {

    }

    public override void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        Name = Namings.ShipConfig(_shipConfig);
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;
        // RandomizeBorders(); 
        _startGlobalCell = CreateStartCell();
        
        var prevCell = PopulateToSide(_startGlobalCell);
        var endCell = CreateEnd();
        ConnectCells(endCell, prevCell);
    }

    private GlobalMapCell PopulateToSide(GlobalMapCell startCell)
    {
        ListCells = new HashSet<SectorCellContainer>();
        GlobalMapCell lastCell = startCell;
        bool prevThisLevel = false;
        for (int i = startCell.indX + 1; i < Size; i++)
        {
            if (i == 0 || i == Size - 1)
            {
                var rndIndex = MyExtensions.Random(0, Size - 1);
//                if (startCell.indX != i && startCell.indZ != rndIndex)
//                {
                    var cell = PopulateCell(i, rndIndex);
                    if (lastCell != null && cell != null)
                    {
                        ConnectCells(cell, lastCell);
                        //                    Debug.LogError($":Link {_lastCell.indX}.{_lastCell.indZ} <> {cell.indX}.{cell.indZ}");
                    }
                    lastCell = cell;
//                }
            }
            else
            {
                bool thisLevel = MyExtensions.IsTrueEqual();
                if (prevThisLevel)
                {
                    thisLevel = false;
                }
                var rndIndex = MyExtensions.Random(0, Size - 1);
//                if (startCell.indX != i && startCell.indZ != rndIndex)
//                {
                    var cell = PopulateCell(i, rndIndex);
                    if (lastCell != null && cell != null)
                    {
                        ConnectCells(cell, lastCell);
                        //                    Debug.LogError($":Link {_lastCell.indX}.{_lastCell.indZ} <> {cell.indX}.{cell.indZ}");
                        lastCell = cell;
                    }

                    if (thisLevel)
                    {
                        i--;
                    }

                    prevThisLevel = thisLevel;
//                }


            }

        }

        return lastCell;
    }

    private void ConnectCells(GlobalMapCell cell1,GlobalMapCell cell2)
    {

        cell1.AddWay(cell2);
        cell2.AddWay(cell1);
    }

    public ArmyGlobalMapCell PopulateCell(int j, int i)
    {
        ArmyGlobalMapCell armyCellcell = null;
        var cellContainer = Cells[i, j];

        if (armyCellcell == null)
        {
            armyCellcell = new ArmyDungeonGlobalMapCell(_power, _shipConfig, Utils.GetId(),
                StartX + cellContainer.indX, StartZ + cellContainer.indZ, this, 0.05f,1.5f);
        }
        cellContainer.SetData(armyCellcell);
        ListCells.Add(cellContainer);
        return armyCellcell;
    }


    public override void CacheWays()
    {

    }

}