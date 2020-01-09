using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TestTargetPositionBeh : MonoBehaviour
{
    public TestAttackPositionBeh Attacker;
    public TestTargetPosition TargetPos;
    public float EulerY = 0f;
    public Vector3 Position;
    public Vector3 LookDirection;
    public Vector3 LookRight;
    public Vector3 LookLeft;

    private float TargetSpeed = 0.05f;
    private float DistSpeed = 0.1f;
    private float ShootDist = 9f;
    private float TooCloseDist = 1f;
    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set
        {
            transform.rotation = value;
            EulerY = value.eulerAngles.y;
            var y = EulerY * Mathf.Deg2Rad;
            var v = new Vector3(Mathf.Sin(y), 0, Mathf.Cos(y));
            LookDirection = v;
            LookLeft = Utils.Rotate90(LookDirection, SideTurn.left);
            LookRight = Utils.Rotate90(LookDirection, SideTurn.right);
        }
    }

    void Awake()
    {
        TargetPos = new TestTargetPosition();
    }

    public void Update()
    {
        Rotation = transform.rotation;
        Position = transform.position;
        Attacker.ManualUpdate();
        var dir = transform.position - Attacker.transform.position;
        var dist = dir.magnitude;
        float distCoef = 1f;
        if (dist > TooCloseDist)
        {
            distCoef = dist * DistSpeed;
        }

        var offsetCoef = TargetSpeed * distCoef;
        TargetPos.TestTarget(Position,LookDirection,LookRight,Attacker.transform.position, Attacker.LookDirection, ShootDist, offsetCoef);
    }

    void OnDrawGizmos()
    {
        if (TargetPos != null)
            TargetPos.OnDrawGizmos();
    }
}

