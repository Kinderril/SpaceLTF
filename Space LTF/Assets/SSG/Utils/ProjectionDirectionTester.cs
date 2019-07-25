using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ProjectionDirectionTester : MonoBehaviour
    {
        public Transform Target;
        public Transform StartPoint;
        public Transform PointToStartDir;
    public float dist;

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
            var resultPoint = AIUtility.GetByWayDirection(StartPoint.position, Target.position, d, dist);
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(Target.position, 0.5f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(StartPoint.position, 0.3f);
//            DrawUtils.DrawCircle(transform.position, Vector3.up, Color.yellow, radius);
            DrawUtils.GizmosArrow(StartPoint.position, d, Color.yellow);
            if (resultPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(resultPoint.Right, StartPoint.position);
                Gizmos.DrawCube(resultPoint.Right,Vector3.one);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(resultPoint.Wrong, StartPoint.position);
                Gizmos.DrawCube(resultPoint.Wrong,Vector3.one);
            }
            
//            DrawUtils.GizmosArrow(resultPoint.Right, Target.position - resultPoint.Right, Color.green);
//            DrawUtils.GizmosArrow(resultPoint.Wrong, Target.position - resultPoint.Wrong, Color.red);

        }
    }