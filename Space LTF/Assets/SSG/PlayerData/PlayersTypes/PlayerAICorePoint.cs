﻿using System.Collections.Generic;


[System.Serializable]
public class PlayerAICorePoint : PlayerAI
{
    public PlayerAICorePoint(string name)
        : base(name)
    {

    }

    public override ETurretBehaviour GetTurretBehaviour()
    {
        return ETurretBehaviour.nearBase;
    }

    public override LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(5);
        return base.GetReward(winner);
    }
}

