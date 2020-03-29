using System;
using UnityEngine;

public class ShipBoostLoop : ShipBoostAbstract
{
    private float _yValueDir = 0;
    private float _turnSpeed = 0.5f;
    private float _curRotationAng = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;
    private float _wCoefl = 0.5f;
    private Quaternion _quaternion;


    public ShipBoostLoop(ShipBase ship, float turnSpeed, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(ship, ship.MoveBoostEffect, activateCallback, endCallback, setAddMoveCallback)
    {
        _turnSpeed = turnSpeed;
    }

    public void Activate()
    {
        if (!CanUse)
        {
            return;
        }
        Debug.LogError($"Loop activated {_owner.Id}");
        _curRotationAng = 0f;
        IsActive = true;
    }

    public void ManualUpdate()
    {
        if (!IsActive)
        {
            return;
        }

        var speed = _turnSpeed * Time.deltaTime;
        _curRotationAng = _curRotationAng + speed;
        var halfAng = -_curRotationAng / 2f;
        _yValueDir = Mathf.Sin(halfAng);
        _wCoefl = Mathf.Cos(halfAng);
        if (_curRotationAng > STOPANG_)
        {
            Stop();
        }
        else
        {
            _quaternion = new Quaternion(_yValueDir, 0, 0, _wCoefl);
            var moveCoef = 1 + _yValueDir;
            _owner.YMoveRotation.SetYDir(_quaternion, moveCoef);
        }
    }

    private void Stop()
    {
        Debug.LogError($"Loop stop {_owner.Id}");
        _wCoefl = 1f;
        _yValueDir = 0f;
        IsActive = false;
        _owner.YMoveRotation.SetYDir(Quaternion.identity, 1f);

    }
}

