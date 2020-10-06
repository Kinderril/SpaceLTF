using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnComeToDelegate(GlobalMapCell to, GlobalMapCell from);

[System.Serializable]
public abstract class ArmyGlobalMapCell : GlobalMapCell
{
    protected Player _enemyPlayer;

//    protected int _power;
    public virtual int Power
    {
        get;
        private set;
    }

    protected float _additionalPower = 0f;
    protected float _collectedPower = 0f;
    protected int _startPower;
    protected float _powerCoef = 1.001f;


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
        _startPower = power;
        SubUpdatePower();


    }

    public override void UpdateAdditionalPower(int additionalPower)
    {
        _additionalPower = additionalPower;
        _enemyPlayer = null;
        SubUpdatePower();
    }

    protected void SubUpdatePower()
    {
        Power = (int)((_startPower + _collectedPower + _additionalPower) * _powerCoef);
    }

    public override void UpdateCollectedPower(float powerDelta)
    {
        _collectedPower += powerDelta;
        SubUpdatePower();
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
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(), false, true);
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
        {
            var player = MainController.Instance.MainPlayer;
            player.ReputationData.WinBattleAgainst(ConfigOwner);
            var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power, ConfigOwner,player);
            return msg;
        }
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.winFull)
        {
            return null;
        }
        return null;
    }

    public ShipConfig GetConfig()
    {
        return ConfigOwner;
    }

    public string PowerDesc()
    {
        return PlayerArmy.PowerDesc(_sector, Power);
    }
}