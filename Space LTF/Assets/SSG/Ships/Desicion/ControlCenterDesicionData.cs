using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ControlCenterDesicionData : IShipDesicion
{
    private ShipControlCenter _shipBase;
    private ActionType _lastAction = ActionType.moveToBase;
    private const float DOT_DIST_PARAMS = 8;

    public ControlCenterDesicionData(ShipControlCenter shipBase)
    {
        _shipBase = shipBase;
    }


    public ECommanderPriority1 CommanderPriority1 { get; }
    public ESideAttack SideAttack { get; }
    public EGlobalTactics GlobalTactics { get; }

    public ActionType CalcTask(out ShipBase ship)
    {
        ship = null;
        return ActionType.none;
    }

    public void Dispose()
    {
        
    }


    public void SetLastAction(ActionType actionType)
    {
        _lastAction = actionType;
    }

    public void Select(bool val)
    {
        
    }

    public void DrawUpdate()
    {

    }

    public string GetName()
    {
        return "Control";
    }

    public BaseAction CalcAction()
    {
        return new WaitEnemyTime(_shipBase,20f);
    }


    public void ChangePriority(ESideAttack SideAttack)
    {
        
    }

    public void ChangePriority(EGlobalTactics globalTactics)
    {

    }

    public void ChangePriority(ECommanderPriority1 CommanderPriority1)
    {

    }

    public event Action OnChagePriority;
}

