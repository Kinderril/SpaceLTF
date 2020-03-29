﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CellsInGalaxy
{

    private GlobalMapCell[,] cells;
    private List<GlobalMapCell> cellsList;
    public int SizeX { get; private set; }
    public int SizeZ { get; private set; }

    public CellsInGalaxy(int sizeX, int sizeZ)
    {
        SizeX = sizeX;
        SizeZ = sizeZ;
        cells = new GlobalMapCell[SizeX, SizeZ];
        cellsList = new List<GlobalMapCell>(sizeX * sizeZ);
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
        try
        {
            cells[cell.indX, cell.indZ] = cell;
            cellsList.Add(cell);
        }
        catch (Exception e)
        {
            Debug.LogError("can't implement sector");
        }
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