using System;
using System.Collections;
using UnityEngine;


public enum CointState
{
    Block,
    Ready,
    Recharging,
}

public class TempCoin
{
    private float _remainVal;
    private float _speedCoef = 1f;
    private float _maxPeriod = 1f;
    private CointState _state = CointState.Ready;
    public event Action<TempCoin> OnStateChange;
    public CointState State => _state;
    public int _id = 0;
    public float RemainVal => _remainVal;

    public TempCoin(int id)
    {
        _id = id;
    }

    public void Update()
    {
        if (_state != CointState.Recharging)
        {
            return;
        }
        if (_remainVal <= 0)
        {
            SetState(CointState.Ready);
            return;
        }
        _remainVal = _remainVal - Time.deltaTime * _speedCoef;
#if UNITY_EDITOR
        if (DebugParamsController.FastRecharge)
        {
            _remainVal = -1f;
        }
#endif

        if (_remainVal <= 0)
        {
            _remainVal = 0;
            SetState(CointState.Ready);
        }
    }

    public void SetState(CointState st)
    {
        if (st == _state)
        {
            return;
        }
        _state = st;
        OnStateChange?.Invoke(this);
        // Debug.LogError($"SetState id:{_id} st:{st.ToString()} ");
    }

    public void SetSpeedCoef(float coef)
    {
        _speedCoef = coef;
    }
                    
    public bool IsNotMax(float batteryPeriod)
    {
        return _remainVal < batteryPeriod;
    }

    public float GetVal()
    {
        return _remainVal;
    }

    public void AddVal(float period,float maxPeriod)
    {
        _remainVal += period;
        SetState(CointState.Block);
        _maxPeriod = maxPeriod;
    }
    public float Percent()
    {
        return _remainVal/ _maxPeriod;
    }

    public void SetValue(int usingSpellCostPeriod)
    {
        _remainVal = usingSpellCostPeriod;
    }
}
