public enum BattlefildEventType
{
    asteroids,
    shieldsOff,
    fireVortex,
    Vortex,
    IceZone,
    BlackHole,
}

public abstract class BattleFieldEvent
{
    protected BattleController _battle;
    public BattleFieldEvent(BattleController battle)
    {
        _battle = battle;
    }

    public abstract BattlefildEventType Type
    {
        get;
    }

    public abstract void Init();
    public abstract void Dispose();
}
