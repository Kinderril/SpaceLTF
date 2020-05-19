using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class MovingArmy
{
    public GlobalMapCell CurCell;
    protected Player _player;
    private Action<MovingArmy> _destroyCallback;
    protected bool _noStepNext;

    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete = false;
    public int Id { get; private set; }
    public int Priority { get; private set; }
    public bool Destroyed { get; private set; }
    public float Power { get; protected set; }
    public GlobalMapCell PrevCell { get; set; }
    protected float _startPower;
    public ShipConfig StartConfig { get; private set; }

    private GalaxyEnemiesArmyController _armiesController;

    protected MovingArmy(GlobalMapCell startCell,
        Action<MovingArmy> destroyCallback, GalaxyEnemiesArmyController armiesController)
    {
        _armiesController = armiesController;
        _destroyCallback = destroyCallback;
        Id = Utils.GetId();
        if (startCell == null)
        {
            Debug.LogError("can't create moving army nowhere");
        }
        CurCell = startCell;
        StartConfig = startCell.ConfigOwner;
        startCell.CurMovingArmy = this;
//        Power = power;
        BattleController.Instance.OnBattleEndCallback += OnBattleEndCallback;
    }


    public void GetRewardsItems()
    {
        RewardPlayer();
    }
    public GlobalMapCell NextCell()
    {
        return _armiesController.GetNextTarget(this);
    }

    public abstract GlobalMapCell FindCellToMove( HashSet<GlobalMapCell> posibleCells);
    protected virtual void RewardPlayer()
    {
        var human = MainController.Instance.MainPlayer;
        human.ReputationData.WinBattleAgainst(StartConfig, 1f);
    }

    private void OnBattleEndCallback(Player human, Player ai, EndBattleType win)
    {
        switch (win)
        {
            case EndBattleType.win:
                Destroyed = true;
                if (ai == _player)
                {
                    _destroyCallback?.Invoke(this);
                    Dispose();

                }

                break;
            case EndBattleType.lose:
                Destroyed = true;
                break;
            case EndBattleType.runAway:
                _noStepNext = true;
                break;
        }
    }


    public void Dispose()
    {
        CurCell.CurMovingArmy = null;
        BattleController.Instance.OnBattleEndCallback -= OnBattleEndCallback;
    }

    public string Name()
    {
        return Namings.Format(Namings.Tag("MovingArmyName"), Namings.ShipConfig(StartConfig));
    }

    public abstract string ShortDesc();

    protected string SubStr(string coreTag)
    {
        var playersPower = MainController.Instance.MainPlayer.Army.GetPower();
        var thisPower = Power;
        var desc = PlayerArmy.ComparePowers(playersPower, thisPower);
        var status = MainController.Instance.MainPlayer.ReputationData.GetStatus(StartConfig);
        var txt = Namings.Format(coreTag, _player.Name, Namings.ShipConfig(StartConfig), Namings.Tag($"rep_{status.ToString()}"), desc);
        return txt;
    }

    public void UpdatePower(float power)
    {
        Power = power;
//        Debug.Log($"power updated to:{Power}");
    }

    public abstract Player GetArmyToFight();

    public abstract MessageDialogData GetDialog(Action FightMovingArmy);

    public abstract MessageDialogData MoverArmyLeaverEnd();

    public virtual void UpdateAllPowers(int visitedSectors, int step,int sectorSize)
    {

    }
}

