using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IPilotParameters
{
    bool CanUpgradeByLevel(int curValue);
    bool CanUpgradeAnyParameter(int phantomMoney);

    int HealthLevel { get; }
    int ShieldLevel { get; }
    int SpeedLevel { get; }
    int TurnSpeedLevel { get; }

    float PercentLevel { get; }
    //HashSet<LibraryPilotUpgradeType> UnCheckedLevelUp { get; }

    int CurLevel { get; }
    int Money { get; }
//    int Delay { get; set; }
    TotalStats Stats { get; set; }
    PilotTactic Tactic { get; set; }
     
    //void 
    void UpgradeMaxHealthCoef();
    void UpgradeMaxShieldCoef();
    void UpgradeMaxSpeedCoef();
    void UpgradeMaxTurnSpeedCoef();

    [field: NonSerialized]
    event Action<IPilotParameters> OnLevelUp;
    void AddMoney(int money);

    LibraryPilotUpgradeType UpgradeRandomLevel(bool withMoney,bool withMsg);
    void UpgradeLevel(bool withMoney, LibraryPilotUpgradeType type, bool withMsg);
    void UpgradeLevelByType(LibraryPilotUpgradeType rnd, bool withMsg);
    bool HaveMoney(int cost);
    void RemoveMoney(int cost);
}