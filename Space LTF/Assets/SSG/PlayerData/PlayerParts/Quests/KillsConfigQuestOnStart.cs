using System;

[System.Serializable]
public class KillsConfigQuestOnStart : BaseQuestOnStart
{
    private ShipConfig _type;

    public KillsConfigQuestOnStart(ShipConfig type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }

    public override void Init()
    {
        GlobalEventDispatcher.OnShipDeath += OnShipDeath;
    }

    private void OnShipDeath(ShipBase target, ShipBase killer)
    {
        if (target.ShipParameters.StartParams.ShipConfig == _type && target.TeamIndex == TeamIndex.red)
        {
            AddCount();
        }
    }

    public override void Dispose()
    {
        GlobalEventDispatcher.OnShipDeath -= OnShipDeath;
    }
}