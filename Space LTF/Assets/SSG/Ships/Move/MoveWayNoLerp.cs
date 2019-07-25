using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;


public class MoveWayNoLerp
{
    public PointWithTurn[] Points;
    public int Index = 0;
//    const float MinDistBetweenPointsSqrt = 3f * 3f;
    const float MinDistBetweenPointsSqrt = 0;
    private bool withTurn = false;
    private float prevDist;
    private Vector3 dir2point;
    private Action _endcallBack;

    public MoveWayNoLerp(List<Vector3> updatedList, bool withTurn, Action endcallBack, Vector3 startDirk)
    {
        
        this.withTurn = withTurn;
        this._endcallBack = endcallBack;
        Points = CheckTurns(updatedList, startDirk);
    }

    private PointWithTurn[] CheckTurns(List<Vector3> updatedList, Vector3 startDir)
    {
        var c = updatedList.Count;
        var list = new PointWithTurn[c];
        SideTurn? lastTurnSide = null;
        if (c > 1)
        {
            for (int i = 0; i < c - 1; i++)
            {
                var p1 = updatedList[i];
                var p2 = updatedList[i + 1];
                var d = p2 - p1;
                bool nextFrameSameSide = false;
                var lookLeft = Utils.Rotate90(startDir, SideTurn.left);
                var nextDot = Vector3.Dot(d, lookLeft) > 0;
                var curTurnSide = nextDot ? SideTurn.left : SideTurn.right;
                if (lastTurnSide.HasValue)
                {
                    nextFrameSameSide = lastTurnSide.Value == curTurnSide;
                }
                lastTurnSide = curTurnSide;
                startDir = d;
                PointWithTurn p = new PointWithTurn(p1.x, p1.z, nextFrameSameSide);
                list[i] = p;
                if (i == c - 2)
                {
                    list[c - 1] = new PointWithTurn(p2.x, p2.z, false);
                }
            }
        }
        else if (c == 1)
        {
            var p1 = updatedList[0];
            list[0] = new PointWithTurn(p1.x, p1.z, false);
        }
        else
        {
            Debug.Log("WTF2!!!");
        }
#if UNITY_EDITOR
        if (list.Length > 0)
        {
            var pp = list[0];
            if (pp == null)
            {
                Debug.Log("WTF1!!!");
            }
        }
#endif
        return list;
    }

    public static MoveWayNoLerp Create(Vector3 to,ShipBase obj,Action endCallback)
    {
        var turnRadMax = obj.MaxTurnRadius;

        MoveWayNoLerp way = null;
        way = CalcAndCheckWay(obj.Position, to, obj.Position, obj.LookDirection, endCallback, true);
        if (way != null)
        {
            return way;
        }
        var firstPos = obj.Position + 2 * obj.LookDirection * turnRadMax;
        way = CalcAndCheckWay(firstPos, to, obj.Position, obj.LookDirection, endCallback,true);
        if (way != null)
        {
            return way;
        }

        var halfPointLeft = obj.LookDirection * turnRadMax + obj.LookLeft * turnRadMax + obj.Position;
        var halfPointRight = obj.LookDirection * turnRadMax + obj.LookRight * turnRadMax + obj.Position;

        var sDistToFinishHalfRight = (halfPointRight - to).sqrMagnitude;
        var sDistToFinishHalfLeft = (halfPointLeft - to).sqrMagnitude;

        if (sDistToFinishHalfLeft < sDistToFinishHalfRight)
        {

            way = CalcAndCheckWay(halfPointLeft, to, obj.Position, obj.LookLeft, endCallback);
            if (way != null)
            {
                return way;
            }
            way = CalcAndCheckWay(halfPointRight, to, obj.Position, obj.LookRight, endCallback);
            if (way != null)
            {
                return way;
            }
        }
        else
        {
            way = CalcAndCheckWay(halfPointRight, to, obj.Position, obj.LookRight, endCallback);
            if (way != null)
            {
                return way;
            }
            way = CalcAndCheckWay(halfPointLeft, to, obj.Position, obj.LookLeft, endCallback);
            if (way != null)
            {
                return way;
            }
        }


        var pointLeft = 2 * obj.LookLeft * turnRadMax + obj.Position;
        var pointRight = 2 * obj.LookRight * turnRadMax + obj.Position;

        var sDistToFinishRight = (pointRight - to).sqrMagnitude;
        var sDistToFinishLeft = (pointLeft - to).sqrMagnitude;

        if (sDistToFinishLeft < sDistToFinishRight)
        {
            way = CalcAndCheckWay(pointLeft, to, obj.Position, -obj.LookDirection, endCallback);
            if (way != null)
            {
                return way;
            }
            way = CalcAndCheckWay(pointRight, to, obj.Position, -obj.LookDirection, endCallback);
            if (way != null)
            {
                return way;
            }
        }
        else
        {
            way = CalcAndCheckWay(pointRight, to, obj.Position, -obj.LookDirection, endCallback);
            if (way != null)
            {
                return way;
            }
            way = CalcAndCheckWay(pointLeft, to, obj.Position, -obj.LookDirection, endCallback);
            if (way != null)
            {
                return way;
            }
        }



        var pointCenter = obj.Position;
        way = CalcAndCheckWay(pointCenter, to, obj.Position, obj.LookDirection, endCallback);
        if (way != null)
        {
            return way;
        }
        var rotate = ManeuverLib.TurnAround(obj, endCallback);
        
//        Debug.LogError("can' calc path. Тут как то ваще все плохо. Хрен пойми что делать. Но такого быть не должно");
        return rotate;
    }

