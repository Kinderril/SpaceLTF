﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LinearBullet : Bullet
{
    public float DestroyPeriod = 1f;
    protected float _moveLifeTime;
    private float _bornTime;

    public override void LateInit()
    {
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
#if UNITY_EDITOR
        if (_moveLifeTime < 0.3f)
        {
            Debug.LogError("bullet is too fast. _lifeTimeSec:" + _moveLifeTime.ToString("0.000"));
            Debug.LogError("_distanceShoot:" + _distanceShoot.ToString("0.000"));
            Debug.LogError("_curSpeed:" + _curSpeed.ToString("0.000"));
        }

#endif
    }

    protected void DoVisual(float f)
    {
        transform.position = Vector3.Lerp(_startPos, _endPos, f);
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

