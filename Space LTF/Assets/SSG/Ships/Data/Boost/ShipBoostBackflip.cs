using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShipBoostBackflip   : ShipData
{
    public bool IsActive;
    private float _yValueDir = 0;
    private float _turnSpeed = 0.5f;
    public float CurRotationANg = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;
    private float _wCoefl = 0.5f;
    private Quaternion _quaternion;


    public ShipBoostBackflip(ShipBase ship,float turnSpeed) 
        : base(ship)
    {
        _turnSpeed = turnSpeed;
    }

    public void Start()
    {
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
            _owner.YMoveRotation.SetYDir(_quaternion, _yValueDir);
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

