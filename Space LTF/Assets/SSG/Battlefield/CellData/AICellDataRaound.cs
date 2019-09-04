using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class AIAsteroidPredata
{
    public Vector3 Position;
    public float Rad;
    public event Action OnDeath;
    public const float SHIP_SIZE_COEF = 3f;

    public AIAsteroidPredata(Vector3 ateroidPos)
    {
        this.Position = ateroidPos;
        Rad = MyExtensions.Random(0.8f, 1.2f) * SHIP_SIZE_COEF;
    }

    public void Death()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }

    }
}

[Serializable]
public class AICellDataRaound
{
    public const float SafeRadius = 9; 
    private const float AsteroidRadius = 4; 
    private const float AsteroidINFieldMin = 3; 
    private const float AsteroidINFieldMax = 7; 
    public float CellSize;
    public float StartX;
    public float StartZ;

    public int MaxIx;
    public int MaxIz;

    [SerializeField]
    public AICell[,] List;

    public float Radius;

    public float InsideRadius => Radius - AICellDataRaound.SafeRadius;
    public Vector3 Min => new Vector3(StartX,0, StartZ);
    public Vector3 Max => new Vector3(StartX + CellSize* MaxIx, 0, StartZ + CellSize * MaxIz);
    public Vector3 CenterZone { get; private set; }
    public Vector3 StartPosition1 { get; private set; }
    public Vector3 StartPosition2 { get; private set; }

    public List<AIAsteroidPredata> Asteroids = new List<AIAsteroidPredata>();
    public List<Vector3> FreePoints = new List<Vector3>();

