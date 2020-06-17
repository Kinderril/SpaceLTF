using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Free,
    Clouds,
    Asteroids,
    DeepSpace
}

public class AICellSegment : SegmentPoints
{
    private CellPoint _aCell;
    private CellPoint _bCell;

    public AICellSegment(CellPoint a, CellPoint b)
        : base(a.Position, b.Position)
    {
        _aCell = a;
        _bCell = b;
    }

    public CellPoint ACell()
    {
        return _aCell;
    }

    public CellPoint BCell()
    {
        return _bCell;
    }

    public override Vector3 A => _aCell.Position;
    public override Vector3 B => _bCell.Position;
}

public class CellDirection
{
    public Vector3 Dir;
    public float Angle;
    public int Index;
    public bool CanMove;

    public CellDirection(Vector3 Dir, float Angle, int Index, bool Val)
    {
        this.Dir = Dir;
        this.Angle = Angle;
        this.Index = Index;
        this.CanMove = Val;
    }
}


[Serializable]
public class AICell
{
    public const int SIDES_COUNT = 8;
    public const float ONE_TO_SIDES = 45f / 2f;
    const float ONE_TO_SQRT_TWO = 0.707106781186f;

    public CellType CellType = CellType.Free;
    public bool OutOfField = false;
    public int Xindex;
    public int Zindex;
    public Vector3 Center;
    public Vector3 Start;
    public Vector3 End;
    public float Side;
    public bool Marked { get; set; }

    public Dictionary<AICellSegment, AICell> CellsOnSides = new Dictionary<AICellSegment, AICell>();
    public Dictionary<int, CellDirection> PosibleDirections = new Dictionary<int, CellDirection>();

    private Dictionary<ShipBase, List<IShipDangerPoint>> _dangerPoints = new Dictionary<ShipBase, List<IShipDangerPoint>>();
    private HashSet<AIAsteroidPredata> asteroidsPreData = new HashSet<AIAsteroidPredata>();
    private HashSet<ShipDangerAnomalyPoint> anomalyDanger = new HashSet<ShipDangerAnomalyPoint>();
    public bool HaveDanger { get; private set; }

    public AICell(CellType cell, CellPoint start, CellPoint left, CellPoint right, CellPoint end, float side)
    {
        Side = side;
        //        CellPointStart = start;
        Start = start.Position;
        Xindex = start.indexX;
        Zindex = start.indexZ;
        CellType = cell;
        End = new Vector3(Start.x + side, Start.y, Start.z + side);
        Center = (Start + End) / 2f;

    }

    public void AddDangerPoint(Vector3 pos,float rad)
    {
        var anomaly = new ShipDangerAnomalyPoint(pos, rad);
        foreach (var asteroid in _dangerPoints)
        {
            asteroid.Value.Add(anomaly);
        }

        anomalyDanger.Add(anomaly);
    }

    public HashSet<AIAsteroidPredata> GetAllAsteroids()
    {
        return asteroidsPreData;
    }

    public void ClearPosition(Vector3 vector3)
    {
        HaveDanger = asteroidsPreData != null && asteroidsPreData.Count > 0;
        if (HaveDanger)
        {
            asteroidsPreData.RemoveWhere(cellASteroid =>
            {
                var sDist = (cellASteroid.Position - vector3).sqrMagnitude;
                return (sDist <= 9);
            });
        }
    }
    public void AddShip(ShipBase shipBase)
    {
        HaveDanger = asteroidsPreData.Count > 0 || anomalyDanger.Count > 0;
        var asteroidsCopy = new List<IShipDangerPoint>();
        if (HaveDanger)
        {
            foreach (var asteroidPredata in asteroidsPreData)
            {
                var cellASteroid = asteroidPredata;
                var asteroidShip = new ShipAsteroidPoint(cellASteroid);
                cellASteroid.OnDeath += () =>
                {
                    asteroidsCopy.Remove(asteroidShip);
                };
                asteroidsCopy.Add(asteroidShip);
            }

            foreach (var shipDangerAnomalyPoint in anomalyDanger)
            {
                asteroidsCopy.Add(shipDangerAnomalyPoint);
            }
            _dangerPoints.Add(shipBase, asteroidsCopy);
        }
        else
        {
            _dangerPoints.Add(shipBase, asteroidsCopy);
        }
    }

    [CanBeNull]
    public List<IShipDangerPoint> GetDangerPointsForShip(ShipBase owner)
    {
        if (HaveDanger)
        {
            return _dangerPoints[owner];
        }
        else
        {
            return null;
        }
    }

    public bool IsFree()
    {
        if (CellType == CellType.Clouds || CellType == CellType.Free)
        {
            return true;
        }
        return false;
    }



