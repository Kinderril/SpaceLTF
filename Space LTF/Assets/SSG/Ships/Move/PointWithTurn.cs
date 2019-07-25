using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PointWithTurn
{
    public float x { get; private set; }
    public float z { get; private set; }
    public bool NextTurnSame { get; private set; }

    public PointWithTurn(float x , float z, bool nextTurnSame)
    {
        this.x = x;
        this.z = z;
        NextTurnSame = nextTurnSame;
    }

    public static float Dist(PointWithTurn p1, PointWithTurn p2)
    {
        var xx = p1.x - p2.x;
        var zz = p1.z - p2.z;
        var dd = xx*xx + zz*zz;
        return (float)Math.Sqrt(dd);
    }

    public Vector3 Vector3
    {
        get { return new Vector3(x,0,z);}
    }
}

