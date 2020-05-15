using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class MovingArmy
{
    public GlobalMapCell CurCell;
    public Player _player;
    private Action<MovingArmy> _destroyCallback;
    protected bool _noStepNext;

    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete = false;
    public int Id { get; private set; }
    public int Priority { get; private set; }
    public float Power { get; private set; }
    public GlobalMapCell PrevCell { get; set; }

    public MovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback)
    {
        var humanPlayer = MainController.Instance.MainPlayer;
        _destroyCallback = destroyCallback;
        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);
        Id = Utils.GetId();
        CurCell = startCell;
        _player = new PlayerAIMovingArmy($"{ Namings.Tag("Destroyed")}:{MyExtensions.Random(3, 9999)}");
        var armyPower = humanPower * Library.MOVING_ARMY_POWER_COEF;
        var armyData = ArmyCreatorLibrary.GetArmy(startCell.ConfigOwner);
        var army = ArmyCreator.CreateSimpleEnemyArmy(armyPower, armyData, _player);
        _player.Army.SetArmy(army);
        startCell.CurMovingArmy = this;
        BattleController.Instance.OnBattleEndCallback += OnBattleEndCallback;


    }

    public void GetRewardsItems()
    {
        RewardPlayer();
    }

    public abstract GlobalMapCell FindCellToMove(GlobalMapCell playersCell, HashSet<GlobalMapCell> posibleCells);
    private void RewardPlayer()
    {
        var human = MainController.Instance.MainPlayer;
        var baseRep = Library.BATTLE_REPUTATION_AFTER_FIGHT * 2;
        human.ReputationData.WinBattleAgainst(_player.Army.BaseShipConfig, 2f);
    }

    private void OnBattleEndCallback(Player human, Player ai, EndBattleType win)
    {
        switch (win)
        {
            case EndBattleType.win:
                if (ai == _player)
                {
                    _destroyCallback?.Invoke(this);
                    Dispose();

                }

                break;
            case EndBattleType.lose:
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
        return Namings.Format(Namings.Tag("MovingArmyName"), Namings.ShipConfig(_player.Army.BaseShipConfig));
    }

    public string ShortDesc()
    {
        var playersPower = MainController.Instance.MainPlayer.Army.GetPower();
        var thisPower = _player.Army.GetPower();
        var desc = PlayerArmy.ComparePowers(playersPower, thisPower);
        var status = MainController.Instance.MainPlayer.ReputationData.GetStatus(_player.Army.BaseShipConfig);
        var txt = Namings.Format(Namings.Tag("MovingArmy"), _player.Name, Namings.ShipConfig(_player.Army.BaseShipConfig), Namings.Tag($"rep_{status.ToString()}"), desc);
        return txt;
    }

    public void UpdatePower(float power)
    {
        Power = power;
    }
}

