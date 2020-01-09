using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
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
    public const float ONE_TO_SIDES = 45f/2f;
    const float ONE_TO_SQRT_TWO = 0.707106781186f;

    public CellType CellType = CellType.Free;
    public bool OutOfField = false;
    public int Xindex;
    public int Zindex;
    public Vector3 Center;
    public Vector3 Start;
//    public CellPoint CellPointStart;
    public Vector3 End;
    public float Side;
    //    public SegmentPoints Cross1;
    //    public SegmentPoints Cross2;
    public bool Marked { get; set; }

    public AICellSegment Border1;
    public AICellSegment Border2;
    public AICellSegment Border3;
    public AICellSegment Border4;
    public AICellSegment[] Borders = new AICellSegment[4];

    public CellPoint c1;
    public CellPoint c2;
    public CellPoint c3;
    public CellPoint c4;
    public Dictionary<AICellSegment,AICell> CellsOnSides = new Dictionary<AICellSegment, AICell>();
    public Dictionary<int, CellDirection> PosibleDirections = new Dictionary<int, CellDirection>(); 
 
    private CellPoint[] _arrayPoints;
    private Dictionary<ShipBase,List<ShipAsteroidPoint>> _asteroids = new Dictionary<ShipBase, List<ShipAsteroidPoint>>();
