using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct AimStraightData
{
    public AimStraightData(float dist2crossPointTarget, float dist2crossByWeapon, Vector3 crossPoint)
    {
        CrossPoint = crossPoint;
        Dist2crossPointTarget = dist2crossPointTarget;
        Dist2crossByWeapon = dist2crossByWeapon;
    }

    public Vector3 CrossPoint;
    public float Dist2crossPointTarget;
    public float Dist2crossByWeapon;
}

public class SegmentPoints
{
    private Vector3 a;
    private Vector3 b;

    public SegmentPoints(Vector3 a, Vector3 b)
    {
        this.a = a;
        this.b = b;
    }

    public virtual Vector3 A => a;
    public virtual Vector3 B => b;


}

public class Point
{
    public float x;
    public float y;

    public Point(float dx, float dy)
    {
        x = dx;
        y = dy;
    }

    public Point(Vector3 v)
    {
        x = v.x;
        y = v.z;
    }
}

public class PointInt
{
    public int x;
    public int y;

    public PointInt(int dx, int dy)
    {
        x = dx;
        y = dy;
    }
    
}

public class RotateTargetPoints
{
    public RotateTargetPoints(Vector3 Right, Vector3 Wrong)
    {
        this.Right = Right;
        this.Wrong = Wrong;
    }

    public Vector3 Right;
    public Vector3 Wrong;
}

public class AIUtility
{
    public static int GetNearEntity(int count, Func<int, Vector3> getSegment)
    {
        var dist = Single.MaxValue;
        var index = -1;
        for (var i = 0; i < count; i++)
        {
            var segment = getSegment(i);
            var sqrDist = segment.x * segment.x + segment.y * segment.y + segment.z * segment.z;

            if (sqrDist > dist)
                continue;

            dist = sqrDist;
            index = i;
        }

        return index;
    }

    private static bool IsParalel(Point p1, Point p2, Point p3, Point p4)
    {
        var k1 = (p2.y - p1.y)/(p2.x - p1.x);
        var k2 = (p4.y - p3.y)/(p4.x - p3.x);
        var d = k1 - k2;
        return (Math.Abs(d) < 0.06f);
    }
    
    private static Point PointOfIntersection(Point p1, Point p2, Point p3, Point p4)
    {
        var d = (p1.x - p2.x) * (p4.y - p3.y) - (p1.y - p2.y) * (p4.x - p3.x);
        var da = (p1.x - p3.x) * (p4.y - p3.y) - (p1.y - p3.y) * (p4.x - p3.x);
        var db = (p1.x - p2.x) * (p1.y - p3.y) - (p1.y - p2.y) * (p1.x - p3.x);

        if (d == 0)
        {
            return null;
        }

        var ta = da / d;
        var tb = db / d;

        if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
        {
            var dx = p1.x + ta * (p2.x - p1.x);
            var dy = p1.y + ta * (p2.y - p1.y);

            return new Point(dx, dy);
        }
        return null;
    }

    public static bool IsParalel(SegmentPoints p1, SegmentPoints p2)
    {
        return IsParalel(new Point(p1.A), new Point(p1.B), new Point(p2.A),
            new Point(p2.B));
    }

    [CanBeNull]
    public static RotateTargetPoints GetByWayDirection(Vector3 startPos, Vector3 corePos, Vector3 startDir, float dist)
    {
        var a = startPos;
        var b = corePos;
//        var w = startDir.x/startDir.z;
        var w = startDir.x / startDir.z;
        var p = a.x - b.x - w * a.z;
        var bPart = 2* w * p - 2*b.z;
        var cPart = (p*p + b.z*b.z - dist*dist);
        var aPart = (w * w + 1);
        var determinant = bPart*bPart - 4* aPart * cPart;
        if (determinant > 0)
        {
            var sqrtDeterm = Mathf.Sqrt(determinant);
            var z1 = (-bPart + sqrtDeterm)/(2*aPart);
            var z2 = (-bPart - sqrtDeterm)/(2*aPart);


            var x1 = w * (z1 - a.z) + a.x;
            var x2 = w * (z2 - a.z) + a.x;

            var r1 = new Vector3(x1, startPos.y, z1);
            var r2 = new Vector3(x2, startPos.y, z2);
            var dot = Vector3.Dot(r1 - startPos, startDir) > 0;
            RotateTargetPoints rr;
            if (dot)
            {
                rr = new RotateTargetPoints(r1,r2);
            }
            else
            {
                rr = new RotateTargetPoints(r2, r1);
            }


            return rr;
        }
        return null;
    }

