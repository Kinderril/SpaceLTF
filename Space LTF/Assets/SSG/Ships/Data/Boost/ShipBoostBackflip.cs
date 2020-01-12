using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShipBoostBackflip   : ShipData
{
    private float _yValueDir = 0;
    private float _turnSpeed = 0.5f;
    public float CurRotationANg = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;
    private float _wCoefl = 0.5f;
    private Quaternion _quaternion;
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

    public ShipBoostBackflip(ShipBase ship,float turnSpeed, Action<bool> activateCallback, Action endCallback) 
        : base(ship)
    {
        _turnSpeed = turnSpeed;
        _activateCallback = activateCallback;
        _endCallback = endCallback;
    }

    public void Start()
    {
        CurRotationANg = 0f;
        IsActive = true;
    }
    
    public void ManualUpdate()
    {
        if (!IsActive)
        {
            return;
        }

        var speed = _turnSpeed * Time.deltaTime;
        CurRotationANg = CurRotationANg + speed;
        var halfAng = -CurRotationANg / 2f;
        _yValueDir = Mathf.Sin(halfAng);
        _wCoefl = Mathf.Cos(halfAng);
        if (CurRotationANg > STOPANG_)
        {
            Stop();
        }
        else
        {
            _quaternion = new Quaternion(_yValueDir, 0, 0, _wCoefl);
            var moveCoef = 1 + _yValueDir;
            _owner.YMoveRotation.SetYDir(_quaternion, moveCoef);
//            Debug.LogError($"{_owner.Id}: moveCoef:{moveCoef}");
        }
    }

    private void Stop()
    {
        _wCoefl = 1f;
        _yValueDir = 0f;
        IsActive = false;
        _owner.YMoveRotation.SetYDir(Quaternion.identity, 1f);

    }
}

