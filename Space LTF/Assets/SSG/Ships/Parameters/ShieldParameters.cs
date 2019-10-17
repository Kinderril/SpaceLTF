using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ShieldChangeSt
{
//    changeToActive,
    active,
    restoring,
    disable,
}

public delegate void ShieldParameterChange(float curent, float max, float delta, ShieldChangeSt state, ShipBase shipOwner);

public class ShieldParameters
{
    private const float RestoreShieldPercent = 0.3f;
    private Collider _shieldCollider;
    public float ShieldRegenPerSec { get; set; }
    private float _nextTimeShiledRegen;
    public ShieldChangeSt State { get; private set; }
    private float _curShiled;
    private float _endDisablePeriod;
    private Collider shieldCollider;
    private float ShieldDeltaTick = 1f;
    private ShipBase _shipBase;
    private float _restoreTime;
    private float _startRestore;

    public event ShieldParameterChange OnShildChanged;
    public event Action<ShieldChangeSt> OnStateChange;

    public ShieldParameters(ShipBase shipBase,Collider shieldCollider, float shiledRegen, float maxShiled)
    {
        _shipBase = shipBase;
        this._shieldCollider = shieldCollider;
        this.ShieldRegenPerSec = shiledRegen;
        MaxShield = maxShiled;
        CurShiled = MaxShield;
    }

    public float MaxShield { get; set; }

    public float NextTimeShiledRegen
    {
        get { return _nextTimeShiledRegen; }
        set { _nextTimeShiledRegen = value; }
    }
    public bool ShiledIsActive
    {
        get { return State == ShieldChangeSt.active; }
    }

    public float CurShiled
    {
        get
        {
            return _curShiled;
        }
        set
        {
//            if (ShieldBroken)
//            {
//                return;
//            }
            var delta = value - _curShiled;
            var isActive = State == ShieldChangeSt.active;
            if (delta > 0)
            {
               _curShiled = value;
            }
            else
            {
                if (isActive)
                    _curShiled = value;
               }
            if (isActive)
            {
                if (_curShiled <= 0f)
                {
                    SetState(ShieldChangeSt.restoring);
                }
            }

            //            if (_shiledIsActive)
            //            {
            //                ShiledIsActive = _curShiled > 0.9f;
            //            }
            //            else
            //            {
            //                ShiledIsActive = _curShiled > MaxShiled * 0.1f;
            //            }
        }
    }

    public void Crash(float period = -1)
    {
        if (period > 0)
        {
            _endDisablePeriod = Time.time + period;
        }
        if (State == ShieldChangeSt.disable)
        {
            return;
        }
        SetState(ShieldChangeSt.disable);
    }


    public void Enable()
    {
        if (State == ShieldChangeSt.disable)
        {
            SetState(ShieldChangeSt.restoring);
        }
    }

    public void ShiledAction(float delta)
    {
//        if (ShieldBroken)
//        {
//            return;
//        }
        if (OnShildChanged != null)
        {
            OnShildChanged(CurShiled, MaxShield, delta, State,_shipBase);
        }
    }

    public void HealShield(float v)
    {
//        ShieldBroken = false;
        var c = CurShiled + v;
        var d = MaxShield - CurShiled;
        if (d <= 0)
        {
            return;
        }
        if (c > MaxShield)
        {
            c = MaxShield;
        }
        var delta = c - CurShiled;
        CurShiled = c;
        EffectController.Instance.Create(DataBaseController.Instance.DataStructPrefabs.ShieldChagedEffect, _shipBase.transform, 5f);
        ShiledAction(delta);
    }

    private void RegenShield()
    {
        if (NextTimeShiledRegen < Time.time)
        {
            NextTimeShiledRegen = Time.time + ShieldDeltaTick;
            if (CurShiled < MaxShield)
            {
                var d = ShieldRegenPerSec * ShieldDeltaTick;
                if (d > 0)
                {
                    CurShiled = Mathf.Clamp(CurShiled + d, 0, MaxShield);
                    ShiledAction(d);
                }
            }
        }
    }

    public void Update()
    {
        switch (State)
        {
            case ShieldChangeSt.disable:
                if (_endDisablePeriod > 0)
                {
                    if (Time.time > _endDisablePeriod)
                    {
                        _endDisablePeriod = -1;
                        if (CanActivate())
                        {
                            SetState(ShieldChangeSt.active);
                        }
                        else
                        {
                            SetState(ShieldChangeSt.restoring);
                        }
                    }
                }
                break;
            case ShieldChangeSt.active:
                RegenShield();
                break;
            case ShieldChangeSt.restoring:
                if (CanActivate())
                {
                    if (Time.time - _startRestore > 5)
                    {
                        SetState(ShieldChangeSt.active);
                    }
                }
                if (_restoreTime > Time.time)
                {
                    RegenShield();
                }
                break;
        }
    }

    private bool CanActivate()
    {
        return (_curShiled > MaxShield * RestoreShieldPercent);
    }

    private void SetState(ShieldChangeSt v)
    {
        if (State == ShieldChangeSt.disable && v == ShieldChangeSt.active)
        {
            return;
        }
        if (State == v)
        {
            return;
        }

        State = v;
        switch (State)
        {
            case ShieldChangeSt.restoring:
                _restoreTime = Time.time + 5f;
                _startRestore = Time.time;
                break;
            case ShieldChangeSt.active:

                break;
            case ShieldChangeSt.disable:
                break;
        }
        if (OnStateChange != null)
        {
            OnStateChange(v);
        }

        _shieldCollider.gameObject.SetActive(ShiledIsActive);
    }

    public void Dispose()
    {
        OnShildChanged = null;
    }

    public void Repair()
    {
        
    }
}

