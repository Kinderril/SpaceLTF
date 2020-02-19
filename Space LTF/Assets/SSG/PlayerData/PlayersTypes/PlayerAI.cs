using System.Collections.Generic;



[System.Serializable]
public class PlayerAI : Player
{
    public PlayerAI(string name, Dictionary<PlayerParameterType, int> startData = null)
        :
        base(name, startData)
    {

    }

    public virtual LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(1);
        return new LastReward(this, winner);
    }
}

