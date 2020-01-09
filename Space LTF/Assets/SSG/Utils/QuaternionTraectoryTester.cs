using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTraectoryTester : MonoBehaviour
{

    public float SpeedY;
    public float SpeedXZ;

    public bool IsActive;
    private float _yValueDir = 0;
    private float _period;
    // private float _endTime;
    // private float _halfTime;
    public float _turnSpeed = 0.5f;
    private float _wCoefl = 0.5f;
    // private float _turnSpeed = 0.5f;
    private bool _up;
    public float CurRotationANg = 0;
    private const float STOPANG_ = 360 * Mathf.PI / 180;

    void Update()
    {
        RotateNow();
        Debug.Log(transform.rotation);
    }

    private void RotateNow()
    {
        ManualUpdate();
        UpdateXZ();
        transform.rotation = new Quaternion(_yValueDir, 0, 0, _wCoefl);
        transform.position = transform.position + transform.forward * SpeedXZ * Time.deltaTime;
    }

    private void UpdateXZ()
    {
        

    }


    void Start()
    {
        _up = true;
        IsActive = true;
    }

    public void Implement()
    {
        IsActive = true;
        _wCoefl = 1f;
    }

    public Quaternion ManualUpdate()
    {
        if (!IsActive)
        {
            _yValueDir = (0f);
            return Quaternion.identity;
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
        return new Quaternion(_yValueDir, 0, 0, _wCoefl);
    }

    private void Stop()
    {
        _wCoefl = 1f;
        _yValueDir = 0f;
        IsActive = false;

    }
}
