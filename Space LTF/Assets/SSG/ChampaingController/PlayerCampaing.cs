using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerCampaing : Player
{
    public PlayerCampaing(string name) : base(name)
    {
    }

    public PlayerCampaing(string name, PlayerSafe linkedData) : base(name, linkedData)
    {
    }

    public PlayerCampaing(string name, PlayerSafe linkedData, PlayerReputationData rep) : base(name, linkedData, rep)
    {
    }
}
