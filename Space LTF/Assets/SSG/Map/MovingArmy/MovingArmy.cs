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

//    [field: NonSerialized]
//    public event Action<MovingArmy> OnDestroyed;
    protected bool _noStepNext;
    protected float _collectedPower = 0f;
    protected float _additionalPower = 0f;

    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete = false;
    public int Id { get; private set; }
    public int Priority { get; private set; }
    public bool Destroyed { get; private set; }
    public float Power { get; protected set; }
    public GlobalMapCell PrevCell { get; set; }
    protected float _startPower;
    public ShipConfig StartConfig { get; private set; }
    //    public int LifeTime { get; private set; } = 0;

    [field: NonSerialized] private bool _subscribeComplete = false;
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
        Subscribe();
    }

    public void Subscribe()
    {
        if (_subscribeComplete)
        {
            return;
        }
        _subscribeComplete = true;
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
            case EndBattleType.winFull:
                if (ai == _player)
                {
                    Destroyed = true;
                    _destroyCallback?.Invoke(this);
                    Dispose();

                }

                break;
            case EndBattleType.lose:
                if (ai == _player)
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

//    public void SetPower(float power)
//    {
//        Power = power;
////        Debug.Log($"power updated to:{Power}");
//    }


//    public void DoMove()
//    {
//        LifeTime++;
//    }
    public abstract Player GetArmyToFight();

    public abstract MessageDialogData GetDialog(Action FightMovingArmy);

    public abstract MessageDialogData MoverArmyLeaverEnd();

    public virtual void UpdateAllPowersAdditional(float additionalPower)
    {
                
    }
    public virtual void UpdateAllPowersCollected(float collectedPower)
    {
                
    }

    public void AfterLoadCheck()
    {
        Subscribe();
    }
}

