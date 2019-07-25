using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UpDownBullet  :LinearBullet
{
    public float maxY = 3f;
    const float percentFall = .5f;
    const float percentFallCoef = 1f/percentFall;

    public override BulletType GetType
    {
        get { return BulletType.upDown;}
    }

    public override void LateInit()
    {
        _isActive = false;
        base.LateInit();
    }

    protected override void ManualUpdate()
    {
        UpdateLinear();
    }

    private void UpdateLinear()
    {
        _curTime += Time.deltaTime;
        var p = _curTime / _moveLifeTime;
        var pos = Vector3.Lerp(_startPos, _endPos, p);
        float lerp;
        if (p < percentFall)
        {
            lerp = Mathf.Lerp(0, maxY, p* percentFallCoef);
            _isActive = false;
        }
        else
        {
            _isActive = true;
            lerp = Mathf.Lerp(maxY, 0, (p- percentFall) * percentFallCoef);
        }
        pos.y = pos.y + lerp;
        transform.position = pos;
//        transform.rotation.SetLookRotation(,Vector3.up);
        if (p > DestroyPeriod)
        {
            Death();
        }
    }

}

