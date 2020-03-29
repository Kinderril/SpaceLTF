
using System;
using UnityEngine;

public class ShipBoostHalfLoop : ShipBoostAbstract
{
    private float _yValueDir = 0;
    private float _halfLoopTimeSec = 0.5f;
    private float _curRotationAng = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;
    private const float HALF_END = 180 * Mathf.PI / 180;
    private float _wCoefl = 0.5f;
    private Quaternion _quaternion;
    private bool _part2;
    private const float HeightCoef = 1.5f;
    private float _curYVal;
    private Func<Vector3?> _targetGetter;

    private float _side = 1;

    public ShipBoostHalfLoop(ShipBase ship, float halfLoopTimeSec, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(ship, ship.MoveBoostEffect, activateCallback, endCallback, setAddMoveCallback)
    {
        _halfLoopTimeSec = halfLoopTimeSec;
    }

    public void Start(Func<Vector3?> targetGetter)
    {
        if (!CanUse)
        {
            return;
        }
        _part2 = false;
        _targetGetter = targetGetter;
        _curRotationAng = 0f;
        IsActive = true;
        _side = MyExtensions.IsTrueEqual() ? 1 : -1;
    }

    public void ManualUpdate()
    {
        if (!IsActive)
        {
            return;
        }

        if (_part2)
        {
            UpdateDoShoot();
            return;
        }

        UpdateHalfLoop();

    }

    private void UpdateHalfLoop()
    {
        var speed = _halfLoopTimeSec * Time.deltaTime;
        _curRotationAng = _curRotationAng + speed;
        var halfAng = -_curRotationAng / 2f;
        _yValueDir = Mathf.Sin(halfAng);
        _wCoefl = Mathf.Cos(halfAng);
        var bankSide = _side * Mathf.Clamp01(_curRotationAng / HALF_END);

        if (_curRotationAng > HALF_END)
        {
            HalfStart();
        }
        else
        {
            _quaternion = new Quaternion(_yValueDir, 0, bankSide, _wCoefl);
            var moveCoef = 1 + _yValueDir;
            _owner.YMoveRotation.SetYDir(_quaternion, moveCoef * HeightCoef);
        }
    }

    private void UpdateDoShoot()
    {
        var speed = _halfLoopTimeSec * Time.deltaTime;
        _curYVal = _curYVal - speed;
        bool shallStop = (_curYVal < 0f);
        if (shallStop)
        {
            _curYVal = 0f;
        }

        var target = _targetGetter();
        if (target.HasValue)
        {
            var targetDir = target.Value - _owner.Position;
            var q = MovingObject.ApplyRotationXZ(targetDir, _owner.LookDirection, _owner.LookLeft, () => 1f, null,
                _owner.Position, out var steps);
            _quaternion = q;
        }
        else
        {
            _quaternion = _owner.Rotation;
        }

        _owner.YMoveRotation.SetYDir(_quaternion, _curYVal);

        if (shallStop)
            Stop();

    }

    private void HalfStart()
    {
        _curYVal = HeightCoef;
    }

    private void Stop()
    {
        _wCoefl = 1f;
        _yValueDir = 0f;
        IsActive = false;
        _owner.YMoveRotation.SetYDir(Quaternion.identity, 1f);

    }
}

