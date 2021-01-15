using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class MovingArmy
{
    public GlobalMapCell CurCell { get; protected set; }
    protected Player _player;
    private Action<MovingArmy> _destroyCallback;
    protected Func<Action,MessageDialogData> _startDialog = null;
    protected Func<MessageDialogData> _endDialog = null;
    public bool IsAllies { get; private set; }

    //    [field: NonSerialized]
    //    public event Action<MovingArmy> OnDestroyed;
    protected bool _noStepNext;
    protected float _collectedPower = 0f;
    protected float _additionalPower = 0f;

    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete = false;
    public int Id { get; private set; }
    public int Wins { get; private set; }
    public int Priority { get; private set; }
    public bool Destroyed { get; private set; }
    public float Power { get; protected set; }
    public float Reward { get; protected set; }
    public GlobalMapCell PrevCell { get; set; }
    protected float _startPower;
    public ShipConfig StartConfig { get; private set; }
    //    public int LifeTime { get; private set; } = 0;

    [field: NonSerialized] private bool _subscribeComplete = false;
    protected GalaxyEnemiesArmyController _armiesController;

    protected MovingArmy(GlobalMapCell startCell,
        Action<MovingArmy> destroyCallback, GalaxyEnemiesArmyController armiesController,bool isAllies)
    {
        IsAllies = isAllies;
        _armiesController = armiesController;
        _destroyCallback = destroyCallback;
        Id = Utils.GetId();
        if (startCell == null)
        {
            Debug.LogError("can't create moving army nowhere");
        }
        CurCell = startCell;
        StartConfig = startCell.ConfigOwner;
        startCell.CurMovingArmy.ArmyCome(this);
        //        Power = power;
        Subscribe();
    }

    public void TakeReward(Player player)
    {
        player.MoneyData.AddMoney((int)Reward);
        Reward = 0;
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

    public void SetStartDialog(Func<Action,MessageDialogData> startDialog)
    {
        _startDialog = startDialog;
    } 
    public void SetEndDialog(Func<MessageDialogData> endDialog)
    {
        _endDialog = endDialog;
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
        CurCell.CurMovingArmy.ArmyRemove(this);
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

    public abstract MessageDialogData GetDialog(Action FightMovingArmy,MessageDialogData nextDialog);

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

    public void SetCurCell(GlobalMapCell cellToGo)
    {
        CurCell = cellToGo;

    }

    public void AddWin(float enemiesPower)
    {
        Wins++;
        Reward = enemiesPower * 0.85f;
        //Addreward

    }

    public void Destroy()
    {
        CurCell.CurMovingArmy.ArmyRemove(this);
    }

    public virtual void AfterStepAction()
    {
        

    }
}

