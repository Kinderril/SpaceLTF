using System.Collections.Generic;

public class BattlefieldEventController
{
    private List<EBattlefildEventType> _posibleTypes = new List<EBattlefildEventType>()
    {
        EBattlefildEventType.asteroids,EBattlefildEventType.shieldsOff  ,
        EBattlefildEventType.fireVortex,EBattlefildEventType.Vortex     ,
        EBattlefildEventType.IceZone,EBattlefildEventType.BlackHole     ,
    };

    private BattleFieldEvent _event = null;

    public void Init(BattleController battleController, EBattlefildEventType? type, bool canRerandom)
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
        //        type = EBattlefildEventType.asteroids;
        //#endif
        if (type != null)
        {
            switch (type.Value)
            {
                case EBattlefildEventType.asteroids:
                    _event = new AsteroidEvent(battleController);
                    break;
                case EBattlefildEventType.shieldsOff:
                    _event = new ShieldOffEvent(battleController);
                    break;    
                case EBattlefildEventType.fireVortex:
                    _event = new FireVortexBattleEvent(battleController);
                    break;
                case EBattlefildEventType.Vortex:
                    _event = new VortexBattleEvent(battleController);
                    break;    
                case EBattlefildEventType.IceZone:
                    _event = new ZoneIceBattleEvent(battleController);
                    break;
                case EBattlefildEventType.BlackHole:
//                    _event = new BlackHoleBattleEvent(battleController);
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
