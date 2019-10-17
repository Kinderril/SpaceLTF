
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class PlayerReputationData
{
//    private Player _player;
    private const int MIN_REP = 0;
    public const int MAX_REP = 100;
    private const float MIN_COEF = 0.5f;
    private const float MAX_COEF = 2f;
    public int Reputation = 50;

    public Dictionary<ShipConfig,int> ReputationFaction = new Dictionary<ShipConfig, int>();

    [field: NonSerialized]
    public event Action<int> OnReputationChange;
    [field: NonSerialized]
    public event Action<ShipConfig,int> OnReputationNationChange;

    public PlayerReputationData()
    {
#if UNITY_EDITOR
        //        MoneyCount = 1000;
#endif
        //        _player = player;
        ReputationFaction.Add(ShipConfig.droid,30);
        ReputationFaction.Add(ShipConfig.federation,30);
        ReputationFaction.Add(ShipConfig.krios,30);
        ReputationFaction.Add(ShipConfig.mercenary,30);
        ReputationFaction.Add(ShipConfig.ocrons,30);
        ReputationFaction.Add(ShipConfig.raiders,30);
    }

    public void AddReputation(ShipConfig config, int val)
    {
        ReputationFaction[config] = ReputationFaction[config] + val;
        if (OnReputationNationChange != null)
        {
            OnReputationNationChange(config, val);
        }
    }
    public void AddReputation(int moneyToReward)
    {
        var old = Reputation;
        var sum = Reputation + moneyToReward;
        Reputation =  Mathf.Clamp(sum, MIN_REP,MAX_REP);
        var delta = Mathf.Abs(sum - old);
        if (delta > 0)
        {
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(String.Format(Namings.ReputationChanges,old,Reputation));
            if (OnReputationChange != null)
            {
                OnReputationChange(Reputation);
            }
        }
    }

    public void RemoveReputation(int costValue)
    {
        var old = Reputation;
        var sum = Reputation - costValue;
        Reputation = Mathf.Clamp(sum, MIN_REP, MAX_REP);
        var delta = Mathf.Abs(sum - old);
        if (delta > 0)
        {
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(String.Format(Namings.ReputationChanges, old, Reputation));
            if (OnReputationChange != null)
            {
                OnReputationChange(Reputation);
            }
        }
    }

    public int ModifBuyValue(int val)
    {
        var a = (float)(MIN_COEF - MAX_COEF) /(float) MAX_REP;//1.5/100 == 0.015
        var b = MAX_COEF;
        var coef = a*Reputation + b;
        return (int)(coef*(float) val);
    }

    public int ModifSellValue(int val)
    {
        var a = (float)(MAX_COEF - MIN_COEF) /(float) MAX_REP;//1.5/100 == 0.015
        var b = MIN_COEF;
        var coef = a*Reputation + b;
        return (int)(coef*(float) val);
    }

    public bool HaveMoney(int costValue)
    {
        return Reputation >= costValue;
    }

    public void Dispose()
    {
        OnReputationChange = null;
    }


}

