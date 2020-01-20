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
    public ShipBoostLoop BoostLoop;
    public ShipBoostTwist BoostTwist;
    public ShipBoostHalfLoop BoostHalfLoop;
    private bool _isWorkable;
    public bool UseRotationByBoost;
    public bool IsActive { get; private set; }
    public float LoadPercent => 1f - Mathf.Clamp01((_nextBoostUse - Time.time) / _chargePeriod);
    public bool IsReady => _isWorkable && _nextBoostUse < Time.time;
    public Vector3 LastTurnAddtionalMove { get; private set; }

    public ShipBoost(ShipBase owner,float chargePeriod,bool isWorkable) 
        : base(owner)
    {
        _isWorkable = isWorkable;
        _chargePeriod = chargePeriod;
        _nextBoostUse = Time.time + chargePeriod;
        BoostTwist = new ShipBoostTwist(owner, 1f, Activate, EndBoost, SetAdditionaMove);
        BoostTurn = new ShipBoostTurn(owner,Activate,EndBoost,SetAdditionaMove);
        BoostLoop = new ShipBoostLoop(owner,1f, Activate, EndBoost, SetAdditionaMove);
        BoostHalfLoop = new ShipBoostHalfLoop(owner,1f, Activate, EndBoost, SetAdditionaMove);


        var posibleTricks = Library.PosibleTricks[owner.PilotParameters.Stats.CurRank];
        foreach (var ePilotTrickse in posibleTricks)
        {
            switch (ePilotTrickse)
            {
                case EPilotTricks.turn:
                    BoostTurn.CanUse = true;
                    break;
                case EPilotTricks.twist:
                    break;
                case EPilotTricks.loop:
                    BoostLoop.CanUse = true;
                    break;
            }
        }
    }

    private void SetAdditionaMove(Vector3 dir)
    {
        LastTurnAddtionalMove = dir;
    }

    private void EndBoost()
    {
        IsActive = false;
    }

    private void Activate(bool isBloackTurn)
    {
        UseRotationByBoost = isBloackTurn;
        IsActive = true;
    }



    public void SetUsed()
    {
        _nextBoostUse = Time.time + _chargePeriod;
    }

    public void EvadeToSide()
    {
        if (BoostTurn.CanUse)
        {
            SetUsed();
            var side = MyExtensions.IsTrueEqual() ? _owner.LookRight : _owner.LookLeft;
            BoostTurn.Activate(side);
        }
    }

    public void ManualUpdate()
    {
        BoostLoop.ManualUpdate();
    }

    public void ActivateLoop()
    {
        if (BoostLoop.CanUse)
            BoostLoop.Start();
    }

    public void ActivateBack()
    {
        if (BoostTurn.CanUse)
        {
            SetUsed();
            BoostTurn.Activate(-_owner.LookDirection);
        }
    }


    public void ActivateTurn(Vector3 targetDirNorm)
    {
        if (BoostTurn.CanUse)
        {
            SetUsed();
            // Debug.LogError("Actuivate targetDirNorm");
            BoostTurn.Activate(targetDirNorm);
        }
    }

    public void Deactivate()
    {
        BoostTurn.Deactivate();
    }
}

