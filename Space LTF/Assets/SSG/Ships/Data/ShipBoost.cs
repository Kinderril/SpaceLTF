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

    public ShipBoost(ShipBase owner,float chargePeriod) : base(owner)
    {
        _chargePeriod = chargePeriod;
        _nextBoostUse = Time.time + chargePeriod;
        BoostTurn = new ShipBoostTurn(owner,2.5f);
    }


//    public void ActivateTurn()
//    {
//        BoostTurn.Activate();
//        Debug.LogError("Actuivate to dir");
//        SetUsed();
//    }

    public float LoadPercent => 1f - Mathf.Clamp01((_nextBoostUse - Time.time) / _chargePeriod);

    public bool CanUse => _nextBoostUse < Time.time;

    public void SetUsed()
    {
        _nextBoostUse = Time.time + _chargePeriod;
    }

    public void EvadeToSide()
    {
        SetUsed();
        Debug.LogError("Actuivate EvadeToSide");
        var side = MyExtensions.IsTrueEqual()? _owner.LookRight: _owner.LookLeft;
        BoostTurn.Activate(side);

    }

    public void ActivateBack()
    {
        SetUsed();
        Debug.LogError("Actuivate ActivateBack");
        BoostTurn.Activate(-_owner.LookDirection);
    }

    public void ActivateFront()
    {
        SetUsed();
        Debug.LogError("Actuivate ActivateFront");
        var side = MyExtensions.IsTrueEqual() ? _owner.LookRight : _owner.LookLeft;
        BoostTurn.Activate(side);

    }

    public void ActivateTurn(Vector3 targetDirNorm)
    {
        SetUsed();
        Debug.LogError("Actuivate targetDirNorm");
        BoostTurn.Activate(targetDirNorm);

    }
}

