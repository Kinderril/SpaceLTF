using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlefieldEventController
{
    private List<BattlefildEventType> _posibleTypes = new List<BattlefildEventType>()
    {
        BattlefildEventType.asteroids,BattlefildEventType.engineOff,BattlefildEventType.shieldsOff
    };

    private BattleFieldEvent _event = null;

    public void Init()
    {
        _event = null;
        if (MyExtensions.IsTrue01(0.15f))
        {
            var action = _posibleTypes.RandomElement();
            switch (action)
            {
                case BattlefildEventType.asteroids:
                    break;
                case BattlefildEventType.shieldsOff:
                    break;
                case BattlefildEventType.engineOff:
                    break;
            }

        }
        if (_event != null)
        {
            _event.Init();
        }
    }
    public void Dispose()
    {
        if (_event != null)
        {
            _event.Dispose();
            _event = null;
        }
    }
}
