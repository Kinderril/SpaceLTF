using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipBoostTwist : ShipBoostAbstract
{
    private const float START_PERIOD = 0.3f;
    private const float WORK_PERIOD = 0.67f;
    private const float END_PERIOD = 0.3f;
    private const float MIN_BLOCK_DIST = 3f;
    private const float COEF_SIDE_TURN = 0.1f; //Чем меньше тем меньше в сторону и больше вперед

    private const float POWER_TO_SIDE = 2.2f;

    private const float MOVE_COEF = .5f;
    private const float MOVE_COEF_OTHER = 1f - MOVE_COEF;
    private TimeCoef[] _coefs;
    private Vector3 _dirToMoveToSide;

    private float _endLastPeriod;

    // private float _period;
    private float _endStartPeriod;
    private bool _isLeft;
    private float _lastMoveSpeedCoef;
    private Vector3 _lookDirOnStart;
    private float _maxSpeed;
    private float _middlePeriod;
    private float _minDistToAttack;
    private Quaternion _quaternion;
    private Func<float> _rotationCallback;
    private float _speedOnStart;
    private float _startLastPeriod;
    private IShipData _target;

    public ShipBoostTwist(ShipBase owner, float turnSpeed, Action<bool> activateCallback, Action endCallback,
        Action<Vector3> setAddMoveCallback)
        : base(owner, owner.MoveBoostEffect, activateCallback, endCallback, setAddMoveCallback)
    {
        _minDistToAttack = owner.WeaponsController.MaxAttackRadius * 1.3f;
    }

    public float CurSpeed { get; private set; }

    public void Activate()
    {
        if (!CanUse)
            return;

        _rotationCallback = ApplyRotationSimple;
        SubActivate(MyExtensions.IsTrueEqual());
        Debug.LogError($"Twist activated free {_owner.Id}");
    }

    public void Activate(IShipData target)
    {
        if (!CanUse) return;
        _rotationCallback = ApplyRotationToTarget;
        Debug.LogError($"Twist activated {_owner.Id}");
        _target = target;
        var isEnemyAtLeftSide = Vector3.Dot(_owner.LookLeft, target.ShipLink.LookDirection) > 0;
        SubActivate(isEnemyAtLeftSide);
    }

    private void SubActivate(bool isLeft)
    {
        _maxSpeed = _owner.MaxSpeed();
        _isLeft = MyExtensions.IsTrue01(.5f);
        _speedOnStart = _owner.CurSpeed;
        _lookDirOnStart = _owner.LookDirection;
        IsActive = true;
        ActivateTime();
        if (isLeft)
            _dirToMoveToSide = Utils.NormalizeFastSelf(Vector3.Lerp(_owner.LookRight, _lookDirOnStart, COEF_SIDE_TURN));
        else
            _dirToMoveToSide = Utils.NormalizeFastSelf(Vector3.Lerp(_owner.LookLeft, _lookDirOnStart, COEF_SIDE_TURN));
        Debug.DrawRay(_owner.Position, _dirToMoveToSide * 7, Color.yellow, 5);
        Debug.DrawRay(_owner.Position, _owner.LookDirection * 5, Color.green, 5);
    }

    private void ActivateTime()
    {
        var fullLenght = WORK_PERIOD + START_PERIOD + END_PERIOD;
        // _period = 0f;
        _middlePeriod = Time.time + fullLenght * .5f;
        _endLastPeriod = Time.time + fullLenght;
        _startLastPeriod = Time.time + WORK_PERIOD + START_PERIOD;
        _endStartPeriod = Time.time + START_PERIOD;
        var listCoefs = new List<Tuple<float, float>>();
        listCoefs.Add(new Tuple<float, float>(Time.time, 0f));
        listCoefs.Add(new Tuple<float, float>(_endStartPeriod, 1f));
        listCoefs.Add(new Tuple<float, float>(_startLastPeriod, .7f));
        listCoefs.Add(new Tuple<float, float>(_endLastPeriod, 0f));
        _coefs = Utils.CreateTimeCoef(listCoefs);
    }

    public bool ShallStartUse(IShipData target)
    {
        if (!CanUse) return false;
        if (target == null) return false;

        if (target.IsInBack() && target.Dist < 14) return true;

        return false;

        // var dist = target.Dist;
        // if (dist > _minDistToAttack || dist < MIN_BLOCK_DIST)
        // {
        //     return false;
        // }
        // if (!target.IsInFrontSector())
        //     return false;
        //
        // var targetDir = target.ShipLink.LookDirection;
        // var testedDir = -_owner.LookDirection;
        //
        // var isAngOk = Utils.IsAngLessNormazied(targetDir, testedDir, UtilsCos.COS_90_RAD);
        // return isAngOk;
    }

    public float ApplyRotation(Vector3 incomingDir)
    {
        return _rotationCallback();
    }

    private float ApplyRotationToTarget()
    {
        CurSpeed = 0f;
        if (_target.IsDead || _target == null)
        {
            Stop();
            return 1f;
        }

        if (Utils.GetCoefByTime(_coefs, out var sideMoveSpeedCoef))
        {
            var lerpedMove = SubMove(sideMoveSpeedCoef);
            var dirToLook = _target.DirNorm;
            var debugMovData = new DebugMovingData();
            var qRotation = MovingObject.ApplyRotationXZ(dirToLook,
                _owner.LookDirection, _owner.LookLeft, () => _owner.ShipParameters.TurnSpeed,
                debugMovData, _owner.Position, out var steps);
            _owner.Rotation = qRotation;

            SetAddMoveCallback(lerpedMove);
            var bankSteps = Time.time > _startLastPeriod ? 0f : Time.time < _middlePeriod ? 1f : -1f;
            _owner.BankingData.SetNewData(_dirToMoveToSide, bankSteps, 2f);

            _owner.Rotation = qRotation;
        }
        else
        {
            Stop();
        }

        return 1f;
    }

    private float ApplyRotationSimple()
    {
        if (Utils.GetCoefByTime(_coefs, out var sideMoveSpeedCoef))
        {
            var lerpedMove = SubMove(sideMoveSpeedCoef);
            SetAddMoveCallback(lerpedMove);
            var bankSteps = Time.time > _startLastPeriod ? 0f : Time.time < _middlePeriod ? 1f : -1f;
            _owner.BankingData.SetNewData(_dirToMoveToSide, bankSteps,2f);
        }
        else
        {
            Stop();
        }

        return 1f;
    }

    private Vector3 SubMove(float sideMoveSpeedCoef)
    {
        var simpleMoveCoef = 1f - sideMoveSpeedCoef;
        CurSpeed = 1f;
        _lastMoveSpeedCoef = _lastMoveSpeedCoef * MOVE_COEF + sideMoveSpeedCoef * MOVE_COEF_OTHER;
        var ownerCurSpeed = _lookDirOnStart * _owner.CurSpeed * simpleMoveCoef;
        var moveToSide = _dirToMoveToSide * _maxSpeed * POWER_TO_SIDE * _lastMoveSpeedCoef;
        var lerpedMove = (ownerCurSpeed + moveToSide) * Time.deltaTime;
        return lerpedMove;
    }

    public void ActivateBack(ShipPersonalInfo info)
    {
        Activate(info);
    }

    private void Stop()
    {
        IsActive = false;
        Debug.LogError($"Twist stop {_owner.Id}");
        SetAddMoveCallback(Vector3.zero);
    }
}