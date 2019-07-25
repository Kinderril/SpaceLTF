using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public  static class EulerLerp
{
    public static Vector3 LerpVectorByY(Vector3 from, Vector3 to, float percent)
    {
        from.y = to.y = 0;
        from.Normalize();
        to.Normalize();
        var fromAng = ConverToEulerAng(from);
        var toAng = ConverToEulerAng(to);

        var angResult = Lerp(fromAng, toAng, percent);
        var angRadians = angResult*Mathf.Deg2Rad;
//        Debug.Log("angRadians:" + angRadians +  "   " + percent);
        var vectorResult = new Vector3(Mathf.Cos(angRadians), 0, Mathf.Sin(angRadians));
//        var vectorResult2 = new Vector3(Mathf.Sin(angRadians), 0, Mathf.Cos(angRadians));
//        Debug.Log("Ang converts.  from:" + VectorWrite(from) + "  {" + fromAng + "}  to:"
//            + to + "  {" + toAng + "} " + "   angresult:" + angResult + "    vectorResult:" + VectorWrite(vectorResult)
//            + "    vectorResult2:" + VectorWrite(vectorResult2));
        return vectorResult;
    }

    private static float ConverToEulerAng(Vector3 dir)
    {
        var d = Mathf.Acos(dir.x) * Mathf.Rad2Deg;
        if (dir.z <= 0)
        {
            d = 360 - d;
        }
        if (d == 360)
        {
            d = 0;
        }
        return d;
    }

    public static float Lerp(float fromDeg, float toDeg, float percent)
    {
        if (fromDeg > 360 || fromDeg < 0)
        {
            Debug.LogError("fromDeg parameter is bad " + fromDeg);
            return 0;
        }

        if (toDeg > 360 || toDeg < 0)
        {
            Debug.LogError("toDeg parameter is bad " + toDeg);
            return 0;
        }
        if (percent > 1f || percent < 0f)
        {

            Debug.LogError("percent parameter is bad " + percent);
            return 0;
        }

        bool crossing0;
        bool isReverted = fromDeg > toDeg;
        float min, max;
        if (isReverted)
        {
            min = toDeg;
            max = fromDeg;
        }
        else
        {
            max = toDeg;
            min = fromDeg;
        }
        bool side;//FALSE == RIGHT, TRUE == LEFT
        var sectorMax = GetSector(max);
        var sectorMin = GetSector(min);

//        Debug.Log("min:" + min + "   max:" + max);
//        Debug.Log("sectorMin:" + sectorMin + "   sectorMax:" + sectorMax);

        float dist;
        if (sectorMax == sectorMin)
        {
            side = true;
        }
        else if (sectorMax - sectorMin == 2)
        {
            float d1 = 0, d2 = 0;
            if (sectorMin == 1)
            {
                d1 = min;
                d2 = max - 180;
            }
            else if (sectorMin == 2)
            {
                d1 = min - 90;
                d2 = max - 270;
            }
            else
            {
                Debug.LogError("cannot be 143543dfgd");
            }
            side = d1 > d2;
        }
        else
        {
            if (sectorMin == 1 && sectorMax == 4)
            {
                side = false;
            }
            else
            {
                side = true;
            }
        }
        crossing0 = !side;
//        Debug.Log("crossing0:" + crossing0 + "    isReverted:" + isReverted);
        var delta = max - min;
        if (delta > 180)
        {
            dist = 360 - delta;
        }
        else
        {
            dist = delta;
        }
        if (isReverted)
        {
            side = !side;
        }
//        Debug.Log("dist:" + dist + "   side:" + side);
        if (crossing0)
        {
            var moveDelta = dist * percent;
            if (side)
            {
                fromDeg += moveDelta;
                if (fromDeg > 360)
                {
                    fromDeg -= 360;
                    return fromDeg;
                }
                return fromDeg;
            }
            else
            {
                fromDeg -= moveDelta;
                if (fromDeg < 0)
                {
                    fromDeg += 360;
                    return fromDeg;
                }
                return fromDeg;
            }

        }
        return Mathf.Lerp(fromDeg, toDeg, percent);
    }


    private static int GetSector(float r)
    {
        if (r >= 0f && r < 90f)
        {
            return 1;
        }
        if (r >= 90 && r < 180f)
        {
            return 2;
        }
        if (r >= 180 && r < 270f)
        {
            return 3;
        }
        return 4;
    }

    private static string VectorWrite(Vector3 p0)
    {
        return "(" + p0.x.ToString("0.000") + "," + p0.z.ToString("0.000") + ")";
    }

}

