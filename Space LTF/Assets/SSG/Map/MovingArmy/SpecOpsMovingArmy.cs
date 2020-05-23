using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SpecOpsMovingArmy : MovingArmy
{
    public SpecOpsMovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback, GalaxyEnemiesArmyController enemiesArmyController) 
        : base(startCell, destroyCallback, enemiesArmyController)
    {
        var humanPlayer = MainController.Instance.MainPlayer;
        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);
        _player = new PlayerAIMovingArmy($"{ Namings.Tag("Destroyed")}:{MyExtensions.Random(3, 9999)}");
        var armyPower = humanPower * Library.MOVING_ARMY_POWER_COEF;
        Power = armyPower;
        var armyData = ArmyCreatorLibrary.GetArmy(startCell.ConfigOwner);
        var army = ArmyCreator.CreateSimpleEnemyArmy(armyPower, armyData, _player);
        _player.Army.SetArmy(army);
        _startPower = armyPower;
    }
    public override string ShortDesc()
    {
        var coreTag = Namings.Tag("MovingArmy");
        return SubStr(coreTag);
    }

    protected override void RewardPlayer()
    {
        var human = MainController.Instance.MainPlayer;
        var baseRep = Library.BATTLE_REPUTATION_AFTER_FIGHT * 2;
        human.ReputationData.WinBattleAgainst(_player.Army.BaseShipConfig, 2f);
    }

    public override Player GetArmyToFight()
    {
        return _player;
    }

    public override MessageDialogData MoverArmyLeaverEnd()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyGerReward"), GetRewardsItems,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyWin"), ans);
        return mesData;

    }

    public override MessageDialogData GetDialog(Action FightMovingArmy)
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), FightMovingArmy,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans);
        return mesData;
    }       


    public override GlobalMapCell FindCellToMove(HashSet<GlobalMapCell> posibleCells)
    {
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }

        //        if (playersCell == CurCell)
        //        {
        //            return null;
        //        }
        var player = MainController.Instance.MainPlayer;
        var playersCell = player.MapData.CurrentCell;
        var ways = CurCell.GetCurrentPosibleWays();
        var ststus = player.ReputationData.GetStatus(_player.Army.BaseShipConfig);
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
                if (posibleCells.Contains(selectedWay))
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
                if (posibleCells.Contains(cellToGo))
                    return cellToGo;
                break;
        }

        return null;

    }

}
