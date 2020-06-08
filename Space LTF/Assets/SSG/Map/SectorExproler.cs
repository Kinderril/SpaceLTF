using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorExproler : SectorData
{

    private StartGlobalCell _startGlobalCell;
    public StartGlobalCell StartCell => _startGlobalCell;
    public SectorExproler(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int index, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig, index, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {

    }

    private void CreateStart()
    {

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var testCell = Cells[i, j];
                if (testCell.Data is GlobalMapNothing)
                {
                    continue;
                }
                var startCEll = new StartGlobalCell(999999, i, j, this, ShipConfig);
                var cellContainer = Cells[i, j];
                cellContainer.SetData(startCEll);
                ListCells.Add(cellContainer);
                _startGlobalCell = startCEll;
                return;
            }
        }
    }

    private void CreateEnd()
    {
        for (int i = Size-1; i >= 0; i--)
        {
            for (int j = Size-1; j >= 0; j--)
            {

                var testCell = Cells[i, j];
                if (testCell.Data is GlobalMapNothing)
                {
                    continue;
                }
                var endCEll = new EndExprolerGlobalCell(StartPowerGalaxy, 999998, i, j, this);
                var endCellContainer = Cells[i, j];
                endCellContainer.SetData(endCEll);
                ListCells.Add(endCellContainer);
                return;
            }

        }
    }

    protected override void PrePopulate()
    {
        CreateStart();
        CreateEnd();
    }
}