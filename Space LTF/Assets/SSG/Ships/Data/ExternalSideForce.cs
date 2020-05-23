using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExternalSideForce
{
    public bool IsActive;
    private float EndTime;
    private float Period;

    private SideTurn _side;
    private float Power;

    public ExternalSideForce()
    {
        IsActive = false;
    }

    public void Init(float power,float delay, SideTurn side)
    {
        IsActive = true;
        this._side = side;
        Power = power;
        EndTime = Time.time + delay;
        Period = delay;
    }
    
    public Vector3 GetLerpPercent(MovingObject ship)
    {
        var delta = EndTime - Time.time;
        if (delta <= 0)
        {
            IsActive = false;
            return ship.LookDirection;
        }
        var d = delta / Period;
        var p = Power * d;
        Vector3 dir;
        switch (_side)
        {
            case SideTurn.left:
                dir = ship.LookLeft;
                break;
            case SideTurn.right:
            default:
                dir = ship.LookRight;
                break;
        }


        return EulerLerp.LerpVectorByY(ship.LookDirection, dir, p);
    }

    public Vector3 GetLerpPercent(Vector3 dirOld)
    {
        var delta = EndTime - Time.time;
        if (delta <= 0)
        {
            IsActive = false;
            return dirOld;
        }
        var d = delta / Period;
        var p = Power * d;
        var dir = Utils.Rotate90(dirOld, _side);
        return EulerLerp.LerpVectorByY(dirOld, dir, p);
    }
}