using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class MovingArmy
{
    public GlobalMapCell CurCell;
    public Player _player;
    private Action<MovingArmy> _destroyCallback;
    private bool _noStepNext;
    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete = false;

    public MovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback)
    {
        var humanPlayer = MainController.Instance.MainPlayer;
        _destroyCallback = destroyCallback;
        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);

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

    public GlobalMapCell FindCellToMove(GlobalMapCell playersCell)
    {
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }

        if (playersCell == CurCell)
        {
            return null;
        }
        var ways = CurCell.GetCurrentPosibleWays();
        var ststus = MainController.Instance.MainPlayer.ReputationData.GetStatus(_player.Army.BaseShipConfig);
        var posibleWays = ways.Where(x => !(x is GlobalMapNothing) && x.CurMovingArmy == null).ToList();
        if (posibleWays.Count == 0)
        {
            return null;
        }

        switch (ststus)
        {
            default:
            case EReputationStatus.friend:
            case EReputationStatus.neutral:
                var selectedWay = posibleWays.RandomElement();
                return selectedWay;
                break;

            case EReputationStatus.negative:
            case EReputationStatus.enemy:
                int minDelta = 999;
                GlobalMapCell cellToGo = posibleWays[0];
                foreach (var way in posibleWays)
                {
                    var a = Mathf.Abs(way.indX - playersCell.indX);
                    var b = Mathf.Abs(way.indZ - playersCell.indZ);
                    var c = a + b;
                    if (c < minDelta)
                    {
                        minDelta = c;
                        cellToGo = way;
                    }
                }
                return cellToGo;
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
}

