using System;
using UnityEngine;

public class ShipBoostTurn : ShipData
{
    private float _angle;
    private bool _isLeft;
    private Vector3 _lookDirOnStart;
    private float _speedOnStart;
    private Vector3 _targetDir;
    private readonly float _turnSpeed;
    private Action<bool> _activateCallback;
    private Action _endCallback;
    
    private bool _isActive;
    public bool IsActive
    {
        get { return _isActive; }
        private set
        {
            _isActive = value;
            if (_isActive)
            {
                _activateCallback(true);
            }
            else
            {
                _endCallback();
            }
        }
    }
    public ShipBoostTurn(ShipBase owner,Action<bool> activateCallback,Action endCallback) : base(owner)
    {
        _turnSpeed = _owner.ShipParameters.TurnSpeed * 1.5f;
        _activateCallback = activateCallback;
        _endCallback = endCallback;
    }

    public Vector3 LastTurnAddtionalMove { get; private set; }
    

    public float TargetBoosSpeed
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
        _targetDir = dir;
        ActivateTime();
    }

    public float ApplyRotation(Vector3 incomingDir)
    {
//        if (!IsActive)
//        {
//            Deactivate();
//        }
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
            Deactivate();
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

        LastTurnAddtionalMove = dirSum * Time.deltaTime; 
        _owner.BankingData.SetNewData(dirToMove, steps);          
        _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);

        return 1f;
    }

    public void Deactivate()
    {
        IsActive = false;
        _angle = 90f;
        LastTurnAddtionalMove = Vector3.zero;
//        EndTime = 0f;
    }
}