    private CellDirection ClosestDir(Vector3 normDir, float eulerAng)
    {
        if (normDir.x > 0)
        {
            if (normDir.z > 0)
            {
                if (Mathf.Abs(normDir.x) > Mathf.Abs(normDir.z))
                {
                    //1 or //2
                    if (eulerAng - 45 < 90 - eulerAng)
                    {
                        return PosibleDirections[1];
                    }
                    else
                    {
                        return PosibleDirections[2];
                    }
                }
                else
                {
                    //1 or //0
                    if (45 - eulerAng < eulerAng)
                    {
                        return PosibleDirections[1];
                    }
                    else
                    {
                        return PosibleDirections[0];
                    }
                }
            }
            else
            {
                if (Mathf.Abs(normDir.x) > Mathf.Abs(normDir.z))
                {
                    //2 or //3
                    if (eulerAng - 90 < 135 - eulerAng)
                    {
                        return PosibleDirections[2];
                    }
                    else
                    {
                        return PosibleDirections[3];
                    }
                }
                else
                {
                    //3 or //4
                    if (eulerAng - 135 < 180 - eulerAng)
                    {
                        return PosibleDirections[3];
                    }
                    else
                    {
                        return PosibleDirections[4];
                    }
                }
            }
        }
        else
        {
            if (normDir.z > 0)
            {
                if (Mathf.Abs(normDir.x) > Mathf.Abs(normDir.z))
                {
                    //6 or //7
                    if (eulerAng - 270 < 315 - eulerAng)
                    {
                        return PosibleDirections[6];
                    }
                    else
                    {
                        return PosibleDirections[7];
                    }
                }
                else
                {
                    //7 or //0
                    if (eulerAng - 315 < 360 - eulerAng)
                    {
                        return PosibleDirections[7];
                    }
                    else
                    {
                        return PosibleDirections[0];
                    }
                }
            }
            else
            {

                if (Mathf.Abs(normDir.x) > Mathf.Abs(normDir.z))
                {
                    //5 or //6
                    if (eulerAng - 225 < 270 - eulerAng)
                    {
                        return PosibleDirections[5];
                    }
                    else
                    {
                        return PosibleDirections[6];
                    }
                }
                else
                {
                    //4 or //5
                    if (eulerAng - 180 < 225 - eulerAng)
                    {
                        return PosibleDirections[4];
                    }
                    else
                    {
                        return PosibleDirections[5];
                    }
                }
            }
        }
    }

    public float CheckWantSlow(Vector3 curDir, Vector3 pos, float eulerAng)
    {
        float sppedPercent = 1f;
        var closestDir = ClosestDir(curDir, eulerAng);
        if (closestDir.CanMove)
        {
            return sppedPercent;
        }
        if (pos.x > Center.x)
        {
            if (pos.z > Center.z)//1
            {
                if (closestDir.Index == 0 || closestDir.Index == 1 || closestDir.Index == 2 || closestDir.Index == 3)
                {
                    sppedPercent = 0.2f;
                }
            }
            else//2
            {

                if (closestDir.Index == 2 || closestDir.Index == 3 || closestDir.Index == 4 || closestDir.Index == 1)
                {
                    sppedPercent = 0.2f;
                }
            }
        }
        else
        {
            if (pos.z > Center.z)//3
            {

                if (closestDir.Index == 0 || closestDir.Index == 6 || closestDir.Index == 7 || closestDir.Index == 5)
                {
                    sppedPercent = 0.2f;
                }
            }
            else//4
            {
                if (closestDir.Index == 4 || closestDir.Index == 5 || closestDir.Index == 6 || closestDir.Index == 7)
                {
                    sppedPercent = 0.2f;
                }
            }
        }
        return sppedPercent;
    }
    public Vector3 UpdatePosibleDirection(Vector3 wantedNormDir, float eulerAng)
    {

#if UNITY_EDITOR
        if (wantedNormDir.sqrMagnitude > 1.0001f)
        {
            Debug.LogError("UpdatePosibleDirection dir not normazzed " + wantedNormDir.sqrMagnitude);
        }
#endif


        return wantedNormDir;
    }

    public void AddAsteroid(AIAsteroidPredata asteroid, bool fromStart)
    {
        asteroidsPreData.Add(asteroid);
        HaveDanger = true;
        if (!fromStart)
        {
            foreach (var asteroid1 in _dangerPoints)
            {
                var cellASteroid = asteroid;
                var asteroidShip = new ShipAsteroidPoint(cellASteroid);
                cellASteroid.OnDeath += () =>
                {
                    asteroid1.Value.Remove(asteroidShip);
                };
                asteroid1.Value.Add(asteroidShip);
            }
        }
    }
    public void SetStubDirections()
    {
        if (PosibleDirections == null || PosibleDirections.Count == 0)
        {
            PosibleDirections = new Dictionary<int, CellDirection>();
            for (int i = 0; i < SIDES_COUNT; i++)
            {
                var ang = i * 360 / SIDES_COUNT;
                PosibleDirections.Add(i, new CellDirection(Vector3.up, ang, i, false));
            }
        }
    }

    public override string ToString()
    {
        return "X:" + Xindex + " Z:" + Zindex + " C:" + Center;
    }

    public void DrawGizmosSelected()
    {
        var h = Side;
        Color c = CellType == CellType.Free ? Color.white : Color.red;
        Gizmos.color = c;
        Gizmos.DrawWireCube(Center, new Vector3(h, 0.1f, h));
        //        foreach (var posibleDirection in PosibleDirections)
        //        {
        //            var d = posibleDirection.Value;
        //            DrawUtils.DebugArrow(Center,d.Dir,d.CanMove?Color.green : Color.red);
        ////            UnityEngine.Handheld.
        //        }

    }

}

