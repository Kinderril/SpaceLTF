using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

[System.Serializable]
public class PilotParameters : IPilotParameters
{

    [field: NonSerialized]
    public event Action<IPilotParameters> OnLevelUp;


    public TotalStats Stats { get; set; }
    public PilotTactic Tactic { get; set; }

    public int HealthLevel { get { return MaxHealthLvl; } }
    public int ShieldLevel { get { return MaxShieldLvl; } }
    public int SpeedLevel { get { return MaxSpeedLvl; } }
    public int TurnSpeedLevel { get { return MaxTurnSpeedLvl; } }


    public int CurLevel
    {
        get { return HealthLevel + ShieldLevel + SpeedLevel + TurnSpeedLevel - 4 + 1; }
    }

    public int MaxHealthLvl = 1;
    public int MaxShieldLvl = 1;
    public int MaxSpeedLvl = 1;
    public int MaxTurnSpeedLvl = 1;
    public int DelayData = 0;

    public PilotMoneyData MoneyData;

    public PilotParameters()
    {
        MoneyData = new PilotMoneyData();
        Stats = new TotalStats();
        Tactic = new PilotTactic(ECommanderPriority1.Any,ESideAttack.Straight);
    }



    public bool CanUpgradeByLevel(int curValue)
    {
        return (curValue < Library.MAX_PILOT_PARAMETER_LEVEL);
    }

    public bool CanUpgradeAnyParameter(int phantomMoney)
    {
        if (MoneyData.CanLvlUp(CurLevel, phantomMoney))
        {
            return true;
        }
        return false;
    }
    
    public void AddMoney(int money)
    {
        MoneyData.AddMoney(money);
    }

    public LibraryPilotUpgradeType UpgradeRandomLevel(bool withMoney, bool withMsg)
    {
        List<LibraryPilotUpgradeType> all = new List<LibraryPilotUpgradeType>()
        {
            LibraryPilotUpgradeType.shield,
            LibraryPilotUpgradeType.speed,
            LibraryPilotUpgradeType.turnSpeed,
            LibraryPilotUpgradeType.health,
        };
        var p = all.RandomElement();
        UpgradeLevel(withMoney,p, withMsg);
        return p;
    }
    public void UpgradeLevel(bool withMoney,LibraryPilotUpgradeType type,bool withMsg)
    {
        if (CanUpgradeByLevel(CurLevel))
        {
            if (withMoney)
            {
                if (!CanUpgradeAnyParameter(0))
                {
                    return;
                }
                MoneyData.PayLvlUp(CurLevel);
            }
            var rnd = type;
            UpgradeLevelByType(rnd,withMsg);
        }
    }

    public int Money
    {
        get { return MoneyData.Money; }
    }

    public int Delay
    {
        get { return DelayData; }
        set { DelayData = value; }
    }

    public float PercentLevel => (float)Money / (float)Library.PilotLvlUpCost(CurLevel);

    public void UpgradeMaxHealthCoef()
    {
        MaxHealthLvl++;
    }

    public void UpgradeMaxShieldCoef()
    {
        MaxShieldLvl++;
    }

    public void UpgradeMaxSpeedCoef()
    {
        MaxSpeedLvl++;
    }

    public void UpgradeMaxTurnSpeedCoef()
    {
        MaxTurnSpeedLvl++;
    }

    public void UpgradeLevelByType(LibraryPilotUpgradeType rnd, bool withMsg)
    {
        switch (rnd)
        {
            case LibraryPilotUpgradeType.health:
                UpgradeMaxHealthCoef();
                break;
            case LibraryPilotUpgradeType.shield:
                UpgradeMaxShieldCoef();
                break;
            case LibraryPilotUpgradeType.speed:
                UpgradeMaxSpeedCoef();
                break;
            case LibraryPilotUpgradeType.turnSpeed:
                UpgradeMaxTurnSpeedCoef();
                break;
        }

        var msg = String.Format("Ship {0} upgraded!", Namings.ParameterName(rnd));
        UnityEngine.Debug.Log(msg.Yellow());
        if (withMsg)
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(msg);
        if (OnLevelUp != null)
        {
            OnLevelUp(this);
        }
    }

    public bool HaveMoney(int cost)
    {
        return Money >= cost;
    }

    public void RemoveMoney(int cost)
    {
        MoneyData.RemoveMoney(cost);
    }
}