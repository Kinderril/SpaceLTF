using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


public  abstract class AbstractAttackAction : BaseAction
{
    protected float _nextCheckTwist;
    protected float _nextCheckRam;
    protected float _nextCheckTurn;
    public AbstractAttackAction([NotNull] ShipBase owner, ActionType actionType)
        : base(owner, actionType)
    {

    }
    protected void CheckBoostTwistByShoot(ShipBase shooter)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTwist < Time.time)
        {
            _nextCheckTwist = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Enemies.TryGetValue(shooter, out var data))
            {
                _owner.Boost.BoostTwist.Activate(data);
            }
        }
    }

    protected void CheckBoostTwist(IShipData Target)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTwist < Time.time)
        {
            _nextCheckTwist = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD); ;
            if (_owner.Boost.BoostTwist.ShallStartUse(Target))
            {
                _owner.Boost.BoostTwist.Activate(Target);
            }
        }
    }
    protected void CheckBoostRam(IShipData Target)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckRam < Time.time)
        {
            _nextCheckRam = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Boost.BoostRam.ShallStartUse(Target))
            {
                _owner.Boost.BoostRam.Activate();
            }
        }
    }

    protected void CheckBoostTurn(IShipData Target)
    {
        if (!_owner.Boost.IsReady)
        {
            return;
        }

        if (_nextCheckTurn < Time.time)
        {
            _nextCheckTurn = Time.time + MyExtensions.GreateRandom(TRICK_CHECK_PERIOD);
            if (_owner.Boost.BoostTurn.ShallStartUse(Target))
            {
                _owner.Boost.BoostTurn.Activate(Target.DirNorm);
            }
        }
    }
}

