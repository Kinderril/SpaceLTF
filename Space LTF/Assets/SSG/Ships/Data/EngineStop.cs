using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EngineStop
{

    private bool _isStop;
    public float RepairEndTime;
    public float Period;
    private MovingObject _movingObject;
    public event Action<MovingObject, bool> OnStop;
    private BaseEffectAbsorber _shipEngineStop;

    public EngineStop(MovingObject movingObject, BaseEffectAbsorber shipEngineStop)
    {
        _shipEngineStop = shipEngineStop;
           _movingObject = movingObject;
        _isStop = false;
    }

    public void Start()
    {
        _isStop = false;
        RaiseEvent();
        if (_shipEngineStop != null)
        {
            _shipEngineStop.Stop();
        }
    }

    private void Stop()
    {
        _isStop = true;
        RaiseEvent();
        if (_shipEngineStop != null)
        {
            _shipEngineStop.Play();
        }
    }

    private void RaiseEvent()
    {
        if (OnStop != null)
        {
            OnStop(_movingObject, _isStop);
        }
    }


    public void Stop(float delay)
    {
//        Debug.Log(Namings.TryFormat("Engine crahed for {0}",delay.ToString("0.0")).Red());
        Stop();
        Period = delay;
        RepairEndTime = Time.time + delay;
    }

    public bool IsCrash()
    {
        if (_isStop)
        {
            if (RepairEndTime < Time.time)
            {
                Start();
            }
        }
        return _isStop;
    }
}

