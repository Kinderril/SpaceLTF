using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class PlayerAITutorWearModuls : PlayerAI
{
    public PlayerAITutorWearModuls(string name)
        : base(name)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        var reward = new LastReward();
        return reward;
    }
}

