using System.Collections.Generic;



[System.Serializable]
public class PlayerAI : Player
{

    public PlayerAI(string name)
        :
        base(name)
    {

    }

    public virtual LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(1);
        return new LastReward(this, winner);
    }
    public override ETurretBehaviour GetTurretBehaviour()
    {
        return ETurretBehaviour.stayAtPoint;
    }

    public virtual bool DoBaseDefence()
    {
        return false;
    }
}

