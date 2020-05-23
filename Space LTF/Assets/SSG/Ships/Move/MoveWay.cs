using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TurnData
{
    public Vector3 Center;
    public float Radius;
    public Quaternion From;
    public Quaternion To;
//    public Vector3 FromVector;
//    public Vector3 ToVector;
    public Quaternion ObjectRotationFrom;
    public Quaternion ObjectRotateTo;
    public bool BigSector;

    public TurnData(Vector3 center,
        float rad, Quaternion fromR, Quaternion to,
        Quaternion objectRotationFrom,
        Quaternion objectRotateTo,bool bigSector)
    {
        BigSector = bigSector;
//        FromVector = fromR;
//        ToVector = to;
        To =   to;
        From = fromR;
        Center = center;
        Radius = rad;
        this.ObjectRotationFrom = objectRotationFrom;
        this.ObjectRotateTo = objectRotateTo;
    }
}

public class MovePointData
{
    public int Index = 0;
    public Vector3 To;
    public Vector3 From;
    public TurnData TurnData = null;
    public float StartTime { get; private set; }
    public float EndTime { get; private set; }
    public float Delta;

    public MovePointData(Vector3 To,
        Vector3 From,
        float spendTime,
        TurnData TurnData)
    {
        this.EndTime = spendTime;
        this.To = To;
        this.From = From;
        this.TurnData = TurnData;
        Delta = spendTime;
//        Debug.Log("time:" + endTime + "  TurnData:" + TurnData);
    }

    public float UpdateTime(float timeNow)
    {
        StartTime = timeNow;
        EndTime = StartTime + Delta;
//        Debug.Log("EndTime:" + EndTime);
        return EndTime;
    }
    
}


public class MoveWay
{
    private MovePointData[] _points;
    private int _curIndex = 0;
    private int _lastIndex  = 0;
    private ShipBase owner;
    private Action _endCallback;

    public MoveWay(List<MovePointData> data, ShipBase owner, Action endCallback)
    {
        this.owner = owner;
        this._endCallback = endCallback;
        int index = 0;
        foreach (var movePointData in data)
        {
            movePointData.Index = index;
            index++;
        }
        _points = data.ToArray();


        float timeNow = Time.time;
        for (int i = 0; i < _points.Length; i++)
        {
            var movePointData = _points[i];
            timeNow = movePointData.UpdateTime(timeNow);
//            Debug.Log("next start time:" + timeNow);
            Debug.Log("1Index:" + movePointData.Index + "  StartTime:" + movePointData.StartTime + "   EndTime:" + movePointData.EndTime + "    delta:" + movePointData.Delta);
//            if (i > 0)
//            {
//                var p = _points[i];
//                Debug.Log("prev====Index:" + (i-1) + "  StartTime:" + p.StartTime + "   EndTime:" + p.EndTime + "    delta:" + p.Delta);
//            }

        }
//        var endTime = 0;
        for (int i = 0; i < _points.Length; i++)
        {
            var movePointData = _points[i];
//            va

            Debug.Log("2Index:"+ movePointData.Index + "  StartTime:" + movePointData.StartTime + "   EndTime:" + movePointData.EndTime + "    delta:" + movePointData.Delta);

        }

//        for (int i = 0; i < _points.Length; i++)
//        {
//            var movePointData = _points[i];
//            Debug.Log("2Index:"+ i+ "  StartTime:" + movePointData.StartTime + "   EndTime:" + movePointData.EndTime + "    delta:" + movePointData.Delta);
//
//        }
    }
    /*
    public void MoveTransform()
    {
        var c = _points[_curIndex];

        var delta =  (Time.time - c.StartTime)/ c.Delta;
        var withTurn = c.TurnData != null;
        //TURNING
        if (withTurn)
        {
            owner.Rotation = Quaternion.Lerp(c.TurnData.ObjectRotationFrom, c.TurnData.ObjectRotateTo, delta);

        }
        //MOVING
        if (withTurn)
        {
            TurnLerp(delta, c.TurnData);
        }
        else
        {
            owner.transform.position = Vector3.Lerp(c.From, c.To, delta);
        }
        if (delta > 1)
        {
            _curIndex++;
            if (c.TurnData != null)
                owner.Rotation = c.TurnData.ObjectRotateTo;
            if (_points.Length <= _curIndex)
            {
                _endCallback();
            }
        }
    }
    */

