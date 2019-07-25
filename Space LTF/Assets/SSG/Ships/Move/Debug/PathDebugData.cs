using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public  class PathDebugData
{
    public AICell c0 = null;
    public AICell c1 = null;
    public AICell c2 = null;
    public AICell c3 = null;
    public CellPoint a = null;
    public CellPoint b = null;
    public CellPoint d = null;
    public CellPoint e = null;

    public Vector3? CrossPoint = null;
    public Vector3? PointToGo = null;

    public Vector3 StartPoint;
    public Vector3 TargetPoint;
    public Vector3 ResultDirection;

    public void Reset()
    {
        c0 = c1 = c2 = c3 = null;
        a = b = d = e = null;
        CrossPoint = PointToGo = null;
    }

    public void DrawGizmos()
    {
        if (c0 != null)
        {
            var scale2 = 0.98f * Vector3.one * c0.Side ;
            scale2.y = 0.1f;
            var scale3 = new Vector3(0.1f,4f,0.1f);
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(c0.Center, scale2);

            Gizmos.DrawWireCube(c0.c1.Position, scale3);
            Gizmos.DrawWireCube(c0.c2.Position, scale3);
            Gizmos.DrawWireCube(c0.c3.Position, scale3);
            Gizmos.DrawWireCube(c0.c4.Position, scale3);
        }
        if (c1 != null)
        {
            var scale2 = 0.98f * Vector3.one * c1.Side ;
            scale2.y = 0.1f;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(c1.Center, scale2);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(c2.Center, scale2);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(c3.Center, scale2);
        }
        if (a != null)
        {
            var scale = Vector3.one*0.5f;
            scale.y = 0.1f;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(a.Position, scale);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(b.Position, scale);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(e.Position, scale);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(d.Position, scale);
        }
        Gizmos.color = WithAlpha(Color.blue);
        var pointScale = Vector3.one*0.5f;
        GizmoUtils.DrawCube(StartPoint, Quaternion.identity, pointScale);
        if (CrossPoint.HasValue)
        {
            Gizmos.color = WithAlpha(Color.yellow);
            GizmoUtils.DrawCube(CrossPoint.Value, Quaternion.identity, pointScale);
        }
        Gizmos.color = WithAlpha(Color.green);
        GizmoUtils.DrawCube(TargetPoint, Quaternion.identity, pointScale);
        if (PointToGo.HasValue)
        {
            Gizmos.color = WithAlpha(Color.red);
            GizmoUtils.DrawCube(PointToGo.Value, Quaternion.identity, pointScale);
        }
        GizmoUtils.DrawArrow(StartPoint + Vector3.up * 0.2f, ResultDirection);

    }

    private Color WithAlpha(Color c)
    {
        c.a = 0.5f;
        return c;
    }
}

