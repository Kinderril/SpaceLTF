using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidEvent : BattleFieldEvent
{
    public List<FlyingAsteroid> _asteroids = new List<FlyingAsteroid>();
    private List<TimerManager.ITimer> _battleTimers = new List<TimerManager.ITimer>();
    List<FlyingAsteroid> _allAsteroidsPrefabs;
    private const int maximumAsteroids = 50;
    private const int timers = 5;
    private Vector3 _baseDir;

    public AsteroidEvent(BattleController battle) : base(battle)
    {
        _allAsteroidsPrefabs = DataBaseController.Instance.DataStructPrefabs.FlyingAsteroids;

    }

    public override BattlefildEventType Type => BattlefildEventType.asteroids;
    public override void Init()
    {
        var rndX = MyExtensions.Random(-1f, 1f);
        var rndZ = MyExtensions.Random(-1f, 1f);
        var dir = Utils.NormalizeFastSelf(new Vector3(rndX, 0, rndZ));
        _baseDir = dir;
        for (int i = 0; i < timers; i++)
        {
            var battleTimer = MainController.Instance.BattleTimerManager.MakeTimer(MyExtensions.Random(0.7f,1.5f), true);
            battleTimer.OnTimer += OnTimer;
            _battleTimers.Add(battleTimer);
        }
    }

    private void OnTimer()
    {
        CreateAsteroid();
    }

    private void CreateAsteroid()
    {
        if (_asteroids.Count < maximumAsteroids)
        {
            var rnd = _allAsteroidsPrefabs.RandomElement();
            var center = _battle.CellController.Data.CenterZone;
            var rad = _battle.CellController.Data.Radius * 1.5f;
            Vector3 subDir;
            if (MyExtensions.IsTrueEqual())
            {
                subDir = Utils.Rotate90(_baseDir, SideTurn.left);
            }
            else
            {
                subDir = Utils.Rotate90(_baseDir, SideTurn.right);
            }

            float subDirOffset = 50f;
            var dir = Utils.RotateOnAngUp(_baseDir,MyExtensions.Random(-15f,15f));
            var startPoint = center + rad * dir + subDir * MyExtensions.Random(-subDirOffset, subDirOffset);
            var asteroid = DataBaseController.GetItem(rnd);
            asteroid.Init(callbackDeath, startPoint, -dir, rad * 2f);
            _asteroids.Add(asteroid);
        }
    }

    private void callbackDeath(FlyingAsteroid obj)
    {

        _asteroids.Remove(obj);

    }

    public override void Dispose()
    {
        foreach (var battleTimer in _battleTimers)
        {
            if (battleTimer != null && battleTimer.IsActive)
            {
                battleTimer.Stop();
            }
        }

        foreach (var flyingAsteroid in _asteroids)
        {
            GameObject.Destroy(flyingAsteroid);
        }
        _asteroids.Clear();
        _battleTimers.Clear();
    }
}
