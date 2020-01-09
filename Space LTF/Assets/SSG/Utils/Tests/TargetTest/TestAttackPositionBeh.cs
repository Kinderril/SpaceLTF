using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TestAttackPositionBeh : MonoBehaviour
{
    public Vector3 LookDirection;
    public Vector3 LookRight;
    public Vector3 LookLeft;
    public Quaternion Rotation
    {
        set
        {
            transform.rotation = value;
            var EulerY = value.eulerAngles.y;
            var y = EulerY * Mathf.Deg2Rad;
            var v = new Vector3(Mathf.Sin(y), 0, Mathf.Cos(y));
            LookDirection = v;
            LookLeft = Utils.Rotate90(LookDirection, SideTurn.left);
            LookRight = Utils.Rotate90(LookDirection, SideTurn.right);
        }
    }
    public void ManualUpdate()
    {
        Rotation = transform.rotation;
        // Position = Transform.position;
    }
}

