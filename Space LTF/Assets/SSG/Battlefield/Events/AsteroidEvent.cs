using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidEvent : BattleFieldEvent
{
    public List<FlyingAsteroid> _asteroids = new List<FlyingAsteroid>();
    private TimerManager.ITimer _battleTimer;
    public AsteroidEvent(BattleController battle) : base(battle)
    {
        var allAsteroids = DataBaseController.Instance.DataStructPrefabs.FlyingAsteroids;

    }

    public override BattlefildEventType Type => BattlefildEventType.asteroids;
    public override void Init()
    {
        _battleTimer = MainController.Instance.BattleTimerManager.MakeTimer(3f, true);
        _battleTimer.OnTimer += OnTimer;
    }

    private void OnTimer()
    {
        

    }

    public override void Dispose()
    {

    }
}
