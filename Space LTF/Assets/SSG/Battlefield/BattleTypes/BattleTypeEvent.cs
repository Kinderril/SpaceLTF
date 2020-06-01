using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleTypeEvent
{
    protected BattleController _battle;
    protected bool _inited = false;
    public event Action<float,bool, string> OnTimeLeft;
    private string _msg;
    public abstract bool HaveActiveTime { get; }

    protected BattleTypeEvent(string msg)
    {
        _msg = msg;
    }

    public string GetMsg()
    {
        return _msg;
    }


    public virtual void Init(BattleController battle)
    {
        _battle = battle;
        _inited = true;
    }

    protected void OnTimeEndAction(float time, bool isLast,string msg)
    {
        OnTimeLeft?.Invoke(time,isLast, msg);

    }

    public void ManualUpdate()
    {
        if (!_inited)
        {
            return;
        }
        SubUpdate();
    }

    protected virtual void SubUpdate()
    {

    }

    public virtual EndBattleType WinCondition(EndBattleType prevResult)
    {
        return prevResult;
    }

    public virtual List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, List<StartShipPilotData> paramsOfShips,Player player)
    {
        return paramsOfShips;
    }

    public virtual bool CanEnd()
    {
        return true;

    }

}
