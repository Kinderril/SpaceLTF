using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class DebugTurnData
{
    public Vector3 TurnCenter;
    public Vector3 Target;
    public Vector3 PointEndTurn;
    public Vector3 PointEndTurn2;
    public Vector3 CheckForCenter;
    public Vector3 ShipPoint;
    public float TrunRaius;
    public Vector3? NeightCellBlocked;


    public void DrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(TurnCenter,Target);
        Gizmos.DrawLine(PointEndTurn, Target);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(TurnCenter, PointEndTurn);
        Gizmos.DrawLine(TurnCenter, PointEndTurn2);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(PointEndTurn, Vector3.one * 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(PointEndTurn2, Vector3.one * 0.3f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(CheckForCenter, PointEndTurn);
        Gizmos.DrawLine(CheckForCenter, ShipPoint);
        Gizmos.DrawCube(CheckForCenter,Vector3.one*0.6f);
        DrawUtils.DrawCircle(TurnCenter,Vector3.up, Color.green,TrunRaius);
        if (NeightCellBlocked != null)
        {
            var c = Color.red;
            c.a = .5f;
            Gizmos.color = c;
            var v = new Vector3(7,0.3f,7);
            Gizmos.DrawCube(NeightCellBlocked.Value,v);

        }
    }

}

