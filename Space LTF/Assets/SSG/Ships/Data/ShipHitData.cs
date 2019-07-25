using System;
using UnityEngine;
using System.Collections;

public enum ShipHitVisual
{
    soft,
    medium,
    hard,
}

public class ShipHitData
{
    private Transform _shaker;
    private bool _enable = false;
    //private ExternalSideForce _externalSide;
    private float _remainTime = 0;
    private float _powerX = 1f;
    private float _powerZ = 1f;
    private bool _overX = true;
    private bool _overZ = true;
    private bool _goingUp = true;
    private float _totalShakeTime = 2f;
    private float _startShakeTime = 2f;
    private float _endShakeTime = 2f;

    private float easingStart = 0f;
    private float easingEnd = 1f;

    private const float MIN = 0.05f;
    private const float SOFTMAX = 0.1f;
    private const float HARDMAX = 0.2f;
    private const float FIRST_STAGE = 0.3f;
    private const float REVERSE_STAGE = 1.2f;

    private NgMath.EasingFunction _easingFunction;

    public void Init(Transform transformToShake,Easing.EaseType easing)
    {
        //_externalSide = externalSide;
        _shaker = transformToShake;
        var easingFunction = Easing.GetEasingFunction(easing);
        _easingFunction = (start, end, value) => easingFunction(start, end, value);
        //_easingFunction = (start, end, value) => { return easingFunction(start, end, value); };
    }

    public void HitTo(ShipHitVisual power)
    {
//        Debug.LogError("Start:" + Time.time);
        _enable = true;
        _goingUp = true;
        _startShakeTime = Time.time;
        _totalShakeTime = FIRST_STAGE;
        _endShakeTime = _startShakeTime + _totalShakeTime;
        float max;
        switch (power)
        {
            case ShipHitVisual.soft:
                max = SOFTMAX;
                break;
            case ShipHitVisual.medium:
                max = SOFTMAX;
                //if (_externalSide != null)
                //    _externalSide.Init(1f, 0.3f, MyExtensions.IsTrueEqual() ? SideTurn.left : SideTurn.right);
                break;
            case ShipHitVisual.hard:
                max = HARDMAX;
                //if (_externalSide != null)
                //    _externalSide.Init(2f, 0.5f, MyExtensions.IsTrueEqual() ? SideTurn.left : SideTurn.right);
                break;
            default:
                max = SOFTMAX;
                break;
        }

        _overX = MyExtensions.IsTrueEqual();
        _overZ = MyExtensions.IsTrueEqual();
        _powerX = MyExtensions.Random(MIN, max);
        _powerZ = MyExtensions.Random(MIN, max);
        if (!_overX)
        {
            _powerX = -_powerX;
        }

        if (!_overZ)
        {
            _powerZ = -_powerZ;
        }
    }

    public void Update()
    {
        if (!_enable)
        {
            return;
        }

//        if (_remainTime > 0)
//        {
        var coreTime = (_endShakeTime - Time.time) / _totalShakeTime;
        float percent;
        if (_goingUp)
        {
            percent = 1f - coreTime; //0-->1    
        }
        else
        {
            percent = coreTime; //1-->0    
        }

        if (coreTime < 0.001f)
        {
            if (_goingUp)
            {
                DoReverse();
            }
            else
            {
                DoEnd();
            }
        }

        var percent2 = _easingFunction(easingStart, easingEnd, percent);
//        Debug.LogError("percent:" + percent2   +"   realPer:" + percent + "   coreTime:" + coreTime) ;
        var xx = _powerX * percent2;
        var zz = _powerZ * percent2;
        var q = new Quaternion(xx, 0, zz, 1f);
        _shaker.localRotation = q;
//        }
//        else
//        {
//        }
    }

    private void DoReverse()
    {
//        Debug.LogError(" <--->:" + Time.time);
        _totalShakeTime = REVERSE_STAGE;
        _endShakeTime = Time.time + _totalShakeTime;
        _goingUp = false;
    }

    private void DoEnd()
    {
        _shaker.localRotation = Quaternion.identity;
        _enable = false;
//        Debug.LogError("END " + Time.time);
    }
}