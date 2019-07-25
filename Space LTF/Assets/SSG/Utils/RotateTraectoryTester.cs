using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class RotateTraectoryTester : MonoBehaviour
{
    public Transform Target;
    public Transform StartPoint;
    public Transform PointToStartDir;

    void OnDrawGizmos()
    {
        if (Target == null)
        {
            return;
        }
        if (StartPoint == null)
        {
            return;
        }
        if (PointToStartDir == null)
        {
            return;
        }
        var d = PointToStartDir.position - StartPoint.position;
        var maxTurnRadius1 = (StartPoint.position - transform.position).magnitude;
        var resultPoint = AIUtility.RotateByTraectory(StartPoint.position, Target.position, transform.position, d, maxTurnRadius1);

        var radius = (StartPoint.position - transform.position).magnitude;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Target.position,0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position,0.3f);
        DrawUtils.DrawCircle(transform.position,Vector3.up, Color.yellow,radius);
        DrawUtils.GizmosArrow(StartPoint.position,d,Color.red);
        DrawUtils.GizmosArrow(resultPoint.Right, Target.position - resultPoint.Right, Color.green);
        DrawUtils.GizmosArrow(resultPoint.Wrong, Target.position - resultPoint.Wrong, Color.red);

    }
}

