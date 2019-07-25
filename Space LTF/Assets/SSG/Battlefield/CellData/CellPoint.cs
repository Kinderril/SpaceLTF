using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CellPoint
{
    public int indexX;
    public int indexZ;
    public Vector3 Position;
    public CellPoint[] ConnectedPoints = new CellPoint[4]; 
//    public AICell[] ConnectedCells = new AICell[4];

    Dictionary<AICell,AICell[]> _otherCells = new Dictionary<AICell, AICell[]>();
    //Пусть 0 - ближайшая по Х
    //Пусть 1 - ближайшая по Z
    //Пусть 2 - по диагонали

    public CellPoint(int indexX,int indexZ,Vector3 Position)
    {
        this.indexX = indexX;
        this.indexZ = indexZ;
        this.Position = Position;
    }



    public void SetData(AICell[] cells, CellPoint[] connectedPoints)
    {
        ConnectedPoints = connectedPoints;

//        ConnectedCells = cells;
        for (int i = 0; i < cells.Length; i++)
        {
            var cMain = cells[i];
            AICell[] orderedCells = new AICell[3];
            _otherCells.Add(cMain,orderedCells);
            for (int j = 0; j < cells.Length; j++)
            {
                if (i != j)
                {
                    var cTest = cells[j];
                    int indexToAdd = 0;
                    var deltaX = Mathf.Abs(cTest.Center.x - cMain.Center.x) > 0.001f;
                    var deltaZ = Mathf.Abs(cTest.Center.z - cMain.Center.z) > 0.001f;
                    if (deltaX && deltaZ)
                    {
                        indexToAdd = 2;
                    }
                    else
                    {
                        if (deltaX)
                        {
                            indexToAdd = 0;
                        }
                        else
                        {
                            indexToAdd = 1;
                        }
                    }
                    orderedCells[indexToAdd] = cTest;
                }
            }
        }
    }

    public AICell[] CellsByCell(AICell core)
    {
        return _otherCells[core];
    }


}

