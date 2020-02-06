using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



public class ShipPathController
{
    private float Dist = 5f;
    private bool _lastFrameBlocks;
    private int _maskLayer;
    private SideTurn turnSideTurn = SideTurn.left;
    public Vector3 Target { get; private set; }
    private ShipBase _owner;
    private Vector3 _lastTarget;

    private Vector3? blockPosition = null;
    private Vector3? targetCorner = null;
    private PathDebugData _debugData;

#if DEBUG
    private SideTurn _lastSideTurn = SideTurn.left;
    private int _switchTimes = 0;
#endif

    public ShipPathController(ShipBase owner,float dist)
    {
        Dist = dist;
        _maskLayer = 1 << LayerMask.NameToLayer("Border");
        _owner = owner;
        _debugData = new PathDebugData();
    }
    

    public bool Complete(Vector3 target,float checkDist = 0.5f)
    {
        var sDist = (target - _owner.Position).sqrMagnitude;
        if (sDist < checkDist)
        {
            return true;
        }
        return false;
    }

    public Vector3 Turn45()
    {
        var turnRad = _owner.MaxTurnRadius;
        Vector3 side;
        if (MyExtensions.IsTrue01(0.5f))
        {
            side = _owner.LookLeft;
        }
        else
        {
            side = _owner.LookRight;
        }
        var target = _owner.LookDirection * turnRad + side * turnRad + _owner.Position;
        return target;
    }

    public void OnDrawGizmosSelected()
    {
        _debugData.DrawGizmos();
//        Gizmos.color = _lastFrameBlocks ? Color.red : Color.green;
//        Gizmos.DrawRay(_owner.Position, _owner.LookDirection* Dist);
//        Gizmos.color = Color.white;
//        Gizmos.DrawLine(_owner.Position, _lastTarget);
    }
}

