using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class AICellData
{
    public float CellSize;
    public float StartX;
    public float StartZ;

    public int MaxIx;
    public int MaxIz;

    [SerializeField]
    public AICell[,] List;

    public Vector3 Min
    {
        get { return new Vector3(StartX,0, StartZ); }
    }
    public Vector3 Max
    {
        get { return new Vector3(StartX + CellSize* MaxIx, 0, StartZ + CellSize * MaxIz); }
    }

    public void Init(Vector3 startPos,int sizeX,int sizeZ,float cellSize)
    {
        Debug.Log(String.Format("Cells inited. SizeX:{0}  SizeZ:{1}",sizeX,sizeZ).Red());
        CellSize = cellSize;
        StartX = startPos.x;
        StartZ = startPos.z;
//        var xxDelta = Mathf.Abs(end.x - start.x);
//        var zzDelta = Mathf.Abs(end.z - start.z);
        MaxIx = sizeX;//(int)(xxDelta/CellSize) + 1;
        MaxIz = sizeZ;//(int)(zzDelta/CellSize) + 1;
        int cellX = MaxIx + 1;
        int cellZ = MaxIz + 1;

        CellPoint[,] cellsPoints = new CellPoint[cellX, cellZ];
        List = new AICell[MaxIx, MaxIz];
        //Create Poitns
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var startPoint = new Vector3(StartX + CellSize * i, 0, StartZ + cellSize * j);
                CellPoint point = new CellPoint(i, j, startPoint);
                cellsPoints[i, j] = point;
            }
        }
        //SUB CREATE POINTS
        for (int i = 0; i < MaxIx; i++)
        {
            var startPoint = new Vector3(StartX + CellSize * i, 0, StartZ + cellSize * MaxIz);
            CellPoint point = new CellPoint(i, MaxIz, startPoint);
            cellsPoints[i, MaxIz] = point;
        }
        for (int i = 0; i < MaxIz; i++)
        {
            var startPoint = new Vector3(StartX + CellSize * MaxIx, 0, StartZ + cellSize * i);
            CellPoint point = new CellPoint(MaxIx, i, startPoint);
            cellsPoints[MaxIx, i] = point;
        }
        //Add last point
        var startPoint2 = new Vector3(StartX + CellSize * MaxIx, 0, StartZ + cellSize * MaxIz);
        CellPoint point2 = new CellPoint(MaxIx, MaxIz, startPoint2);
        cellsPoints[MaxIx, MaxIz] = point2;



        //Create cells
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                CellType ct = CellType.Free;
                bool outOfField = false;
                if (i == 0 || i == MaxIx-1 || j == 0 || j == MaxIz-1)
                {
                    outOfField = true;
                    ct = CellType.DeepSpace;
                }
                else
                {
                    if (i == 1 || i == MaxIx - 2 || j == 1 || j == MaxIz - 2)
                    {
                        outOfField = true;
                        ct = CellType.Asteroids;
                    }
                }
                var startCorner = cellsPoints[i, j];
                var leftCorner = cellsPoints[i, j+1];
                var rightCorner = cellsPoints[i+1, j];
                var endCorner = cellsPoints[i+1, j+1];
                var cell = new AICell(ct, startCorner, leftCorner, rightCorner, endCorner, cellSize);
                cell.OutOfField = outOfField;
                SetCell(i, j, cell);
            }
        }



        for (int i = 1; i < cellX-1; i++)
        {
            for (int j = 1; j < cellZ-1; j++)
            {
                var cUp = cellsPoints[i, j + 1];
                var cDown = cellsPoints[i, j - 1];
                var cLeft = cellsPoints[i-1, j];
                var cRight = cellsPoints[i+1, j];
                //MUST BE IN RIGHT ORDERED !!!
                CellPoint[] points = new CellPoint[]
                {
                    cRight,cUp,cLeft,cDown,
                };
                var upCell = List[i, j - 1];
                var downCell = List[i-1, j];
                var diagCell = List[i-1, j - 1];
                var currCell = List[i, j];

                AICell[] list = new AICell[]
                {
                    upCell,downCell,diagCell,currCell
                };
                var tested = cellsPoints[i, j];
                tested.SetData(list, points);
            }
        }

        //Соотношение зависимости границ к соседним клеткам
        for (int i = 1; i < MaxIx - 1; i++)
        {
            for (int j = 1; j < MaxIz - 1; j++)
            {
                var c = GetCell(i, j);
                c.AddCellOfBorder(c.Border1, GetCell(i - 1, j));
                c.AddCellOfBorder(c.Border2, GetCell(i, j + 1));
                c.AddCellOfBorder(c.Border3, GetCell(i + 1, j));
                c.AddCellOfBorder(c.Border4, GetCell(i, j - 1));

            }
        }

        int cloudsCount = (int)MyExtensions.Random(0, 1);
        int asteroidsCount = 1 + (int)MyExtensions.Random(1, 4);
        AddTypes(CellType.Asteroids, asteroidsCount);
        AddTypes(CellType.Clouds, cloudsCount);

        //Добавить направления возможностей
        for (int i = 1; i < MaxIx - 1; i++)
        {
            for (int j = 1; j < MaxIz - 1; j++)
            {
                var cell = GetCell(i, j);
                cell.ImplementBorderDirection(GetCell(i    , j + 1), 0);//up
                cell.ImplementBorderDirection(GetCell(i + 1, j + 1), 1);//up right
                cell.ImplementBorderDirection(GetCell(i + 1, j    ), 2);//right
                cell.ImplementBorderDirection(GetCell(i + 1, j - 1), 3);//down right
                cell.ImplementBorderDirection(GetCell(i    , j - 1), 4);//down
                cell.ImplementBorderDirection(GetCell(i - 1, j - 1), 5);//down left
                cell.ImplementBorderDirection(GetCell(i - 1, j    ), 6);//left
                cell.ImplementBorderDirection(GetCell(i - 1, j + 1), 7);//up left
            }
        }
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var cell = GetCell(i, j);
                cell.SetStubDirections();
            }
        }
    }

    private void AddTypes(CellType type, int count)
    {
        int offset = 1;
        for (int i = 0; i < count; i++)
        {
            var delta = (MaxIx- offset)/count;
            var d = offset + delta*i;
            var rndX = Math.Abs((int) MyExtensions.Random(d, d + delta) -1);
            var rndZ = (int) MyExtensions.Random(offset, MaxIz - offset);
            var cell = GetCell(rndX, rndZ);
            int dist;
            var closest = FindClosestCellByType(cell, CellType.Free, true, out dist);
//            FindClosestCellByType(cell, );
            if (closest != null)
            {
                closest.CellType = type;
                closest.Marked = true;
                var cellsAround = GetCellsAround(closest);
                foreach (var aiCell in cellsAround)
                {
                    aiCell.Marked = true;
                }
            }
            else
            {
                Debug.LogError("can't find free cell");
            }
        }
    }

    public void SetCell(int i, int j, AICell cell)
    {
        List[i,j] = cell;
    }

    public AICell GetCell(int i, int j)
    {
#if  UNITY_EDITOR
        try
        {
            var a = List[i, j];
        }
        catch (Exception e)
        {
            Debug.LogError("wrong gettin element " + i + "  " + j + "   " + CellSize);
        }
#endif
        return List[i,j];
    }

    public void ValidateOnStart()
    {
        if (CellSize < 3)
        {
            Debug.LogError("cell controller ERROR: cell size is wrong. RECALCULATE!!!");
        }
        if (MaxIx < 3)
        {
            Debug.LogError("cell controller ERROR: MaxIx is wrong. RECALCULATE!!!");
        }
        if (MaxIz < 3)
        {
            Debug.LogError("cell controller ERROR: MaxIz is wrong. RECALCULATE!!!");
        }
        if (List.Length < 3)
        {
            Debug.LogError("cell controller ERROR: List is wrong. RECALCULATE!!!");
        }
    }

    public void SetCellSize(float val)
    {
        if (val < 3)
        {
            Debug.LogError("Cell cell size wrong. Set to 10. min cell siz = 3");
            val = 10f;
        }
        CellSize = val;
    }

    [CanBeNull]
    public AICell FindClosestCellByType(AICell cell, CellType celType)
    {
        int d;
        return FindClosestCellByType(cell, celType,false, out d);
    }

    [CanBeNull]
    public List<AICell> FindClosestCellsByType(AICell cell, CellType celType)
    {
        var distances = new List<KeyValuePair<AICell, int>>();
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var tested = List[i,j];
                if (tested.CellType == celType)
                {
                    var tdist = Mathf.Abs(tested.Xindex - cell.Xindex) + Mathf.Abs(tested.Zindex - cell.Zindex);
                    distances.Add(new KeyValuePair<AICell, int>(tested, tdist));
                }
            }
        }
        var ordered = distances.OrderBy(pair => pair.Value);

        List<AICell> cells = new List<AICell>();
        foreach (var keyValuePair in ordered)
        {
            cells.Add(keyValuePair.Key);
        }
        return cells;
    }

    private AICell[] GetCellsAround(AICell cell)
    {
        var xI = cell.Xindex;
        var zI = cell.Zindex;
        AICell[] cellsTo = new AICell[8]
        {
            GetCell(xI - 1, zI),
            GetCell(xI - 1, zI - 1),
            GetCell(xI,     zI - 1),
            GetCell(xI + 1, zI - 1),
            GetCell(xI + 1, zI),
            GetCell(xI + 1, zI + 1),
            GetCell(xI,     zI + 1),
            GetCell(xI - 1, zI + 1),
        };
        return cellsTo;
    }

    public AICell FindClosestCellByType(AICell cell, CellType celType, bool checkMarked,out int dist)
    {
        AICell closest = null;
        int iDist = Int32.MaxValue;
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var tested = List[i, j];
                bool checkIsOk = true;
                if (checkMarked)
                {
                    checkIsOk = !tested.Marked;
                }
                if (tested.CellType == celType && checkIsOk)
                {
                    var tdist = Mathf.Abs(tested.Xindex - cell.Xindex) + Mathf.Abs(tested.Zindex - cell.Zindex);
                    if (tdist < iDist)
                    {
                        iDist = tdist;
                        closest = tested;
                    }
                }
            }
        }
        dist = iDist;
        return closest;
    }

}

