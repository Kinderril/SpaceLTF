using System;
using UnityEngine;


public abstract class ShipBoostAbstract : ShipData
{
    protected Action<bool> _activateCallback;
    protected Action _endCallback;
    protected bool _isActive;
    protected Action<Vector3> SetAddMoveCallback;
    public BaseEffectAbsorber Effect;
    public bool CanUse { get; set; }
    public bool IsActive
    {
        get { return _isActive; }
        protected set
        {
            _isActive = value;
            if (_isActive)
            {
                Effect.Play();
                _activateCallback(true);
            }
            else
            {
                Effect.StopEmmision();
                _endCallback();
            }
        }
    }

    protected ShipBoostAbstract(ShipBase owner, BaseEffectAbsorber effect, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(owner)
    {
        Effect = effect;
        SetAddMoveCallback = setAddMoveCallback;
        _activateCallback = activateCallback;
        _endCallback = endCallback;
    }
}

