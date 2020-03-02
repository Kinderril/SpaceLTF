using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class TimerModul : ActionModulInGame
{
    private TimerManager.ITimer _timer;

    public TimerModul(BaseModulInv baseModulInv) 
        : base(baseModulInv)
    {
        Period = Delay();
    }

    protected abstract float Delay();
    
    protected abstract void  TimerAction();
    

    public override void Apply(ShipParameters Parameters, ShipBase owner)
    {
        if (_timer == null || !_timer.IsActive)
        {
            _timer = MainController.Instance.BattleTimerManager.MakeTimer(0.7f, true);
            _timer.OnTimer += OnTimer;
        }
        base.Apply(Parameters,owner);
    }

    private void OnTimer()
    {
        bool isReady = IsReady();
        if (isReady)
        {
            TimerAction();
        }
    }

    public override void Dispose()
    {
        if (_timer != null)
        {
            _timer.Stop();
        }
    }

    public override void Delete()
    {
        _timer.Stop();
        base.Delete();
    }


}

