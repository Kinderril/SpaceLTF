﻿using System;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform GameCameraHolder;
    public Transform GameCameraHolderY;
    public Transform RotateHolder;
    private float SpeedXZ = 0.5f;
    private float SpeedY = 1.5f;
    private float SpeedYByKey = 0.3f;
    public float MIN_Y = 10f;
    public float MAX_Y = 19f;
    private Vector3 _min;
    private Vector3 _max;
    private Vector3? _targetPos;
    private float startMoveTime;
    private float _period;
    public Camera Camera;
    public CameraShake MainCameraShake;
    private bool _isEnable = true;
    public AudioSource SourceAmbient;
    public CameraEffects Effects;
    public FXAA FxaaEffect;

    public void InitBorders(Vector3 min, Vector3 max)
    {
        _min = min;
        _max = max;
    }


    public void ReturnCamera()
    {

        Camera.transform.SetParent(RotateHolder, false);
        Camera.transform.localRotation = Quaternion.identity;
        Camera.transform.localPosition = Vector3.zero;
    }

    public void Enable(bool val)
    {
        _isEnable = val;
        if (_isEnable)
            ReturnCamera();
    }

    void Update()
    {
        if (_targetPos.HasValue)
        {
            var oPos = GameCameraHolder.transform.position;
            if (_period > 0)
            {
                var p = (Time.time - startMoveTime) / _period;
                if (p > 1)
                {
                    GameCameraHolder.transform.position = _targetPos.Value;
                    _targetPos = null;
                }
                else
                {
                    GameCameraHolder.transform.position = Vector3.Lerp(oPos, _targetPos.Value, p);
                    //                transform.position = Vector3.Lerp(_from, _bulletTarget.transform.position, p);
                }
            }
            else
            {
                GameCameraHolder.transform.position = _targetPos.Value;
                _targetPos = null;
            }
        }

        UpdateCamUpDown();
        UpdateWheel();
    }

    private void UpdateCamUpDown()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            MoveMainCamUp(-SpeedYByKey);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            MoveMainCamUp(SpeedYByKey);
        }
    }

    public void SetCameraTo(Vector3 v, float period)
    {
        if (period > 0)
        {
            startMoveTime = Time.time;
            _targetPos = v;
            _period = period;
        }
        else
        {
            GameCameraHolder.transform.position = v;
        }
    }

    public void MoveMainCamToDir(Vector3 keybordDir)
    {
        // if (_isEnable)
        // {
        keybordDir.y = 0;
        var dist = keybordDir.sqrMagnitude > 0;
        if (dist && _targetPos.HasValue)
        {
            _targetPos = null;
        }
        var nextPos = GameCameraHolder.transform.position + keybordDir * SpeedXZ;
        nextPos.z = Mathf.Clamp(nextPos.z, _min.z, _max.z);
        nextPos.x = Mathf.Clamp(nextPos.x, _min.x, _max.x);
        GameCameraHolder.transform.position = nextPos;
        // }
    }

    private void UpdateWheel()
    {
        var mouseScroll = Input.mouseScrollDelta;
        if (Math.Abs(mouseScroll.y) > 0.1f)
        {
            MoveMainCamUp(mouseScroll.y);
        }
    }

    public void MoveMainCamUp(float delta)
    {
        var p = GameCameraHolderY.transform.localPosition;
        var nextPos = Mathf.Clamp(p.y - delta * SpeedY, MIN_Y, MAX_Y);

        GameCameraHolderY.transform.localPosition = new Vector3(p.x, nextPos, p.z);
    }

    public void SetYHolderToMid()
    {
        var p = GameCameraHolderY.transform.localPosition;
        var delta = MAX_Y - MIN_Y;
        var nextPos = MIN_Y + delta * 0.25f;
        GameCameraHolderY.transform.localPosition = new Vector3(p.x, nextPos, p.z);
    }

    public void Stop()
    {
        _targetPos = null;
    }

    public Vector3 WorldToScreenPoint(Vector3 shipPosition)
    {
        return Camera.WorldToScreenPoint(shipPosition);
    }

    public Ray ScreenPointToRay(Vector3 pos)
    {
        return Camera.ScreenPointToRay(pos);
    }

    public void ApplyEMPEffect(float period)
    {
        if (Effects != null)
        {
            Effects.StartBloom(period);
        }

    }

    public void CheckAntiAlysing(bool fxaaEnable)
    {
        if (FxaaEffect != null)
        {
            FxaaEffect.enabled = fxaaEnable;
        }
    }

    public void SetEvent(EBattlefildEventType? eventType)
    {

        if (Effects != null)
        {
            Effects.SetEvent(eventType);
        }
    }

    public void SetNormalTemperature()
    {
        if (Effects != null)
            Effects.SetNormalTemperature();
    }
}

