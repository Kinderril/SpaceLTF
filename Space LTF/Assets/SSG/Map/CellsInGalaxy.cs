using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CellsInGalaxy
{

    private GlobalMapCell[,] cells;
//    private List<GlobalMapCell> cellsList;
    private List<SectorCellContainer> cellsContainers;
    public int SizeX { get; private set; }
    public int SizeZ { get; private set; }

    public CellsInGalaxy(int sizeX, int sizeZ)
    {
        SizeX = sizeX;
        SizeZ = sizeZ;
        cells = new GlobalMapCell[SizeX, SizeZ];
//        cellsList = new List<GlobalMapCell>(sizeX * sizeZ);
        cellsContainers = new List<SectorCellContainer>(sizeX * sizeZ);
    }
    public List<SectorCellContainer> GetAllContainers()
    {
        return cellsContainers;
    }

//    public void SetCell(int x, int z, GlobalMapCell cell)
//    {
//        cells[x, z] = cell;
//        cellsList.Add(cell);
//    }

    public void FindNoWayCells()
    {
        var copyCells = new HashSet<SectorCellContainer>();
        foreach (var globalMapCell in cellsContainers)
        {
            if (globalMapCell != null && !(globalMapCell.Data is GlobalMapNothing) && !globalMapCell.Data.IsHide)
                copyCells.Add(globalMapCell);
        }

        foreach (var globalMapCell in cellsContainers)
        {
            if (globalMapCell != null && !(globalMapCell.Data is GlobalMapNothing) && !globalMapCell.Data.IsHide)
            {
                var allWays = globalMapCell.GetCurrentPosibleWays();
                foreach (var way in allWays)
                {
                    copyCells.Remove(way);
                }
            }
        }

        foreach (var globalMapCell in copyCells)
        {
            var nearestCell = GetNearestCell(globalMapCell);
            if (nearestCell != null)
                globalMapCell.AddWay(nearestCell);
        }
    }

    private SectorCellContainer GetNearestCell(SectorCellContainer globalMapCell)
    {
        int dist = Int32.MaxValue;
        SectorCellContainer tmpCel = null;


        foreach (var data in cellsContainers)
        {
            var cell = data.Data;
            if (!(cell is GlobalMapNothing) && !cell.IsHide && cell.Container != globalMapCell)
            {
                var d = Mathf.Abs(globalMapCell.indZ - cell.indZ) + Mathf.Abs(globalMapCell.indX - cell.indX);
                if (d < dist)
                {
                    d = dist;
                    tmpCel = data;
                }
            }
        }

        return tmpCel;

    }


    public void SetCell(GlobalMapCell cell)
    {
        try
        {
            cells[cell.indX, cell.indZ] = cell;
//            cellsList.Add(cell);
            cellsContainers.Add(cell.Container);
        }
        catch (Exception e)
        {
            if (cell != null)
                Debug.LogError($"can't implement sector  cell.indX:{cell.indX}  cell.indZ:{cell.indZ}   ");
            else
            {

                Debug.LogError($"can't implement sector  ");
            }
        }
    }

    public GlobalMapCell GetCell(int x, int z)
    {
        return cells[x, z];
    }

    public SectorCellContainer GetRandom()
    {
        return cellsContainers.RandomElement();
    }

    public GlobalMapCell[,] GetAllCells()
    {
        return cells;
    }


    public GlobalMapCell GetRandomClosestCellWithNoData(ShipConfig config, int indX, int indZ)
    {
        int minLengh = 9999;
        GlobalMapCell cellOut = null;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeZ; j++)
            {
                var cell = cells[i, j];
                if (cell != null && !cell.IsScouted && !cell.Completed)
                {
                    var armyCell = cell as ArmyGlobalMapCell;
                    if (armyCell != null && armyCell.GetConfig() == config)
                    {
                        var xx = Mathf.Abs(indX - cell.indX);
                        var zz = Mathf.Abs(indZ - cell.indZ);
                        var dist = (xx + zz);
                        if (dist < minLengh && dist > 0)
                        {
                            minLengh = dist;
                            cellOut = cell;
                        }
                    }
                }
            }
        }
        return cellOut;
    }

}