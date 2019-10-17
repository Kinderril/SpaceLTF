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

    public void Init(BattleController battleController,BattlefildEventType? type,bool canRerandom)
    {
        _event = null;
        if (type == null && canRerandom)
        {
            if (MyExtensions.IsTrue01(0.15f))
            {
                type = _posibleTypes.RandomElement();
            }
        }

//#if UNITY_EDITOR    
//        type = BattlefildEventType.asteroids;
//#endif
        if (type != null)
        {
            switch (type.Value)
            {
                case BattlefildEventType.asteroids:
                    _event = new AsteroidEvent(battleController);
                    break;
                case BattlefildEventType.shieldsOff:
                    _event = new ShieldOffEvent(battleController);
                    break;
                case BattlefildEventType.engineOff:
                    break;
            }
            if (_event != null)
            {
                _event.Init();
            }
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
