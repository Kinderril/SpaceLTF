using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IPilotParameters
{
    PilotTcatic Tactic { get; }
    bool CanUpgradeByLevel(int curValue);
    bool CanUpgradeAnyParameter(int phantomMoney);
    void Init(PilotTcatic taTcatic);

    int HealthLevel { get; }
    int ShieldLevel { get; }
    int SpeedLevel { get; }
    int TurnSpeedLevel { get; }

    float PercentLevel { get; }
    //HashSet<LibraryPilotUpgradeType> UnCheckedLevelUp { get; }

    int CurLevel { get; }
    int Money { get; }
    int Delay { get; set; }
    TotalStats Stats { get; set; }
     
    //void 
    void UpgradeMaxHealthCoef();
    void UpgradeMaxShieldCoef();
    void UpgradeMaxSpeedCoef();
    void UpgradeMaxTurnSpeedCoef();
    void ChangeTactic();
    //void CheckWannaLvlUp();
    //void SecureLevelUp(LibraryPilotUpgradeType upgradeType);

    [field: NonSerialized]
    event Action<IPilotParameters,PilotTcatic> OnTacticChange;
    [field: NonSerialized]
    event Action<IPilotParameters> OnLevelUp;
    void AddMoney(int money);

    void UpgradeRandomLevel(bool withMoney);
    void UpgradeLevel(bool withMoney, LibraryPilotUpgradeType type);
    void UpgradeLevelByType(LibraryPilotUpgradeType rnd);
    bool HaveMoney(int cost);
    void RemoveMoney(int cost);
}