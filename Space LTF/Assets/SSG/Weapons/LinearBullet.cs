using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LinearBullet : Bullet
{
    public float DestroyPeriod = 1f;
    protected float _moveLifeTime;
    private float _bornTime;
    private Vector3 _externalTotalOffset;
//    private Vector3 _curDir;

    public override void LateInit()
    {
        _externalTotalOffset = Vector3.zero;
        base.Init();
        _traileChecked = false;
        if (TrailEffect != null)
        {
            TrailEffect.Play();
        }
        if (TrailEffect != null)
        {
            TrailEffect.StartEmmision();
        }
        _bornTime = Time.time;
        _moveLifeTime = _distanceShoot / _curSpeed;
        transform.position = _startPos;
        LookDirection = Utils.NormalizeFastSelf(_endPos - _startPos);

//#if UNITY_EDITOR
//        if (_moveLifeTime < 0.3f)
//        {
//            Debug.LogError("bullet is too fast. _lifeTimeSec:" + _moveLifeTime.ToString("0.000"));
//            Debug.LogError("_distanceShoot:" + _distanceShoot.ToString("0.000"));
//            Debug.LogError("_curSpeed:" + _curSpeed.ToString("0.000"));
//        }
//
//#endif
    }

    protected void DoVisual(float f)
    {
        //        if (ExternalForce.IsActive)
        //        {
        //            _externalTotalOffset += ExternalForce.Update();
        //        }
        //        transform.position = Vector3.Lerp(_startPos, _endPos, f) + _externalTotalOffset;

//        if (ExternalSideForce.IsActive)
//        {
//            _curDir = ExternalSideForce.GetLerpPercent(_curDir);
//            Rotation = Quaternion.FromToRotation(Vector3.forward, _curDir);
//        }

        Vector3 externalForce = ExternalForce.IsActive ? ExternalForce.Update() : Vector3.zero;
        transform.position = transform.position + _curSpeed * LookDirection * Time.deltaTime + externalForce;
    }

    public override BulletType GetType => BulletType.linear;

    protected override void ManualUpdate()
    {
        UpdateLinear();
    }

    private void UpdateLinear()
    {

        _curTime += Time.deltaTime;
        
        var p = _curTime / _moveLifeTime;
        if (!_traileChecked && p > 0.1f)
        {
            RecheckTrail();
        }
        DoVisual(p);
        if (p > DestroyPeriod)
        {
            Death();
        }
    }

    private void RecheckTrail()
    {
        if (!_traileChecked)
        {
            _traileChecked = true;
            if (TrailEffect != null)
            {
                if (!TrailEffect.gameObject.activeSelf)
                {
                    TrailEffect.Play();
                }
            }
        }
    }
}