    [CanBeNull]
    private static MoveWayNoLerp CalcAndCheckWay(Vector3 startPathPoint, Vector3 to,Vector3 objectPosition,
        Vector3 look,Action endCallback,bool byObject = false)
    {
        var halfPathRight = new NavMeshPath();
        if (NavMesh.CalculatePath(startPathPoint, to, NavMesh.AllAreas, halfPathRight))
        {
            var firstPoint = halfPathRight.corners[1];
            bool isPosible;
            if (byObject)
            {
                isPosible = IsPosibleToUse(objectPosition, look, firstPoint);
            }
            else
            {
                isPosible = IsPosibleToUse(startPathPoint, look, firstPoint);
            }
                
                
#if UNITY_EDITOR
//            Debug.DrawRay(startPathPoint,Vector3.up*5,isPosible?Color.green :Color.red,2f);
#endif
            if (isPosible)
            {

                List<Vector3> list;
                if (byObject)
                {
                    list = null;
                }
                else
                {
                    list = new List<Vector3>() { startPathPoint };
                }
//                var gg = GetTurnPoints(objectPosition, obj.LookLeft, obj.LookDirection, turnRadMax, pathLeft.corners[0]);
                return Merge(list, UpdatePath(halfPathRight.corners), endCallback, look);
            }
        }
        return null;
    }

    private static bool IsPosibleToUse(Vector3 from,Vector3 startDir,Vector3 to)
    {
        var dir =Utils.NormalizeFastSelf(to - from);
        return Utils.IsAngLessNormazied(dir, startDir, UtilsCos.COS_45_RAD);
    }

    private static MoveWayNoLerp Merge(List<Vector3> turnPoints, List<Vector3> mainPoints,Action endcallBack,Vector3 startDir)
    {
        var withTurn = turnPoints != null && turnPoints.Count > 0;
        if (withTurn)
        {
            if (mainPoints.Count > 0)
            {
                mainPoints.RemoveAt(0);
            }
            turnPoints.AddRange(mainPoints);

            return new MoveWayNoLerp(turnPoints,withTurn, endcallBack, startDir);
        }
        else
        {
            return new MoveWayNoLerp(mainPoints, withTurn, endcallBack, startDir);
        }
    }

    private static List<Vector3> GetTurnPoints(Vector3 pos, Vector3 sideDir,Vector3 lookDir,float turnRad,Vector3 nextPoint)
    {
        var d = lookDir*turnRad;
        var d2 = sideDir * turnRad;
#if UNITY_EDITOR
        if (lookDir.magnitude > 1)
        {
            Debug.Log("lookDir.magnitude " + lookDir.magnitude);
        }
        if (sideDir.magnitude > 1)
        {
            Debug.Log("sideDir.magnitude " + sideDir.magnitude);
        }
#endif

        var p1 = pos + d + d2;
//        var p2 = pos + d2*2;

//        var dist1 = DistFast(p1, nextPoint);
//        var dist2 = DistFast(p2, nextPoint);

        return new List<Vector3>() {p1};
    }

    public bool Complete()
    {
        return  !(Index < Points.Length);
    }

