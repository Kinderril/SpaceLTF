using UnityEngine;

public class ShipBoostTurn : ShipData
{
//    private float EndTime;
//    public bool IsActive => EndTime > Time.time;
    public Vector3 LastTurnAddtionalMove { get; private set; }
    public bool IsActive { get; private set; }

    public float TargetBoosSpeed
    {
        get
        {
            if (_angle < 25f)
            {
                return 1f;
            }
            return 0.1f;
        }
    }

    private Vector3 _lookDirOnStart;

    private float Period;
    private Vector3 _targetDir;
//    private bool _haveTargetDir;
    private bool _isLeft;
    private float _turnSpeed;
    private float _speedOnStart;
    private float _angle;

    public ShipBoostTurn(ShipBase owner,float period) : base(owner)
    {
        Period = period;
        _turnSpeed = _owner.ShipParameters.TurnSpeed * 1.5f;
    }

    private void ActivateTime()
    {
//        EndTime = Time.time + Period;
        _isLeft = Vector3.Dot(_targetDir, _owner.LookLeft) > 0;
        _speedOnStart = _owner.CurSpeed;
        _lookDirOnStart = _owner.LookDirection;
        IsActive = true;
//       LookDirOnStart =(_isLeft)? _owner.LookLeft: _owner.LookRight;
    }

//    public void Activate()
//    {
//        Debug.LogError($"TURN ACTIVATED ");
//        ActivateTime();
//        _haveTargetDir = false;
//    }

    public void Activate(Vector3 dir)
    {
        Debug.LogError($"TURN ACTIVATED {dir}");
//        _haveTargetDir = true;
        _targetDir = dir;
        ActivateTime();
    }
       
    public bool ApplyRotation(Vector3 incomingDir)
    {
//        if (!IsActive)
//        {
//            Deactivate();
//        }
        if (_owner.EngineStop.IsCrash())
        {
            return false;
        }
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && _owner is ShipBase)
        {
            return false;
        }
#endif
        if (!_owner.EngineWork)
        {
            return false;
        }
        if (Time.deltaTime < 0.00001f)
        {
            return false;
        }
        Vector3 dirToMove;

        dirToMove = _targetDir;
        _angle = Vector3.Angle(dirToMove, _owner.LookDirection);
        var angPerFrameTurn = _turnSpeed * Time.deltaTime;
        var steps = _angle / angPerFrameTurn;
        if (steps <= 1f) // && exactlyPoint)
        {
            Deactivate();
            _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, dirToMove);
            return true;
        }
        //        var percentOfRotate = Mathf.Clamp01(1f / steps);
        //        var lerpRes = EulerLerp.LerpVectorByY(LookDirection, dir, percentOfRotate);
        Vector3 lerpRes;
//        var isLeft = Vector3.Dot(dirToMove, _owner.LookLeft) > 0;
        if (_isLeft)
        {
            lerpRes = Utils.RotateOnAngUp(_owner.LookDirection, angPerFrameTurn);
        }
        else
        {
            lerpRes = Utils.RotateOnAngUp(_owner.LookDirection, -angPerFrameTurn);
        }

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
        Vector3 dir2 = curvSpeed * _lookDirOnStart;// * Time.deltaTime;
        var dirSum = (dir1 + dir2) * Time.deltaTime;

        LastTurnAddtionalMove = dirSum;
        _owner.SetBankData(new BankingData(dirToMove, steps));
        _owner.Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);

        return false;

    }

    private void Deactivate()
    {
        IsActive = false;
        _angle = 90f;
        LastTurnAddtionalMove = Vector3.zero;
//        EndTime = 0f;
    }
}