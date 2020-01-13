using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ShipBoostAbstract : ShipData
{
    protected Action<bool> _activateCallback;
    protected Action _endCallback;
    protected bool _isActive;
    protected Action<Vector3> SetAddMove;
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

    protected ShipBoostAbstract(ShipBase owner, Action<bool> activateCallback, Action endCallback, Action<Vector3> setAddMove)
        : base(owner)
    {
        SetAddMove = setAddMove;
        _activateCallback = activateCallback;
        _endCallback = endCallback;
    }
}

