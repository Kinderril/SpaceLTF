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
    private float _curDistValue;
    private float _speed = 0.3f;
    private bool _isDistortion;
    private bool _distortionDirection;

    public event ShieldParameterChange OnShildChanged;
    public event Action<ShieldChangeSt> OnStateChange;
    private Material _materialToChange = null;

    public ShieldParameters(ShipBase shipBase,Collider shieldCollider, float shiledRegen, float maxShiled)
    {
        _shipBase = shipBase;
        this._shieldCollider = shieldCollider;
        if (_shieldCollider != null)
        {
            var settings = MainController.Instance.Settings;
            _shieldCollider.gameObject.layer = LayerMaskController.ShieldLayer;
            var renderer = _shieldCollider.gameObject.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                _materialToChange = Utils.CopyMaterial(renderer);
                switch (shipBase.TeamIndex)
                {
                    case TeamIndex.red:
                        _materialToChange.SetColor("_LitColor", settings.ShieldRed);
                        break;
                    case TeamIndex.green:
                        _materialToChange.SetColor("_LitColor", settings.ShieldGreen);
                        break;
                }
            }
        }
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
                GetHit();
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
        }
    }

    private void GetHit()
    {
        StartDistortion();
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
        OnShildChanged?.Invoke(CurShiled, MaxShield, delta, State,_shipBase);
    }

    public void HealShield(float v, bool withStateChange = false)
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
        if (withStateChange)
        {
            if (State != ShieldChangeSt.active)
            {
                SetState(ShieldChangeSt.active);
            }
        }
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
        UpdateDistortion();
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


    public void StartDistortion()
    {
        if (_materialToChange == null)
        {
            return;
        }

        _curDistValue = 0f;
        _distortionDirection = true;
        _isDistortion = true;
//        Debug.LogError("StartDistortion");
    }

    private void UpdateDistortion()
    {
        if (_isDistortion)
        {
            var val = _speed * Time.deltaTime;
            if (_distortionDirection)
            {
                if (_curDistValue < 0.65f)
                {
                    val *= 4f;
                }
                _curDistValue += val;
                if (_curDistValue > 1)
                {
                    _curDistValue = 1f;
                    _distortionDirection = false;
                }
            }
            else
            {

                _curDistValue -= 4 * val;
                if (_curDistValue < 0)
                {
                    _curDistValue = 0f;
                    _isDistortion = false;
                }

            }
            _materialToChange.SetFloat("_DissortAmt",_curDistValue);
//            Debug.LogError($"SetFloat {_shipBase.Id}  :{_curDistValue}");
        }
    }

    private bool CanActivate()
    {
        return (_curShiled > MaxShield * RestoreShieldPercent);
    }

    private void SetState(ShieldChangeSt v)
    {
//        if (State == ShieldChangeSt.disable && v == ShieldChangeSt.active)
//        {
//            return;
//        }
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