    public Vector3 GetCurentDirection(ShipBase shipBase,out bool nextRotationToSameSide)
    {
        if (Index >= Points.Length)
        {
            nextRotationToSameSide = false;
            return shipBase.LookDirection;
        }

        var p = Points[Index];
        var dx = p.x - shipBase.Position.x;
        var dz = p.z - shipBase.Position.z;
        dir2point = new Vector3(dx, 0, dz);
        var dist = Mathf.Sqrt(dx * dx + dz * dz);
        var dot = Vector3.Dot(shipBase.LookDirection, dir2point);
        prevDist = dist;
        if (dot < 0)
        {
            //Значит мы прошли текущую точку.
            if (dist < shipBase.MaxTurnRadius)
            {
                //Точка была близко значит берем следущую
                Index++;
                if (Index >= Points.Length)
                {
                    _endcallBack();
                    nextRotationToSameSide = true;
                    return shipBase.LookDirection;
                }
                p = Points[Index];
                dx = p.x - shipBase.Position.x;
                dz = p.z - shipBase.Position.z;
                dir2point = new Vector3(dx, p.z, dz);
                dir2point = Utils.NormalizeFastSelf(dir2point);
                nextRotationToSameSide = p.NextTurnSame;
                return dir2point;
            }
            else
            {
                dir2point = Utils.NormalizeFastSelf(dir2point);
                nextRotationToSameSide = false;
                return dir2point;
            }
        }
        else
        {
            dir2point = Utils.NormalizeFastSelf(dir2point);
            nextRotationToSameSide = false;
            return dir2point;
        }

    }

    private static float DistFast(Vector3 v1, Vector3 v2)
    {
        var dx = v1.x - v2.x;
        var dz = v1.z - v2.z;
        return Mathf.Sqrt(dx*dx + dz*dz);
    }

    private static float CalcLenght(Vector3[] corners)
    {
        float c = 0;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            var c1 = corners[i];
            var c2 = corners[i+1];
            var d = c1 - c2;
            var l = Mathf.Sqrt(d.x*d.x + d.z*d.z);
            c += l;
        }
        return c;
    }

    public float GetLenght()
    {
        float dist = 0;
        for (int i = 0; i < Points.Length -1; i++)
        {
            var p1 = Points[i];
            var p2 = Points[i+1];
            var d = PointWithTurn.Dist(p1, p2);
            dist += d;

        }
        return dist;
    }

    private static List<Vector3> UpdatePath(Vector3[] corners)
    {
        List<Vector3> updatedList = new List<Vector3>();
        var c = corners.Length;
        int startIndex = 0;
        if (c > 2)
        {
            startIndex = 1;
        }

        for (int i = startIndex; i < c - 1; i++)
        {
            var c1 = corners[i];
            var c2 = corners[i + 1];
            var d = c1 - c2;
            var sDist = d.x * d.x + d.z * d.z;
//            Debug.Log("Dist:" + Mathf.Sqrt(sDist));
            if (sDist > MinDistBetweenPointsSqrt)
            {
                updatedList.Add(c1);
                if (i == corners.Length - 2)
                {
                    updatedList.Add(c2);
                }
            }
            else
            {
                if (i == corners.Length - 2)
                {
                    updatedList.Add(c2);
                }
                else
                {
                    updatedList.Add((c1 + c2) / 2f);
                    i++;
                }
            }
        }
        return updatedList;
    }

    public void DrawGizmos(Vector3 curPos)
    {
        Vector3 up = Vector3.up * 2.8f;
        if (Points.Length > 2)
        {
            for (int i = 0; i < Points.Length - 1; i++)
            {
                if (i < Index)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    if (withTurn && i <= 1)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }
                }
                var p1 = Points[i].Vector3 + up;
                var p2 = Points[i + 1].Vector3 + up;
                Gizmos.DrawLine(p1, p2);
            }
            var pStart = Points[0];
            var pEnd = Points[Points.Length - 1];
            Gizmos.color = Color.red;
            Gizmos.DrawRay(pStart.Vector3, Vector3.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pEnd.Vector3, Vector3.up);
            DrawUtils.GizmosArrow(Points[Index].Vector3, dir2point, Color.magenta);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(Points[Index].Vector3, 0.3f);
        }
        else if (Points.Length == 2)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Points[1].Vector3 + up, Points[0].Vector3 + up);
            Gizmos.DrawLine(curPos + up, Points[0].Vector3 + up);
        }
        else if (Points.Length == 1)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(curPos + up, Points[0].Vector3 + up);
        }
    }

}

