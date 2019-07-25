using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManeuverLib
{
    public static MoveWayNoLerp Turn45(ShipBase ship, Action endCallback)
    {

        var turnRad = ship.MaxTurnRadius;
        Vector3 side;
        if (MyExtensions.IsTrue01(0.5f))
        {
            side = ship.LookLeft;
        }
        else
        {
            side = ship.LookRight;
        }
        var target = ship.LookDirection*turnRad + side * turnRad + ship.Position;
        List<Vector3> way = new List<Vector3>()
        {
            target
        };
        var way2 = new MoveWayNoLerp(way, true, endCallback, ship.LookDirection);

        return way2;

    }

    public static MoveWayNoLerp TurnAround(ShipBase obj,Action endCallback)
    {
        var turnRad = obj.MaxTurnRadius;
        var sideRightDir = Utils.NormalizeFastSelf((obj.LookDirection + obj.LookRight)/2f);
        var leftTp = obj.Position + obj.LookLeft*turnRad;
        var p1 = leftTp + sideRightDir*turnRad;
        var p2 = 2*p1 - obj.Position;
        var centerTurnPoint = p2 + obj.LookRight*turnRad;
        var p3 = centerTurnPoint + obj.LookDirection*turnRad;
        var p4 = centerTurnPoint + obj.LookRight * turnRad;
        List<Vector3> way = new List<Vector3>()
        {
            p1,p2,p3,p4
        };
#if UNITY_EDITOR
        foreach (var vector3 in way)
        {
            Debug.DrawRay(vector3,Vector3.up*4,Color.black,3f);
        }
#endif

        var way2 = new MoveWayNoLerp(way,true, endCallback,obj.LookDirection);

        return way2;
    }

    public static Vector3 ChooseControlPoint(List<Vector3> field, Vector3 hitPoint, Vector3 target)
    {
        //GEt 2 closest points;
        field.Sort((v1, v2) =>
        {
            var s1 = (v1 - hitPoint).sqrMagnitude;
            var s2 = (v2 - hitPoint).sqrMagnitude;
            if (s1 > s2)
            {
                return -1;
            }
            return 1;
        }
        );
        return GetClosestPointToTurn(field, target);
    }

    private static Vector3 GetClosestPointToTurn(List<Vector3> sortedPoints, Vector3 target)
    {
        var p1 = sortedPoints[2];
        var p2 = sortedPoints[3];
        var d1 = (p1 - target).sqrMagnitude;
        var d2 = (p2 - target).sqrMagnitude;
        Vector3 keyPoint;
        if (d1 < d2)
        {
            keyPoint = p1;
        }
        else
        {
            keyPoint = p2;
        }
        return keyPoint;
    }

    public static SideTurn GetTurnByDir(Vector3 keyPoint, Vector3 startPos,Vector3 dir)
    {
        var dirToKey = keyPoint - startPos;
        if (Utils.VectorMultY(dirToKey, dir) > 0f)
        {
            return SideTurn.left;
        }
        else
        {
            return SideTurn.right;
        }
    }
}