//    private Asteroid[] _cellAsteroids = null;
    private List<AIAsteroidPredata> asteroidsPreData = new List<AIAsteroidPredata>();
    public bool HaveAsteroids { get; private set; }

  //  public Asteroid[] Asteroids => _cellAsteroids;

    public AICell(CellType cell,CellPoint start,CellPoint left,CellPoint right,CellPoint end,float side)
    {
        Side = side;
//        CellPointStart = start;
        Start = start.Position;
        Xindex = start.indexX;
        Zindex = start.indexZ;
        CellType = cell;
        End = new Vector3(Start.x + side, Start.y, Start.z + side);
//        Cross1 = new SegmentPoints(Start, End);
        c1 = start;
        c2 = left;
        c3 = right;
        c4 = end;
//        Cross2 = new SegmentPoints(c4.Position, c2);
        Center = (Start + End) / 2f;
        Border1 = new AICellSegment(start, left);
        Border2 = new AICellSegment(left, end);
        Border3 = new AICellSegment(end, right);
        Border4 = new AICellSegment(right, start);
        Borders[0] = Border1;
        Borders[1] = Border2;
        Borders[2] = Border3;
        Borders[3] = Border4;
        _arrayPoints = new CellPoint[]
        {
            c1,c2,c3,c4
        };
        //        _listPoints.Add(c1);
        //        _listPoints.Add(c2);
        //        _listPoints.Add(c3);
        //        _listPoints.Add(c4);
    }


    public void AddCellOfBorder(AICellSegment cBorder1, AICell getCell)
    {
        CellsOnSides.Add(cBorder1,getCell);
    }

    public Vector3? GetHit(Vector3 from, Vector3 target)
    {
//        var dir = target - from;
        var testedSegment = new SegmentPoints(target, from);
        var c1 = AIUtility.GetCrossPoint(Border1, testedSegment);
        if (c1.HasValue)
        {
            return c1;
        }
        var c2 = AIUtility.GetCrossPoint(Border2, testedSegment);
        if (c2.HasValue)
        {
            return c2;
        }
        var c3 = AIUtility.GetCrossPoint(Border3, testedSegment);
        if (c3.HasValue)
        {
            return c3;
        }
        var c4 = AIUtility.GetCrossPoint(Border4, testedSegment);
        if (c4.HasValue)
        {
            return c4;
        }
        return null;
    }

    public void ClearPosition(Vector3 vector3)
    {
        HaveAsteroids = asteroidsPreData != null && asteroidsPreData.Count > 0;
        if (HaveAsteroids)
        {
            asteroidsPreData.RemoveAll(cellASteroid =>
            {
                var sDist = (cellASteroid.Position - vector3).sqrMagnitude;
                return (sDist <= 9);
            });
        }
    }
    public void AddShip(ShipBase shipBase)
    {
        HaveAsteroids = asteroidsPreData != null && asteroidsPreData.Count > 0;
        if (HaveAsteroids)
        {
            var asteroidsCopy = new List<ShipAsteroidPoint>();
            for (int i = 0; i < asteroidsPreData.Count; i++)
            {
                var cellASteroid = asteroidsPreData[i];
                var asteroidShip = new ShipAsteroidPoint(cellASteroid);
                cellASteroid.OnDeath += () =>
                {
                    asteroidsCopy.Remove(asteroidShip);
                };
//                cellASteroid.AddAsteroidForShip(asteroidShip);
                asteroidsCopy.Add(asteroidShip);
            }
            _asteroids.Add(shipBase, asteroidsCopy);
        }
    }

    [CanBeNull]
    public List<ShipAsteroidPoint> GetAsteroidsForShip(ShipBase owner)
    {
        if (HaveAsteroids)
        {
            return _asteroids[owner];
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

    public bool HavePointsInside(Vector3 start, Vector3 end,out CellPoint point)
    {
        if (_arrayPoints != null)
        {
            for (int i = 0; i < _arrayPoints.Length; i++)
            {
                var p = _arrayPoints[i];
                if (MyExtensions.IsPointInside(p.Position, start, end))
                {
                    point = p;
                    return true;
                }
            }
        }
        point = null;
        return false;

    }
    public bool HavePointsInsideNoY(Vector3 start, Vector3 end,out CellPoint point)
    {
        if (_arrayPoints != null)
        {
            for (int i = 0; i < _arrayPoints.Length; i++)
            {
                var p = _arrayPoints[i];
                if (MyExtensions.IsPointInsideNoY(p.Position, start, end))
                {
                    point = p;
                    return true;
                }
            }
        }
        point = null;
        return false;

    }

    public void ImplementBorderDirection(AICell aICell,int index)
    //0 ↑     //1 ↗    //2 →    //3 ↘
    //4 ↓    //5 ↙    //6 ←    //7 ↖
    {
        var canGo = aICell.IsFree();
        var ang = index * 360 / SIDES_COUNT;
        Vector3 dir;
        switch (index)
        {
            case 0:
                dir = new Vector3(0, 0, 1);
                break;
            case 1:
                dir = new Vector3(ONE_TO_SQRT_TWO, 0, ONE_TO_SQRT_TWO);
                break;
            case 2:
                dir = new Vector3(1, 0, 0);
                break;
            case 3:
                dir = new Vector3(ONE_TO_SQRT_TWO, 0, -ONE_TO_SQRT_TWO);
                break;
            case 4:
                dir = new Vector3(0, 0, -1);
                break;
            case 5:
                dir = new Vector3(-ONE_TO_SQRT_TWO, 0, -ONE_TO_SQRT_TWO);
                break;
            case 6:
                dir = new Vector3(-1, 0, 0);
                break;
            case 7:
                dir = new Vector3(-ONE_TO_SQRT_TWO, 0, ONE_TO_SQRT_TWO);
                break;
            default:
                dir = Vector3.up;
                break;
        }

        var cellDir = new CellDirection(dir, ang, index, canGo);
        PosibleDirections.Add(index, cellDir);
    }

    //public void UpdatesClosestBorders()
    //{
    //    for (int i = 0; i < SIDES_COUNT; i++)
    //    {
    //        if (i == 0)
    //        {
    //            var prev = PosibleDirections[SIDES_COUNT - 1];
    //            var next = PosibleDirections[1];
    //            if (prev && next)
    //            {

    //            }
    //        }
    //        else
    //        {

    //        }
    //    }
    //}

    private CellDirection ClosestPosibleDirection(Vector3 normDir, float eulerAng)
    {
        CellDirection closestDir = PosibleDirections[0];
        if (closestDir.CanMove && eulerAng < ONE_TO_SIDES)
        {
            return closestDir;
        }
        if (closestDir.CanMove && 360 - eulerAng < ONE_TO_SIDES)
        {
            return closestDir;
        }
        float diff = Single.MaxValue;
        for (int i = 1; i < SIDES_COUNT; i++)
        {
            var side = PosibleDirections[i];
            if (side.CanMove)
            {
                var curDiff = Mathf.Abs(side.Angle - eulerAng);
                if (curDiff <= ONE_TO_SIDES)
                {
                    return side;
                }

                if (curDiff < diff)
                {
                    closestDir = side;
                    diff = curDiff;
                }
            }
        }
//                Debug.LogError("Can't find closest posible direction: " + normDir.ToString() + "  eulerAng:" + eulerAng);
        return closestDir;
    }

    private CellDirection ClosestDir(Vector3 normDir,float eulerAng)
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
            Debug.LogError("UpdatePosibleDirection dir not normazzed "  + wantedNormDir.sqrMagnitude);
        }
#endif

//        var closestDir = ClosestDir(wantedNormDir, eulerAng);
//        if (closestDir.CanMove)
//        {
//            return wantedNormDir;
//        }
//        var targetDir = ClosestPosibleDirection(wantedNormDir, eulerAng);

        //Maybe add later

        return wantedNormDir;
    }

    public void AddAsteroid(AIAsteroidPredata asteroid)
    {
        asteroidsPreData.Add(asteroid);
    }
    public void SetStubDirections()
    {
        if (PosibleDirections == null || PosibleDirections.Count == 0)
        {
            PosibleDirections = new Dictionary<int, CellDirection>();
            for (int i = 0; i < SIDES_COUNT; i++)
            {
                var ang = i * 360 / SIDES_COUNT;
                PosibleDirections.Add(i,new CellDirection(Vector3.up, ang, i,false));
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
        Gizmos.DrawWireCube(Center,new Vector3(h,0.1f,h));
//        foreach (var posibleDirection in PosibleDirections)
//        {
//            var d = posibleDirection.Value;
//            DrawUtils.DebugArrow(Center,d.Dir,d.CanMove?Color.green : Color.red);
////            UnityEngine.Handheld.
//        }

    }

}

