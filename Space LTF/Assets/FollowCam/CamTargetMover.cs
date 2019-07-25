using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CamTargetMover : MonoBehaviour
{
    public float maxDist = 3f;
    public float increaseRate = 0.2f;
    public float decreaseRate = 0.3f;
    public Rigidbody bodyOwner;
    private Vector3 curOffset = Vector3.zero;

    void Update()
    {
        var curVelocity = bodyOwner.velocity;
        var isVelocityZero = curVelocity == Vector3.zero;
        float d = 0f;
        if (!isVelocityZero)
        {
            curOffset += curVelocity * increaseRate * Time.deltaTime;
        }
        DEcreaseVelocity();
        curOffset.x = Mathf.Clamp(curOffset.x, -maxDist, maxDist);
        curOffset.y = 0;
        curOffset.z = Mathf.Clamp(curOffset.z, -maxDist, maxDist);
        transform.localPosition = curOffset;
    }

    private void DEcreaseVelocity()
    {
        var d = decreaseRate * Time.deltaTime;
        var xx = Mathf.Abs(curOffset.x);
        var zz = Mathf.Abs(curOffset.z);
        var max = Mathf.Max(xx,zz);
        if (max > 0)
        {
            curOffset.x = Decrease(curOffset.x, d*xx/max);
            curOffset.z = Decrease(curOffset.z, d*zz/max);
        }
    }

    private float Decrease(float c,float d)
    {
        if (c == 0)
        {
            return 0f;
        }
        var wasPositive = c > 0;
        if (c > 0)
        {
            c -= d;
        }
        else
        {
            c += d;
        }
        var nowPositive = c > 0;
        if (nowPositive != wasPositive)
        {
            c = 0;
        }
        return c;
    }

}

