using System;

[System.Serializable]
public class KillsTypeQuestOnStart : BaseQuestOnStart
{
    private ShipType _type;

    public KillsTypeQuestOnStart(ShipType type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }


    private void OnShipDeath(ShipBase target, ShipBase killer)
    {
        if (target.ShipParameters.StartParams.ShipType == _type && target.TeamIndex == TeamIndex.red)
        {
            AddCount();
        }
    }

    protected override bool StageActivate(Player player)
    {
        GlobalEventDispatcher.OnShipDeath += OnShipDeath;
        return true;
    }

    protected override void StageDispose()
    {
        GlobalEventDispatcher.OnShipDeath -= OnShipDeath;
    }
}