using System;
using UnityEngine;

public class ShipBoostTurn : ShipBoostAbstract
{
    private float _angle;
    private bool _isLeft;
    private Vector3 _lookDirOnStart;
    private float _speedOnStart;
    private float _minAttackDistToStart;
    private Vector3 _targetDir;
    private readonly float _turnSpeed;


    public ShipBoostTurn(ShipBase owner, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(owner, owner.MoveBoostEffect, activateCallback, endCallback, setAddMoveCallback)
    {
        _turnSpeed = _owner.ShipParameters.TurnSpeed * 1.5f;
        var minAttackDist = float.MaxValue;
        foreach (var weapon in owner.WeaponsController.GelAllWeapons())
        {
            if (weapon.AimRadius < minAttackDist)
                minAttackDist = weapon.AimRadius;
        }
        _minAttackDistToStart = minAttackDist * 1.3f;
    }
    public float CurSpeed
    {
        get
        {
            if (_angle < 25f)
                return 1f;
            return 0.01f;
        }
    }

    private void ActivateTime()
    {
        _isLeft = Vector3.Dot(_targetDir, _owner.LookLeft) > 0;
        _speedOnStart = _owner.CurSpeed;
        _lookDirOnStart = _owner.LookDirection;
        IsActive = true;
    }

    public void Activate(Vector3 dir)
    {
        if (!CanUse)
        {
            return;
        }
//        Debug.LogError($"Turn activated {_owner.Id}");
        _targetDir = dir;
        ActivateTime();
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

        var dirToMove = _targetDir;
        _angle = Vector3.Angle(dirToMove, _owner.LookDirection);
        var angPerFrameTurn = _turnSpeed * Time.deltaTime;
        var steps = _angle / angPerFrameTurn;
        if (steps <= 1f) // && exactlyPoint)
        {
            Stop();
            _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, dirToMove);
            return 1f;
        }
        Vector3 lerpRes;
        if (_isLeft)
            lerpRes = Utils.RotateOnAngUp(_owner.LookDirection, angPerFrameTurn);
        else
            lerpRes = Utils.RotateOnAngUp(_owner.LookDirection, -angPerFrameTurn);

        float straightSpeed, curvSpeed;

        var curSpeed = _owner.CurSpeed;
        var maxSpeed = _owner.MaxSpeed();

        var sumSpeed = curSpeed + _speedOnStart;
        if (sumSpeed > maxSpeed)
        {
            var coef = maxSpeed / sumSpeed;
            straightSpeed = curSpeed * coef;
            curvSpeed = _speedOnStart * coef;
        }
        else
        {
            straightSpeed = curSpeed;
            curvSpeed = _speedOnStart;
        }

        var dir1 = straightSpeed * _owner.LookDirection;
        var dir2 = curvSpeed * _lookDirOnStart;
        var dirSum = dir1 + dir2;


        var lastTurnAddtionalMove = dirSum * Time.deltaTime;
        SetAddMoveCallback(lastTurnAddtionalMove);
        _owner.BankingData.SetNewData(dirToMove, steps);
        _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);

        return 1f;
    }

    public void Stop()
    {
        if (!IsActive)
        {
            return;
        }

//        Debug.LogError($"Turn stop {_owner.Id}");
        IsActive = false;
        _angle = 90f;
        SetAddMoveCallback(Vector3.zero);
        //        EndTime = 0f;
    }

    public bool ShallStartUse(IShipData target)
    {
        if (!CanUse)
        {
            return false;
        }
        if (target.Dist < _minAttackDistToStart && target.Dist > 2)
        {
            if (!Utils.IsAngLessNormazied(target.DirNorm, _owner.LookDirection, UtilsCos.COS_30_RAD))
            {
                return true;
            }
        }

        return false;
    }
}