    public static RotateTargetPoints RotateByTraectory(Vector3 startPoint,Vector3 target, Vector3 centerPoint, Vector3 startDir,float maxTurnRadius)
    {
#if UNITY_EDITOR
        var maxTurnRadius1 = (startPoint - centerPoint).magnitude;
        if (Mathf.Abs(maxTurnRadius1 - maxTurnRadius) > 0.001f)
        {
            Debug.LogError("RotateByTraectory error");
        }
#endif


        var distFromCentToTarget = Vector3.Magnitude(centerPoint - target);
        var arcCosTo = maxTurnRadius / distFromCentToTarget;
        var radius = Mathf.Acos(arcCosTo);
        var centerDir = target - centerPoint;


        var rotated = Utils.RotateOnAngUp(centerDir, Mathf.Rad2Deg * radius);
        var normalizedDirToEndPoit = Utils.NormalizeFastSelf(rotated);
        var dirTest1 = normalizedDirToEndPoit * maxTurnRadius;
        var test1 = centerPoint + dirTest1;

        var rotatedOther = Utils.RotateOnAngUp(centerDir, -Mathf.Rad2Deg * radius);
        var normalizedDirToEndPoit1 = Utils.NormalizeFastSelf(rotatedOther);
        var dirTest2 = normalizedDirToEndPoit1 * maxTurnRadius;
        var test2 = centerPoint + dirTest2;



        bool positiveTurn;
        if (startPoint.x < centerPoint.x)
        {
            if (startDir.z > 0)
            {
                positiveTurn = false;
            }
            else
            {
                positiveTurn = true;
            }
        }
        else
        {

            if (startDir.z < 0)
            {
                positiveTurn = false;
            }
            else
            {
                positiveTurn = true;
            }
        }
        var dirToMeFromCenter = startPoint - centerPoint;
        var vectorMulY1 = dirTest1.z * dirToMeFromCenter.x - dirTest1.x * dirToMeFromCenter.z > 0;
        var vectorMulY2 = dirTest2.z * dirToMeFromCenter.x - dirTest2.x * dirToMeFromCenter.z > 0;
        var angTest1 = Vector3.Angle(dirTest1, dirToMeFromCenter);
        var angTest2 = Vector3.Angle(dirTest2, dirToMeFromCenter);
        var angTestModif1 = (!positiveTurn && vectorMulY1) || (positiveTurn && !vectorMulY1) ? 360 - angTest1 : angTest1;
        var angTestModif2 = (!positiveTurn && vectorMulY2) || (positiveTurn && !vectorMulY2) ? 360 - angTest2 : angTest2;
//        Debug.Log("angTest1:" + angTest1 + "  angTest2:" + angTest2 + " angTestModif1:" + angTestModif1 
//            + "  angTestModif2:" + angTestModif2 + "   positiveTurn:" + positiveTurn 
//            + "   vectorMulY1:" + vectorMulY1+ "   vectorMulY2:" + vectorMulY2);

        if (angTestModif1 < angTestModif2)
        {
            return new RotateTargetPoints(test1, test2);
        }
        else
        {
            return new RotateTargetPoints(test2, test1);
        }
    }

    public static Vector3? GetCrossPoint(SegmentPoints p1, SegmentPoints p2)
    {
//        Debug.DrawLine(p1.A(),p1.B(),Color.cyan);
//        Debug.DrawLine(p2.A(),p2.B(), Color.blue);
        var result1 = PointOfIntersection(new Point(p1.A), new Point(p1.B), new Point(p2.A),
            new Point(p2.B));
        if (result1 != null)
        {
            var v = new Vector3(result1.x, p1.A.y, result1.y);
            return v;
        }
        return null;
    }

    public static float Vector3Dot(Vector3 lhs, Vector3 rhs)
    {
        return (float)(lhs.x * rhs.x + lhs.z * rhs.z);
    }

