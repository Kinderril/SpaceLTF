using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Time = UnityEngine.Time;

public class DestroyShipByTimeBattle : BattleTypeEvent
{
    private float _targetTime;
    private float period = 35f;
    private bool _isShipDead = false;
    private bool _failed = false;
    private bool _runAwayComplete = false;
    private ShipBase _shipToKill;
    private string _mag;

    public override bool HaveActiveTime => true;
    public override void Init(BattleController battle)
    {
        base.Init(battle);
        _targetTime = Time.time + period;
        var ships = battle.RedCommander.Ships;
        _shipToKill = ships.Values.FirstOrDefault(x => x.ShipInventory.Marked);
        if (_shipToKill == null)
        {
            _shipToKill = ships.Values.ToList().RandomElement();
        }
        _shipToKill.OnDeath += OnDeath;
        _mag = Namings.Tag("remainTime");
    }


    protected override void SubUpdate()
    {
        if (!_runAwayComplete)
        {
            var remainTime = _targetTime - Time.time;
            OnTimeEndAction(remainTime, false, _mag);
            if (remainTime < 0f)
            {
                _runAwayComplete = true;
                OnTimeEndAction(remainTime, true, _mag);
                _shipToKill.Commander.ShipRunAway(_shipToKill);
            }
        }
    }

    private void OnDeath(ShipBase obj)
    {
        var remainTime = _targetTime - Time.time;
        if (Time.time > _targetTime)
        {
            _failed = true;
        }
        _isShipDead = true;
        OnTimeEndAction(remainTime,false, _mag);
    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, List<StartShipPilotData> paramsOfShips, Player player)
    {
        var markedShip = paramsOfShips.FirstOrDefault(x => x.Ship.ShipType == ShipType.Base);
        if (markedShip == null)
        {
            markedShip = paramsOfShips.RandomElement();
        }

        markedShip.Ship.Marked = true;
        return paramsOfShips;
    }


    public override EndBattleType WinCondition(EndBattleType prevResult)
    {
        if (prevResult == EndBattleType.win)
        {
            if (_failed)
            {
                return prevResult;
            }
            else
            {
                if (_isShipDead)
                {
                    return EndBattleType.winFull;
                }
            }
        }
        return prevResult;
    }
}
