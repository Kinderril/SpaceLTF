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

//    public override void Init()
//    {
//        GlobalEventDispatcher.OnShipDeath += OnShipDeath;
//    }

    private void OnShipDeath(ShipBase target, ShipBase killer)
    {
        if (killer.ShipParameters.StartParams.ShipType == _type && target.TeamIndex == TeamIndex.red)
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