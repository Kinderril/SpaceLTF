using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BezieUtils 
{

    public static List<Vector3> GetBeziePoints(Vector3 startDir, List<Vector3> points)
    {
        List<Vector3> pointsBezie = new List<Vector3>();
        var c = 14f;
        var curDir = startDir;
        for (int i = 0; i < points.Count - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i + 1];

            var ctr = GetMidPos2(curDir, p1, p2);
            for (int j = 0; j < c; j++)
            {
                var t = (float)j / c;
                var t2 = (1 - t) * (1 - t);
                var x1 = t2 * p1.x + 2 * (1 - t) * t * ctr.x + t * t * p2.x;
                var z1 = t2 * p1.z + 2 * (1 - t) * t * ctr.z + t * t * p2.z;
                var v = new Vector3(x1, 0, z1);
                pointsBezie.Add(v);
            }
            curDir = p2 - ctr;
        }

        return pointsBezie;
    }



    private static Vector3 GetMidPos(Vector3 startDir, Vector3 p1, Vector3 p2)
    {
        var dist = (p1 - p2).magnitude;
        var r = p1 + Utils.NormalizeFastSelf(startDir) * dist * 0.65f;
        return r;
    }      
    private static Vector3 GetMidPos2(Vector3 startDir, Vector3 p1, Vector3 p2)
    {
        var dist = (p1 - p2).magnitude;
        var r = p1 + Utils.NormalizeFastSelf(startDir) * dist * 0.6f;
        var ctr = (p1 + p2) / 2f;
        var midMid = Vector3.Lerp(ctr, r, 0.6f);
//#if UNITY_EDITOR
//        Debug.Draw
//#endif
        return midMid;
    }
}
