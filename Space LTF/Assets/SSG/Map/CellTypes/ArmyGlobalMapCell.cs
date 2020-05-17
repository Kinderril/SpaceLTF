using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnComeToDelegate(GlobalMapCell to, GlobalMapCell from);

[System.Serializable]
public abstract class ArmyGlobalMapCell : GlobalMapCell
{
    protected Player _enemyPlayer;

    protected int _power;
    public virtual int Power {
        get { return _power; }
        set { _power = value; }
    }

    protected int _additionalPower;


    [field: NonSerialized]
    public event OnComeToDelegate OnComeToCell;

    protected virtual Player GetArmy()
    {
        if (_enemyPlayer == null)
        {
            CacheArmy();
        }

        return _enemyPlayer;
    }

    protected virtual void CacheArmy()
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(ConfigOwner);
        var player = new PlayerAI(name);
        var army = ArmyCreator.CreateSimpleEnemyArmy(Power, data, player);
        player.Army.SetArmy(army);
        _enemyPlayer = player;
    }

    public override bool CanCellDestroy()
    {
        return true;
    }
    public override void ComeTo(GlobalMapCell from)
    {
        OnComeToCell?.Invoke(this, from);
    }
    protected ArmyGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind,
        SectorData sector)
        : base(id, Xind, Zind, sector, config)
    {
        // _armyType = type;
        Power = power;
        
    }

    public override void UpdatePowers(int visitedSectors, int startPower, int additionalPower)
    {
        _additionalPower = additionalPower;
        var nextPower = SectorData.CalcCellPower(visitedSectors, _sector.Size, startPower, _additionalPower);
        _enemyPlayer = null;
        // Debug.Log($"Army power sector updated prev:{_power}. next:{nextPower}");
        Power = nextPower;
    }

    public override Color Color()
    {
        return new Color(255f / 255f, 102f / 255f, 0f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }

    public override string Desc()
    {
        var army = Namings.Tag("Amry");
        if (_eventType.HasValue)
        {
            return $"{army}:{Namings.ShipConfig(ConfigOwner)}\nZone:{Namings.BattleEvent(_eventType.Value)}";
        }
        else
        {
            return $"{army}:{Namings.ShipConfig(ConfigOwner)}";
        }
    }

    public override void Take()
    {
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(), false, true, _eventType);
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
        {
            var player = MainController.Instance.MainPlayer;
            player.ReputationData.WinBattleAgainst(ConfigOwner);
            var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power, ConfigOwner);
            return msg;
        }
        else
        {
            return null;
        }
    }

    public ShipConfig GetConfig()
    {
        return ConfigOwner;
    }

    public string PowerDesc()
    {
        return PlayerArmy.PowerDesc(_sector, Power, _additionalPower);
    }
}