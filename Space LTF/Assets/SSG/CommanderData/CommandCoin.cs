using System;
using UnityEngine;

public class CommandCoin
{
    private float _restoreEndTime;
    private float _lenghtTime;
    private ShipBase _ship;
    public bool _used;
    public bool _enableRegen;
    private float _remainCausePause;
    private float _remainPercent;
    public int Id { get; private set; }
    public float SpeedCoef { get; private set; }
    public event Action<CommandCoin, bool> OnUsed;
    private TimerManager.ITimer timer;


    public CommandCoin(int id,float speedCoef)
    {
        Id = id;
        SpeedCoef = speedCoef;
    }

    public bool Used
    {
        get { return _used; }
        private set
        {
            _used = value;
            if (OnUsed != null)
                OnUsed(this, value);
        }
    }
    public void Recharge()
    {
        timer.Stop();
        Used = false;
        _remainPercent = 0f;
    }


    //    public void SetUsed(ShipBase selectedShip)
    //    {
    //        _ship = selectedShip;
    ////        _ship.AddCoin();
    //        _ship.OnDispose += OnDispose;
    //        SetUsed();
    //    }

    public float Percent()
    {
        if (_enableRegen)
        {
            var p = RemainTime() / _lenghtTime;
            return p;
        }
        return _remainPercent;
    }

    public float RemainTime()
    {
        return (_restoreEndTime - Time.time);
    }

    public void SetUsed(float delay)
    {
        if (!_enableRegen)
        {
            _remainPercent = 1f;
        }
#if UNITY_EDITOR
        if (DebugParamsController.FastRecharge)
        {
            delay = 1f;
        }
#endif
        _lenghtTime = delay;
        _restoreEndTime = Time.time + _lenghtTime;
        Used = true;
        timer = MainController.Instance.TimerManager.MakeTimer(delay);
        timer.OnTimer += () =>
        {
            Used = false;
        };
    }
    
    public void Dispose()
    {
        OnUsed = null;
    }

    public void EnableRegen(bool b)
    {
        if (_enableRegen == b)
        {
            return;
        }

        if (_enableRegen)
        {
            _remainCausePause = (_restoreEndTime - Time.time);
            _remainPercent = Percent();
        }
        else
        {
            if (_remainPercent >= 1)
            {
                _restoreEndTime = 0;
            }
            else
            {
                _restoreEndTime = _remainCausePause + Time.time;
            }
        }
        _enableRegen = b;
    }

    public float BlockedPercent()
    {
        return _remainPercent;
    }

}