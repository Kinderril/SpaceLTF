using System.Collections.Generic;


[System.Serializable]
public class PlayerAIMainBoss : PlayerAI
{
    public PlayerAIMainBoss(string name, Dictionary<PlayerParameterType, int> startData = null)
        : base(name, startData)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(2);
        return new LastReward();
    }
}

