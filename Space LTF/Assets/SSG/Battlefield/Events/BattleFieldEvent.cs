public enum EBattlefildEventType
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

    public abstract EBattlefildEventType Type
    {
        get;
    }

    public abstract void Init();
    public abstract void Dispose();
}