    public static  MoveWay CalcWayByPoints(Vector3[] points,ShipBase shipBase)
    {
        var turnRad = shipBase.MaxTurnRadius; //Radius of turn
        var data2move = new List<MovePointData>();
        Vector3 lastDir = shipBase.LookDirection;
        for (int i = 0; i < points.Length - 1; i++)
        {
            var p01 = points[i];
            var p02 = points[i + 1];
            lastDir = CalcPoints(data2move, p01, p02, turnRad, shipBase.MaxSpeed(), lastDir);
#if UNITY_EDITOR
            Debug.DrawLine(p01 + Vector3.up, p02 + Vector3.up, Color.yellow, 10f);
#endif
        }
        return new MoveWay(data2move, shipBase, shipBase.WayEnds);
    }

    private static Vector3 CalcPoints(List<MovePointData> data2move, Vector3 p1, Vector3 p2, float turnRad,float MaxSpeed, Vector3 startDir)
    {
        var speed = MaxSpeed;
        CalcSegment(startDir, p1, p2, turnRad, data2move, speed);
        var last = data2move[data2move.Count - 1];
        Vector3 lastDir;
        if (last.TurnData != null)
        {
            lastDir = last.TurnData.ObjectRotateTo * Vector3.forward;
        }
        else
        {
            lastDir = last.To - last.From;
        }
        return lastDir;
    }

    private static void CalcSegment(Vector3 startDir, Vector3 startPoint, Vector3 endPoint, float turnRad, List<MovePointData> list, float speed)
    {
        startDir.Normalize();
        var dir = endPoint - startPoint;

        DrawUtils.DebugArrow(startPoint, startDir, Color.blue, 7);
        //        var dist = dir.magnitude;
        //        var ang = Vector3.Angle(startDir, dir);
        var leftDir = Utils.Rotate90(startDir, SideTurn.left);
        var rightDir = Utils.Rotate90(startDir, SideTurn.right);
        var leftCenter = leftDir * turnRad + startPoint;
        var rightCenter = rightDir * turnRad + startPoint;

        //        DrawUtils.DebugPoint(startPoint,Color.green,2f,5f);
        //        DrawUtils.DebugCircle(leftCenter, Vector3.up, Color.red, turnRad, 5f);
        //        DrawUtils.DebugCircle(rightCenter, Vector3.up, Color.blue, turnRad, 5f);

        var distToLeftCenter = (leftCenter - endPoint).magnitude;
        if (distToLeftCenter <= turnRad)
        {
            var ednPoint = CalcSubInner(startDir, startPoint, endPoint, turnRad, list, false, leftCenter, speed);
            CalcSegment(startDir, ednPoint, endPoint, turnRad, list, speed);
            return;
        }

        var distToRigthCenter = (rightCenter - endPoint).magnitude;
        if (distToRigthCenter <= turnRad)
        {
            var ednPoint = CalcSubInner(startDir, startPoint, endPoint, turnRad, list, true, rightCenter, speed);
            CalcSegment(startDir, ednPoint, endPoint, turnRad, list, speed);
            return;
        }

        var isInLeft = Vector3.Dot(leftDir, dir) > 0;
        var center = isInLeft ? leftCenter : rightCenter;
        var pointData = СalcOutherTurn(startPoint, startDir, endPoint, center, turnRad, isInLeft, speed, list);

        //        var pointData = СalcOutherTurn(startPoint, startDir, endPoint, center, turnRad, isInLeft, speed);

        var spendTime = (endPoint - pointData.To).magnitude / speed;
        //        Debug.Log("spendTime:" + spendTime);
        MovePointData nextData = new MovePointData(endPoint, pointData.To, spendTime, null);

        //        list.Add(pointData);
        list.Add(nextData);


        //        DrawUtils.DebugPoint(nextData.From, Color.yellow, 2f, 5f);
        //        DrawUtils.DebugPoint(nextData.To, Color.magenta, 3f, 5f);
    }

