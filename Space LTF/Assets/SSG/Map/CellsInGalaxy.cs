using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

[System.Serializable]
public class CellsInGalaxy
{

    private GlobalMapCell[,] cells;
    private List<GlobalMapCell> cellsList;
    public int Size { get; private set; }

    public CellsInGalaxy(int size)
    {
        Size = size;
        cells = new GlobalMapCell[size, size];
        cellsList = new List<GlobalMapCell>(size);
    }
    public List<GlobalMapCell> GetAllList()
    {
        return cellsList;
    }

    public void SetCell(int x, int z, GlobalMapCell cell)
    {
        cells[x, z] = cell;
        cellsList.Add(cell);
    }

    public void SetCell(GlobalMapCell cell)
    {
        cells[cell.indX, cell.indZ] = cell;
        cellsList.Add(cell);
    }

    public GlobalMapCell GetCell(int x, int z)
    {
        return cells[x, z];
    }

    public GlobalMapCell GetRandom()
    {
        return cellsList.RandomElement();
    }

    public GlobalMapCell[,] GetAllCells()
    {
        return cells;
    }
    public GlobalMapCell GetRandomConnectedCell()
    {
        var cell = cellsList.Where(x => x.ConnectedGates > 0 && !x.IsScouted && !x.Completed).ToList();
        var rnd = cell.RandomElement();
        return rnd;
    }

    public GlobalMapCell GetRandomClosestCellWithNoData(ShipConfig config, int indX, int indZ)
    {
        int minLengh = 9999;
        GlobalMapCell cellOut = null;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var cell = cells[i, j];
                if (!cell.IsScouted && !cell.Completed)
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