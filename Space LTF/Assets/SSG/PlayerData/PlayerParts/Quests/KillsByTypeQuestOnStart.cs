using System;

[System.Serializable]
public class KillsByTypeQuestOnStart : BaseQuestOnStart
{
    private ShipType _type;

    public KillsByTypeQuestOnStart(ShipType type, int target,EQuestOnStart typeQuest)
        :base(target, typeQuest)
    {
        _type = type;
    }

    public override void Init()
    {
        GlobalEventDispatcher.OnShipDeath += OnShipDeath;
    }

    private void OnShipDeath(ShipBase target, ShipBase killer)
    {
        if (killer.ShipParameters.StartParams.ShipType == _type && target.TeamIndex == TeamIndex.red)
        {
            AddCount();
        }
    }

    public override void Dispose()
    {
        GlobalEventDispatcher.OnShipDeath -= OnShipDeath;
    }
}