    private static Vector3 CalcSubInner(Vector3 startDir, Vector3 startPoint, Vector3 endPoint, float turnRad, List<MovePointData> list
        , bool isRight, Vector3 circleCenter, float speed)
    {

        //        var moveData = CalcInside(startPoint, startDir, endPoint, circleCenter, turnRad, isRight, sideDir,speed);
        var from = InnerOffset(startPoint, startDir, endPoint, circleCenter, turnRad, isRight);
        var spendTime = (startPoint - from).magnitude / speed;
        MovePointData nextData2 = new MovePointData(from, startPoint, spendTime, null);
        list.Add(nextData2);
        return from;
        //        list.Add(moveData);
    }

    private static Vector3 InnerOffset(Vector3 startPos, Vector3 startDir, Vector3 endPoint,
        Vector3 center, float turnRad, bool isRight)
    {
        var ang2zero = Vector3.Angle(Vector3.forward, startDir);
        var dir2N = endPoint - startPos;
        Vector3 turnedN;

        var modif = Utils.RotateOnAngUp(dir2N, isRight ? -ang2zero : ang2zero);
        turnedN = startPos + modif;
        var pointB = new Vector3(turnedN.x, startPos.y, startPos.z);
        DrawUtils.DebugPoint(pointB, Color.red, 4f, 5f);
        var distBC = (pointB - center).magnitude;
        var delta2 = Mathf.Sqrt(turnRad * turnRad - distBC * distBC);
        var delta1 = Mathf.Abs(turnedN.z - pointB.z);
        var delta = delta1 + delta2;
        var startTurnPoint = startPos + startDir.normalized * delta;
        return startTurnPoint;
    }

