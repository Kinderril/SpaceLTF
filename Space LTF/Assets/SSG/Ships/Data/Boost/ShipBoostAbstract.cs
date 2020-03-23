using System;
using UnityEngine;


public abstract class ShipBoostAbstract : ShipData
{
    protected Action<bool> _activateCallback;
    protected Action _endCallback;
    protected bool _isActive;
    protected Action<Vector3> SetAddMoveCallback;
    public bool CanUse { get; set; }
    public bool IsActive
    {
        get { return _isActive; }
        protected set
        {
            _isActive = value;
            if (_isActive)
            {
                _activateCallback(true);
            }
            else
            {
                _endCallback();
            }
        }
    }

    protected ShipBoostAbstract(ShipBase owner, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMoveCallback)
        : base(owner)
    {
        SetAddMoveCallback = setAddMoveCallback;
        _activateCallback = activateCallback;
        _endCallback = endCallback;
    }
}

