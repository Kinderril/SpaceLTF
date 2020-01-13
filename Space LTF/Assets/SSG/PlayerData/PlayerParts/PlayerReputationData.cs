
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public delegate void ReputationChangeDelegate(ShipConfig config, int curVal, int delta);

[System.Serializable]
public class PlayerReputationData
{
//    private Player _player;
    private const int MIN_REP = -100;
    public const int MAX_REP = 100;
    private const float MIN_COEF = 0.5f;
    private const float MAX_COEF = 2f;
    private const int REP_DIFF = 10;

    public Dictionary<ShipConfig,int> ReputationFaction = new Dictionary<ShipConfig, int>();
    public Dictionary<ShipConfig,List<ShipConfig>> Enemies = new Dictionary<ShipConfig, List<ShipConfig>>();


    [field: NonSerialized]
    public event ReputationChangeDelegate OnReputationNationChange;

    public PlayerReputationData()
    {
#if UNITY_EDITOR
        //        MoneyCount = 1000;
#endif
        //        _player = player;
        ReputationFaction.Add(ShipConfig.droid,-100);
        ReputationFaction.Add(ShipConfig.federation,MyExtensions.Random(-REP_DIFF,REP_DIFF));
        ReputationFaction.Add(ShipConfig.krios, MyExtensions.Random(-REP_DIFF, REP_DIFF));
        ReputationFaction.Add(ShipConfig.mercenary, MyExtensions.Random(-REP_DIFF, REP_DIFF));
        ReputationFaction.Add(ShipConfig.ocrons, MyExtensions.Random(-REP_DIFF, REP_DIFF));
        ReputationFaction.Add(ShipConfig.raiders, MyExtensions.Random(-REP_DIFF, REP_DIFF));

        Enemies.Add(ShipConfig.federation,new List<ShipConfig>() {ShipConfig.krios,ShipConfig.ocrons});
        Enemies.Add(ShipConfig.krios, new List<ShipConfig>() {ShipConfig.mercenary, ShipConfig.federation });
        Enemies.Add(ShipConfig.ocrons, new List<ShipConfig>() {ShipConfig.mercenary, ShipConfig.raiders });
        Enemies.Add(ShipConfig.mercenary, new List<ShipConfig>() {ShipConfig.federation, ShipConfig.raiders });
        Enemies.Add(ShipConfig.raiders, new List<ShipConfig>() {ShipConfig.krios, ShipConfig.ocrons });
        Enemies.Add(ShipConfig.droid, new List<ShipConfig>());
    }

    public void AddReputation(ShipConfig config, int val)
    {
        var old = ReputationFaction[config];
        var sum = old + val;
        var clampedVal = Mathf.Clamp(sum, MIN_REP, MAX_REP);
        ReputationFaction[config] = clampedVal;
        var delta = Mathf.Abs(sum - old);
        if (delta > 0)
        {
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(String.Format(Namings.ReputationChanges, old, clampedVal, Namings.ShipConfig(config)));
            if (OnReputationNationChange != null)
            {
                OnReputationNationChange(config, val, delta);
            }
        }
    }


    public void RemoveReputation(ShipConfig config, int val)
    {
        var old = ReputationFaction[config];
        var sum = old - val;
        var clampedVal = Mathf.Clamp(sum, MIN_REP, MAX_REP);
        ReputationFaction[config] = clampedVal;
        var delta = Mathf.Abs(sum - old);
        if (delta > 0)
        {
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(String.Format(Namings.ReputationChanges, old, clampedVal, Namings.ShipConfig(config)));
            if (OnReputationNationChange != null)
            {
                OnReputationNationChange(config, val, delta);
            }
        }
    }

    public int ModifBuyValue(ShipConfig config,int val)
    {
        var a = (float)(MIN_COEF - MAX_COEF) /(float) MAX_REP;//1.5/100 == 0.015
        var b = MAX_COEF;
        var coef = a*ReputationFaction[config] + b;
        return (int)(coef*(float) val);
    }

    public int ModifSellValue(ShipConfig config,int val)
    {
        var a = (float)(MAX_COEF - MIN_COEF) /(float) MAX_REP;//1.5/100 == 0.015
        var b = MIN_COEF;
        var coef = a* ReputationFaction[config] + b;
        return (int)(coef*(float) val);
    }


    public void Dispose()
    {
        OnReputationNationChange = null;
    }


    public void WinBattleAgainst(ShipConfig config)
    {
        RemoveReputation(config,Library.BATTLE_REPUTATION_AFTER_FIGHT);
        if (MyExtensions.IsTrueEqual())
        {
            var enemy = Enemies[config];
            if (enemy.Count > 0)
            {
                ShipConfig repToAdd = enemy.RandomElement();
                AddReputation(repToAdd, Library.BATTLE_REPUTATION_AFTER_FIGHT * 2);
            }
        }
    }
}

