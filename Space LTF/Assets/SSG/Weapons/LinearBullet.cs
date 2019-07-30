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

    public override void LateInit()
    {
        base.Init();
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
        DoVisual(p);
        if (p > DestroyPeriod)
        {
            Death();
        }
    }
}