    private static MovePointData СalcOutherTurn(Vector3 startPos, Vector3 startDir, Vector3 endPoint, Vector3 center,
        float turnRad, bool isRight, float speed, List<MovePointData> list)
    {
        //        DrawUtils.DebugPoint(center + Vector3.up*2, Color.red, 2f, 5f);

        var cDir = endPoint - center;
        var dMag = cDir.magnitude;
        //        var fSide = Mathf.Sqrt(dMag * dMag + turnRad * turnRad);
        var alpha = Mathf.Asin(turnRad / dMag) * Mathf.Rad2Deg;
        var beta = 90 - alpha;
        var xc = Utils.RotateOnAngUp(cDir, isRight ? -beta : beta);
        var midleXPoint = center + xc.normalized * turnRad;
        var endDirObject = endPoint - midleXPoint;

        var startQuaternion = Quaternion.FromToRotation(Vector3.forward, startDir);
        var endQuaternion = Quaternion.FromToRotation(Vector3.forward, endDirObject);

        var stDir = startPos - center;
        var endDir = midleXPoint - center;

        var dot = Vector3.Dot(startDir, midleXPoint - startPos);
        var bigSector = dot < 0;

        var To = Quaternion.FromToRotation(Vector3.forward, endDir);
        var From = Quaternion.FromToRotation(Vector3.forward, stDir);

        if (bigSector)
        {
            var middle = Quaternion.Lerp(From, To, .5f);


            //            Vector3 targetForward = inversed * Vector3.forward;
            Vector3 targetForward2 = middle * Vector3.forward;
            //            var midPos = center + targetForward.normalized * turnRad;
            var norm = targetForward2.normalized;
            var d = norm * turnRad;
            var midPos = center - d;

            var inversed = Quaternion.FromToRotation(Vector3.forward, -norm);

            var inversedObjectRotation = Quaternion.Inverse(Quaternion.Lerp(startQuaternion, endQuaternion, .5f));
            TurnData turnData1 = new TurnData(center, turnRad, From, inversed, startQuaternion, inversedObjectRotation, true);

            var sectorAng = Quaternion.Angle(From, inversed);
            var spendTime = TurnTime(sectorAng, turnRad, speed);
            MovePointData movePointData1 = new MovePointData(startPos, midPos, spendTime, turnData1);


            TurnData turnData2 = new TurnData(center, turnRad, inversed, To, inversedObjectRotation, endQuaternion, true);
            //            sectorAng = Quaternion.Angle(inversed, To);
            spendTime = TurnTime(sectorAng, turnRad, speed);
            MovePointData movePointData2 = new MovePointData(midleXPoint, midPos, spendTime, turnData2);


            //            DrawUtils.DebugPoint(midleXPoint, Color.green, 5, 6);
            //            DrawUtils.DebugPoint(midPos,Color.magenta,5,6);
            //            DrawUtils.DebugPoint(midPos2, Color.cyan,5,6);
            //            DrawUtils.DebugPoint(startPos, Color.white,5,6);
            list.Add(movePointData1);
            list.Add(movePointData2);
            return movePointData2;
        }
        else
        {
            var sectorAng = Vector3.Angle(stDir, endDir);
            var spendTime = TurnTime(sectorAng, turnRad, speed);
            TurnData turnData = new TurnData(center, turnRad, From, To, startQuaternion, endQuaternion, false);
            MovePointData movePointData = new MovePointData(midleXPoint, startPos, spendTime, turnData);
            list.Add(movePointData);
            return movePointData;
        }
    }


    private static float TurnTime(float sectorAng, float turnRad, float speed)
    {
        var dist = Mathf.PI * turnRad * sectorAng / 180f;
        return dist / speed;
    }

    private void TurnLerp(float delta, TurnData turnData)
    {
        var r = Quaternion.Lerp(turnData.From, turnData.To, delta);
        Vector3 targetForward2 = r * Vector3.forward;
        var p1 = turnData.Center + targetForward2.normalized * turnData.Radius;
        owner.transform.position = p1;
        //        turnData.Center + r.
    }

    public void ComeToIndex()
    {
        _curIndex++;
        if (_curIndex >= _lastIndex)
        {
            Stop();
        }

    }

    private void Stop()
    {
        
    }

    public void DrawGizmos()
    {
        foreach (var movePointData in _points)
        {
            if (movePointData.TurnData != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(movePointData.TurnData.Center, 2f);
                var q1 = movePointData.TurnData.To;
                var q2 = movePointData.TurnData.From;
                var rad = movePointData.TurnData.Radius;
                var center = movePointData.TurnData.Center;
                var stepCount = 20;
                for (int i = 0; i < stepCount - 1; i++)
                {
                    var qNow = Quaternion.Lerp(q1, q2, (float)i / (float)stepCount);
                    var qTo = Quaternion.Lerp(q1, q2, (float)(i + 1f) / (float)stepCount);
                    Vector3 targetForward = qNow * Vector3.forward;
                    Vector3 targetForward2 = qTo * Vector3.forward;
                    var p1 = center + targetForward.normalized * rad;
                    var p2 = center + targetForward2.normalized * rad;
                    if (movePointData.TurnData.BigSector)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }
                    Gizmos.DrawLine(p1, p2);
                }
                Gizmos.color = Color.green;
//                Gizmos.DrawLine(center, center + movePointData.TurnData.FromVector.normalized * rad);
//                Gizmos.DrawLine(center, center + movePointData.TurnData.ToVector.normalized * rad);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(movePointData.From, movePointData.To);
            }
        }
    }
}

