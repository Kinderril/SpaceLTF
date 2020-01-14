using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ShipBoostTwist  : ShipBoostAbstract
{
    private float _yValueDir = 0;
    private float _turnSpeed = 0.5f;
    private float _curRotationAng = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;
    private float _wCoefl = 0.5f;
    private const float POWER_TO_SIDE = 1f;
    private Quaternion _quaternion;
    public ShipBoostTwist(ShipBase owner, float turnSpeed, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMove) 
        : base(owner, activateCallback, endCallback,setAddMove)
    {

        _turnSpeed = turnSpeed;
    }

    public void ManualUpdate()
    {
        if (!IsActive)
        {
            return;
        }

        var speed = _turnSpeed * Time.deltaTime;
        var halfAng = -_curRotationAng / 2f;
        _yValueDir = Mathf.Sin(halfAng);
        _wCoefl = Mathf.Cos(halfAng);

        var lastTurnAddtionalMove = _owner.LookLeft * _yValueDir * POWER_TO_SIDE;
        SetAddMove(lastTurnAddtionalMove);
        if (_curRotationAng > STOPANG_)
        {
            Stop();
        }
        else
        {
            _quaternion = new Quaternion(0, 0, _yValueDir, _wCoefl);
            _owner.YMoveRotation.SetYDir(_quaternion, 1f);
        }
    }

    private void Stop()
    {

        SetAddMove(Vector3.zero);

    }

    public float ApplyRotation(Vector3 incomingDir)
    {
         if (_owner.EngineStop.IsCrash())
            return 0f;
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && _owner is ShipBase)
            return 0f;
#endif
        if (!_owner.EngineWork)
            return 0f;
        if (Time.deltaTime < 0.00001f)
            return 0f;

        float straightSpeed;
        var curSpeed = _owner.CurSpeed;
        straightSpeed = curSpeed;

        var dir1 = straightSpeed * _owner.LookDirection + incomingDir;

        var lastTurnAddtionalMove = dir1 * Time.deltaTime;
        SetAddMove(lastTurnAddtionalMove);
        var lerpRes = _owner.LookDirection;
        // _owner.BankingData.SetNewData(dirToMove, steps);
        _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);

        return 1f;
    }

    private bool IsWayFree()
    {
        return true;
    }
}

