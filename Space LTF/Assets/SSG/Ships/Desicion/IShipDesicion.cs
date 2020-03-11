using System;



public interface IShipDesicion
{
    ECommanderPriority1 CommanderPriority1 { get; }
    ESideAttack SideAttack { get; }
    EGlobalTactics GlobalTactics { get; }
    ActionType CalcTask(out ShipBase ship);
    void Dispose();
    void SetLastAction(ActionType actionType);
    void Select(bool val);
    void DrawUpdate();
    string GetName();
    BaseAction CalcAction();
    void ChangePriority(ESideAttack SideAttack);
    void ChangePriority(EGlobalTactics globalTactics);
    void ChangePriority(ECommanderPriority1 CommanderPriority1);
    bool HaveClosestDamagedFriend(out ShipBase ship);
    bool HaveEnemyInDangerZoneDefenceBase(out ShipBase ship);

    event Action OnChagePriority;

}