    public static Vector3 GetProjectionPoint(Vector3 p, Vector3 p1, Vector3 p2)
    {
        var a = p1.z - p2.z;
        if (a == 0)
        {
            return new Vector3(p.x, p1.y, p1.z);
        }
        var b = p2.x - p1.x;
        if (b == 0)
        {
            return new Vector3(p1.x, p1.y, p.z);
        }
        var c = p1.x * p2.z - p2.x * p1.z;
        var d = b * p.x - a * p.z;
        var yy = -(b * c + a * d) / (b * b + a * a);
        var xx = -(c + b * yy) / a;
        return new Vector3(xx, p1.y, yy);
    }

    public static AimStraightData? IsAimedStraightFindCrossPoint(Vector3 targetPredictionPos, Vector3 targetCurPos,
       Vector3 shooterPos, Vector3 shooterPredictionPos ,bool withDraw = false)
    {
        var s1 = new SegmentPoints(targetPredictionPos, targetCurPos);
        var s2 = new SegmentPoints(shooterPredictionPos, shooterPos);
        var crossPoint = AIUtility.GetCrossPoint(s1, s2);
#if UNITY_EDITOR
        if (withDraw)
        {
            if (crossPoint.HasValue)
                Debug.DrawRay(crossPoint.Value, Vector3.up, Color.red);
            Debug.DrawLine(s1.A,s1.B,Color.green);
            Debug.DrawLine(s2.A,s2.B,Color.yellow);
        }
#endif
        if (crossPoint.HasValue)
        {
            var dist2crossPointTarget = (targetCurPos - crossPoint.Value).magnitude;
            var dist2crossByWeapon = (shooterPos - crossPoint.Value).magnitude;
            AimStraightData data = new AimStraightData(dist2crossPointTarget, dist2crossByWeapon, crossPoint.Value);

            return data;
            
        }
        return null;
    }

    public static bool IsAimedStraightBaseOnCrossPoint(AimStraightData data, float bulletSpeed, float targetSpeed, float posibleDelta)
    {


        if (targetSpeed < 0.001f)
        {
            return true;
        }

        if (data.Dist2crossByWeapon < 1)
        {
            return true;
        }

        if (data.Dist2crossPointTarget < 6 && data.Dist2crossPointTarget > 0.5f && data.Dist2crossByWeapon < 12)
        {
            return true;
        }

        return false;
        var time2crossPointByTarget = data.Dist2crossPointTarget / targetSpeed;
        var time2crossPointByWeapon = data.Dist2crossByWeapon / bulletSpeed;

        if (time2crossPointByWeapon < time2crossPointByTarget)
        {
            return true;
        }

        if (time2crossPointByTarget < 0.3f)
        {
            return true;
        }

        var abs = time2crossPointByWeapon - time2crossPointByTarget;
        //Debug.LogError("abs:" + abs);
        var isOk = abs < posibleDelta;
        return isOk;
    }

    public static bool IsAimedStraightByProjectionPoint(Vector3 targetCurPos,
        Vector3 shooterPos, Vector3 shooterPredictionPos,bool withDraw = false)
    {
        var projection = AIUtility.GetProjectionPoint(targetCurPos, shooterPos, shooterPredictionPos);

        var d = (targetCurPos - projection).magnitude;
        var posibleStraight = 1f;
        var isGood = d < posibleStraight;
#if UNITY_EDITOR
        if (withDraw)
        {
            Debug.DrawLine(targetCurPos, projection, isGood?Color.green : Color.red);
        }
#endif
        return isGood;
    }

    public static bool IsAimedStraight4(Vector3 targetLookDir,Vector3 shooterLookDir)
    {
        var isGood = Utils.IsAngLessNormazied(targetLookDir, shooterLookDir, UtilsCos.COS_30_RAD);
        return isGood;
    }
}

public static class CoverpointIconController
{
#if UNITY_EDITOR
    public enum LabelIcon
    {
        Gray = 0,
        Blue,
        Teal,
        Green,
        Yellow,
        Orange,
        Red,
        Purple
    }

    public enum Icon
    {
        CircleGray = 0,
        CircleBlue,
        CircleTeal,
        CircleGreen,
        CircleYellow,
        CircleOrange,
        CircleRed,
        CirclePurple,
        DiamondGray,
        DiamondBlue,
        DiamondTeal,
        DiamondGreen,
        DiamondYellow,
        DiamondOrange,
        DiamondRed,
        DiamondPurple
    }

