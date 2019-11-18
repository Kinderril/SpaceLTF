using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[System.Serializable]
public class PilotParameters : IPilotParameters
{

    [field: NonSerialized]
    public event Action<IPilotParameters, PilotTcatic> OnTacticChange;
    [field: NonSerialized]
    public event Action<IPilotParameters> OnLevelUp;


    public TotalStats Stats { get; set; }
    //    public Dictionary<ShipType,float> PreferableShipTypeData =new Dictionary<ShipType, float>();
    //    public float MoneyGetterData = 1f;
    public PilotTcatic PilotTcaticlData;

    public int HealthLevel { get { return MaxHealthLvl; } }
    public int ShieldLevel { get { return MaxShieldLvl; } }
    public int SpeedLevel { get { return MaxSpeedLvl; } }
    public int TurnSpeedLevel { get { return MaxTurnSpeedLvl; } }

    //public HashSet<LibraryPilotUpgradeType> UnCheckedLevelUp => _unCheckedLevelUp;

    public int CurLevel
    {
        get { return HealthLevel + ShieldLevel + SpeedLevel + TurnSpeedLevel - 4 + 1; }
    }

    public int MaxHealthLvl = 1;
    public int MaxShieldLvl = 1;
    public int MaxSpeedLvl = 1;
    public int MaxTurnSpeedLvl = 1;
//    private List<LibraryPilotUpgradeType> upgradeOrder;
//    public int MoneyData = 1;
    public int DelayData = 0;
//    private HashSet<LibraryPilotUpgradeType> _unCheckedLevelUp = new HashSet<LibraryPilotUpgradeType>();

    public PilotMoneyData MoneyData;

    public PilotTcatic Tactic
    {
        get { return PilotTcaticlData; }
    }

    public PilotParameters()
    {
        Stats = new TotalStats();
    }

//    public LibraryPilotUpgradeType GetUpgradeType()
//    {
//        var upg = upgradeOrder.RandomElement();
//        upgradeOrder = upgradeOrder.Suffle();
//        return upg;
//    }

    public void Init(PilotTcatic taTcatic)
    {
        MoneyData = new PilotMoneyData();
//        upgradeOrder = ArmyCreator.PosiblePilotUpgrades(this);
//        upgradeOrder = upgradeOrder.Suffle();
//#if UNITY_EDITOR
//        DrawOrder("Init");
//#endif
        PilotTcaticlData = taTcatic;
        if (OnTacticChange != null)
        {
            OnTacticChange(this, PilotTcaticlData);
        }
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
        if (withMoney)
        {
            if (!CanUpgradeAnyParameter(0))
            {
                return;
            }
        }

        var rnd = type;
        if (CanUpgradeByLevel(CurLevel) && MoneyData.PayLvlUp(CurLevel))
        {
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

    public void ChangeTactic()
    {
        switch (PilotTcaticlData)
        {
            case PilotTcatic.defenceBase:
                PilotTcaticlData = PilotTcatic.attack;
                break;
            case PilotTcatic.attack:
                PilotTcaticlData = PilotTcatic.attackBase;
                break;
            case PilotTcatic.attackBase:
                PilotTcaticlData = PilotTcatic.defenceBase;
                break;
        }
        if (OnTacticChange != null)
        {
            OnTacticChange(this, PilotTcaticlData);
        }
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
        if (withMsg)
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(String.Format("Ship {0} upgraded!",Namings.ParameterName(rnd)));
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