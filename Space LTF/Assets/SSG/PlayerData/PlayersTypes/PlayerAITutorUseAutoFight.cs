using System.Collections.Generic;


[System.Serializable]
public class PlayerAITutorUseAutoFight : PlayerAI
{
    public PlayerAITutorUseAutoFight(string name)
        : base(name)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        var reward = new LastReward();
//        reward.Money = 50;

        var cockpit = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.improved, ItemType.cocpit);
        winner.Inventory.TryAddItem(cockpit);
        reward.ParamItems.Add(cockpit);
        var wings = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.normal, ItemType.wings);
        winner.Inventory.TryAddItem(wings);
        reward.ParamItems.Add(wings);
        var engine = Library.CreateParameterItem(EParameterItemSubType.Middle, EParameterItemRarity.normal, ItemType.engine);
        winner.Inventory.TryAddItem(engine);
        reward.ParamItems.Add(engine);


        return reward;
    }
}