    private static GUIContent[] labelIcons;
    private static GUIContent[] largeIcons;

    public static void SetIcon(GameObject gObj, Icon icon)
    {
        //if (largeIcons == null)
        //{
        //    largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
        //}

        //SetIcon(gObj, largeIcons[(int)icon].image as Texture2D);
    }

    private static void SetIcon(GameObject gObj, Texture2D texture)
    {
        var ty = typeof(EditorGUIUtility);
        var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
        mi.Invoke(null, new object[] { gObj, texture });
    }

    private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
    {
        GUIContent[] guiContentArray = new GUIContent[count];
        for (int index = 0; index < count; ++index)
        {
            GUIContent cui = EditorGUIUtility.IconContent(baseName + (object)(startIndex + index) + postFix);
            guiContentArray[index] = cui;
        }

        return guiContentArray;
    }

    public static void EnableLabel(GameObject g, Texture icon)
    {
        return;

        //var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        //var args = new object[] { g, icon };
        ////        EditorGUIUtility.SetIconSize();

        //typeof(EditorGUIUtility).InvokeMember("SetIconForObject", bindingFlags, null, null, args);
    }

    public static void DisableLabel(GameObject g)
    {
        var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        var args = new object[] { g, null };
        typeof(EditorGUIUtility).InvokeMember("SetIconForObject", bindingFlags, null, null, args);
    }
#endif
}


static class NavMeshPathExtension
{
    public static void DrawPathGizmo(this NavMeshPath path, Color color)
    {
        Gizmos.color = color;
        var corners = path.corners;
        for (var i = 0; i < corners.Length - 1; i++)
            Gizmos.DrawLine(corners[i], corners[i + 1]);
    }

#if UNITY_EDITOR
    public static void DrawPathHandleWithSmallUp(this NavMeshPath path, Color color)
    {
        Handles.color = color;
        var corners = path.corners;
        for (var i = 0; i < corners.Length - 1; i++)
            Handles.DrawLine(corners[i] + Vector3.up * 0.1f, corners[i + 1] + Vector3.up * 0.1f);
    }
#endif

    public static float CalculatePathLength(this NavMeshPath path)
    {
        var corners = path.corners;
        var previousCorner = corners[0];
        var pathLength = 0.0f;
        var i = 1;

        while (i < corners.Length)
        {
            var currentCorner = corners[i];
            var vector = previousCorner - currentCorner;
            pathLength += Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
            previousCorner = currentCorner;
            i++;
        }
        return pathLength;
    }
}

public static class VectorExtension
{
    public static Vector3[] CreateVectorPartition(this Vector3 fromPos, Vector3 toPos,
        Vector3 upOffset,
        float step, bool includeOriginAndEnd, bool evenly, bool forceInsert = false)
    {
        toPos += upOffset;
        fromPos += upOffset;
        var vector = toPos - fromPos;
        var currDist = vector.magnitude;

        if (currDist < step || step < 0.0001f)
        {
            if (includeOriginAndEnd)
            {
                if (!forceInsert)
                    return new[] { fromPos, toPos };

                var insertPoint = fromPos + (toPos - fromPos) / 2;
                return new[] { fromPos, insertPoint, toPos };
            }

            return new Vector3[0];
        }

        var stepCount = (int)(currDist / step);

        if (evenly)
            step = currDist / (stepCount + 1);

        if (includeOriginAndEnd)
            stepCount += 2;

        var currPoints = new Vector3[stepCount];
        if (includeOriginAndEnd)
        {
            currPoints[0] = fromPos;
            currPoints[currPoints.Length - 1] = toPos;
        }

        var startIndex = includeOriginAndEnd ? 1 : 0;
        var endIndex = includeOriginAndEnd ? currPoints.Length - 1 : currPoints.Length;

        var currPos = fromPos;
        for (var i = startIndex; i < endIndex; i++)
            currPoints[i] = currPos = currPos + vector.normalized * step;

        return currPoints;
    }

    public static Vector2 ScreenCenter
    {
        get { return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f); }
    }

    public static float Distance(float xdiff, float ydiff, float zdif)
    {
        return (float)Math.Sqrt(xdiff * xdiff + ydiff * ydiff + zdif * zdif);
    }

    public static float Magnitude(Vector3 a)
    {
        return (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
    }
}

