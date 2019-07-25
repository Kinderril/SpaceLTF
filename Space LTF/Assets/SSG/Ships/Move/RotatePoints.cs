using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RotatePoints
{
    public Vector3 o,c1,c0, c2, c3, c4;


    public RotatePoints(Vector3 Position, float Speed, float AngSpeed, Vector3 lookRight)
    {
        c0 = Position;
        var rad = 180 / Mathf.PI * Speed / AngSpeed;//Radius of turn
        var r1 = Position + lookRight * rad;
        var d = (Position - r1).normalized;
        var d2 = Utils.Rotate45(d, SideTurn.right);
        c1 = r1 + d2 * rad;
        o = r1 + 2 * d2 * rad;
        var d3 = Utils.Rotate45(-d2, SideTurn.left);
        c2 = d3*rad + o;
        c4 = o - d3 * rad;
        var d4 = Utils.Rotate90(d3, SideTurn.left);
        c3 = o + d4 * rad;
    }

    public void DrawGizmos()
    {
        var v = Vector3.up*4f;
//        Gizmos.color = Color.black;
//        Gizmos.DrawSphere(o,0.7f);
//        Gizmos.DrawRay(o,v); 
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(c0,0.7f);
        Gizmos.DrawRay(c0,v); 
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(c1,0.7f);
        Gizmos.DrawRay(c1,v); 
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(c2,0.7f);
        Gizmos.DrawRay(c2, v);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(c3,0.7f);
        Gizmos.DrawRay(c3, v);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(c4,0.7f);
        Gizmos.DrawRay(c4, v);
    }
}

