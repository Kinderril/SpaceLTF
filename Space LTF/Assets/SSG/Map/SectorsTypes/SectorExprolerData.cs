using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class SectorExprolerData : SectorData
{

    public abstract StartGlobalCell StartCell { get; }//=> _startGlobalCell


    protected SectorExprolerData(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents, ShipConfig shipConfig, 
        int index, int xIndex, float powerPerTurn, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController) 
        : base(startX, startZ, size, maxCountEvents, shipConfig, index, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {

    }

    protected override ShipConfig IsDroids(ShipConfig coreConfig)
    {
        return coreConfig;
    }

    protected StartGlobalCell CreateStart()
    {

        var zzStart = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = zzStart; j < Size+ zzStart; j++)
            {
                var testCell = Cells[i, j];
                if (testCell.Data is GlobalMapNothing)
                {
                    continue;
                }
                var indZ = StartZ + j;
                var startCEll = new StartGlobalCell(999999, i, indZ, this, ShipConfig);
                var cellContainer = Cells[i, j];
                cellContainer.SetData(startCEll);
                ListCells.Add(cellContainer);
                return startCEll;
            }
        }

        return null;
    }

    protected GlobalMapCell CreateEnd()
    {
        var zzStart = 0;
        for (int i = Size - 1; i >= 0; i--)
        {
            for (int j = Size - 1 + zzStart; j >= zzStart; j--)
            {

                var testCell = Cells[i, j];
                if (testCell.Data is GlobalMapNothing)
                {
                    continue;
                }

                var indZ = StartZ + j;
                var endCEll = new EndExprolerGlobalCell(StartPowerGalaxy, 999998, i, indZ, this);
                var endCellContainer = Cells[i, j];
                endCellContainer.SetData(endCEll);
                ListCells.Add(endCellContainer);
                return endCEll;
            }

        }
        return null;
    }
}