    public void Init(Vector3 startPos,int size,float cellSize)
    {
        Radius = size * cellSize / 2f - cellSize;
//        Debug.Log(String.Format("Cells inited. SizeX:{0}  SizeZ:{1}",sizeX,sizeZ).Red());
        Debug.Log($"Cells inited. SizeX:{size}  ".Red());
        CellSize = cellSize;
        StartX = startPos.x;
        StartZ = startPos.z;
//        var xxDelta = Mathf.Abs(end.x - start.x);
//        var zzDelta = Mathf.Abs(end.z - start.z);
        MaxIx = size;//(int)(xxDelta/CellSize) + 1;
        MaxIz = size;//(int)(zzDelta/CellSize) + 1;
        int cellX = MaxIx + 1;
        int cellZ = MaxIz + 1;
        CenterZone = (Max + Min) / 2f;

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

                var startCorner = cellsPoints[i, j];
                var leftCorner = cellsPoints[i, j+1];
                var rightCorner = cellsPoints[i+1, j];
                var endCorner = cellsPoints[i+1, j+1];
                var cell = new AICell(ct, startCorner, leftCorner, rightCorner, endCorner, cellSize);
                SetCell(i, j, cell);
            }
        }

        CalcStartPositinons();

        int asteroidsFieldCount = (int)((size + 6f) / 2f);
        Asteroids.Clear();
        CreateAsteroidsOnCircle(4);
        CreateRandomAsteroids(asteroidsFieldCount);
        FindFreeCorePoints();

    }

    private void FindFreeCorePoints()
    {
        var rad1 = InsideRadius * 0.3f;
        var rad2 = InsideRadius * 0.5f;
//        var rad3 = InsideRadius * 0.7f;
        var countSides = 16;
        var ang = 360f / countSides;
        var dir1 = new Vector3(1, 0, 0);
        var dir2 = new Vector3(1, 0, 0);
        for (int i = 0; i < countSides; i++)
        {

            dir1 = Utils.RotateOnAngUp(dir1, Utils.GreateRandom(ang));
            dir2 = Utils.RotateOnAngUp(dir2, -Utils.GreateRandom(ang));
            var p1 = CenterZone + dir1 * rad1;
            var p2 = CenterZone + dir2 * rad2;
            if (IsFree(p1))
            {
                FreePoints.Add(p1);
            }
            if (IsFree(p2))
            {
                FreePoints.Add(p2);
            }
        }

        if (FreePoints.Count < 3)
        {
            Debug.LogError("can't find enought free points");
            for (int i = 0; i < 10; i++)
            {
                var xx = MyExtensions.Random(-InsideRadius, InsideRadius);
                var zz = MyExtensions.Random(-InsideRadius, InsideRadius);
                var p = CenterZone + new Vector3(xx, 0, zz);
                FreePoints.Add(p);

            }
        }
    }

    private bool IsFree(Vector3 p2)
    {
        foreach (var asteroid in Asteroids)
        {
            var sDist = (asteroid.Position - p2).sqrMagnitude;
            if (sDist > 2 * 2)
            {
                return false;
            }
        }
        return true;
    }

    private void CalcStartPositinons()
    {
        var dir1 = new Vector3(0, 0, -1);
        var diifAng = MyExtensions.Random(-25, 25f);
        dir1 = Utils.RotateOnAngUp(dir1, diifAng);
        StartPosition1 = CenterZone + dir1 * InsideRadius;

        var dir2 = new Vector3(0, 0, 1);
        var diifAng2 = MyExtensions.Random(-25, 25f);
        dir2 = Utils.RotateOnAngUp(dir2, diifAng2);
        StartPosition2 = CenterZone + dir2 * InsideRadius;
    }

    private void CreateRandomAsteroids(int fieldsCount)
    {
//        AddAsteroidToPosition(CenterZone);
        var offsetCEnterField = InsideRadius - AsteroidRadius;
        for (int i = 0; i < fieldsCount; i++)
        {
            var findPointX = MyExtensions.Random(-offsetCEnterField, offsetCEnterField);
            var findPointZ = MyExtensions.Random(-offsetCEnterField, offsetCEnterField);
//            var point = new Vector3(findPointX, 0, findPointZ);
            var countAsteroids = MyExtensions.Random(AsteroidINFieldMin, AsteroidINFieldMax);
            for (int j = 0; j < countAsteroids; j++)
            {
                var xx = MyExtensions.Random(0.3f, AsteroidRadius);
                var zz = MyExtensions.Random(0.3f, AsteroidRadius);
                var ateroidPos = new Vector3(findPointX + xx, 0, findPointZ + zz);
                AddAsteroidToPosition(CenterZone + ateroidPos);
            }
        }
    }

    private void AddAsteroidToPosition(Vector3 ateroidPos)
    {
        var distTostart1 = (ateroidPos - StartPosition1).magnitude; 
        var distTostart2 = (ateroidPos - StartPosition2).magnitude;
        if (distTostart2 > 10 && distTostart1 > 10)
        {
            var asteroid = new AIAsteroidPredata(ateroidPos);
            Asteroids.Add(asteroid);
            //        StartX + CellSize * i
            var cellIx = (int)((ateroidPos.x - StartX) / CellSize);
            var cellIz = (int)((ateroidPos.z - StartZ) / CellSize);
            for (int i = Mathf.Clamp(cellIx - 1, 0, MaxIx); i < Mathf.Clamp(cellIx + 2, 0, MaxIx); i++)
            {
                for (int j = Mathf.Clamp(cellIz - 1, 0, MaxIz); j < Mathf.Clamp(cellIz + 2, 0, MaxIz); j++)
                {
                    var cell = GetCell(i, j);
                    cell.AddAsteroid(asteroid);
                    //                Debug.LogError($"Add asteroid to cells {cell.Xindex},{cell.Zindex}");
                }
            }
        }

        

    }

    private void CreateAsteroidsOnCircle(int iterations)
    {
        var minRad = (Radius - SafeRadius) * 1.2f;
        for (int j = 0; j < iterations; j++)
        {
            var countSTeps = 40;
            var dir = new Vector3(1, 0, 0);

            var step = (int)(360 / countSTeps);
            for (int i = 0; i < countSTeps; i++)
            {
                dir = Utils.RotateOnAngUp(dir, MyExtensions.GreateRandom(step));
                var point = CenterZone + dir * MyExtensions.Random(minRad, Radius*1.1f);
                var asteroid = new AIAsteroidPredata(point);
                Asteroids.Add(asteroid);
            }
        }

        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var cell = GetCell(i, j);
                var cellCenter = cell.Center;
                var distToCenter = (CenterZone - cellCenter).magnitude;
                if (distToCenter > minRad)
                {
                    cell.CellType = CellType.Asteroids;
                }
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

    public void AddShip(ShipBase shipBase)
    {
        for (int i = 0; i < MaxIx; i++)
        {
            for (int j = 0; j < MaxIz; j++)
            {
                var point = List[i, j];
                point.AddShip(shipBase);
            }
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

