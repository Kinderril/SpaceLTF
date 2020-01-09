using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ShipBoost : ShipData
{
    private float _nextBoostUse;
    private float _chargePeriod;

    public ShipBoostTurn BoostTurn;
    public ShipBoostBackflip BoostBackflip;
    private bool _isWorkable;

    public ShipBoost(ShipBase owner,float chargePeriod,bool isWorkable) 
        : base(owner)
    {
        _isWorkable = isWorkable;
        _chargePeriod = chargePeriod;
        _nextBoostUse = Time.time + chargePeriod;
        BoostTurn = new ShipBoostTurn(owner);
        BoostBackflip = new ShipBoostBackflip(owner,1f);
    }


    public float LoadPercent => 1f - Mathf.Clamp01((_nextBoostUse - Time.time) / _chargePeriod);
    public bool CanUse => _isWorkable && _nextBoostUse < Time.time;

    public void SetUsed()
    {
        _nextBoostUse = Time.time + _chargePeriod;
    }

    public void EvadeToSide()
    {
        SetUsed();
        // Debug.LogError("Actuivate EvadeToSide");
        var side = MyExtensions.IsTrueEqual()? _owner.LookRight: _owner.LookLeft;
        BoostTurn.Activate(side);
    }

    public void ManualUpdate()
    {
        BoostBackflip.ManualUpdate();
    }

    public void ActivateBackflip()
    {
        BoostBackflip.Start();
    }

    public void ActivateBack()
    {
        SetUsed();
        // Debug.LogError("Actuivate ActivateBack");
        BoostTurn.Activate(-_owner.LookDirection);
    }

    public void ActivateFront()
    {
        SetUsed();
        // Debug.LogError("Actuivate ActivateFront");
        var side = MyExtensions.IsTrueEqual() ? _owner.LookRight : _owner.LookLeft;
        BoostTurn.Activate(side);
    }

    public void ActivateTurn(Vector3 targetDirNorm)
    {
        SetUsed();
        // Debug.LogError("Actuivate targetDirNorm");
        BoostTurn.Activate(targetDirNorm);
    }

    public void Deactivate()
    {
        BoostTurn.Deactivate();
    }
}

