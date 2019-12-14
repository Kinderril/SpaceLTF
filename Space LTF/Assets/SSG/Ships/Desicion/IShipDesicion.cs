using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public interface IShipDesicion
{
    ActionType CalcTask(out ShipBase ship);
    void Dispose();
    void SetLastAction(ActionType actionType);
    void Select(bool val);
    void DrawUpdate();
    string GetName();
    BaseAction CalcAction();                      
    void ChangePriority(ESideAttack SideAttack);
    void ChangePriority(EBaseDefence BaseDefence);
    void ChangePriority(ECommanderPriority1 CommanderPriority1);

}


