using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SpecOpsMovingArmy : MovingArmy
{
    private bool _nextCellCrash;
    public SpecOpsMovingArmy(SectorCellContainer startCell, Action<MovingArmy> destroyCallback, 
        GalaxyEnemiesArmyController enemiesArmyController, bool isAllies,int power = -1) 
        : base(startCell, destroyCallback, enemiesArmyController,isAllies)
    {
        var humanPlayer = MainController.Instance.MainPlayer;
        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);
        _player = new PlayerAIMovingArmy($"{ Namings.Tag("Destroyed")}:{MyExtensions.Random(3, 9999)}");
        int armyPower;
        if (power < 0)
        {
            var rnd = MyExtensions.GreateRandom(humanPower * Library.MOVING_ARMY_POWER_COEF);
            armyPower = (int)(rnd);
        }
        else
        {
            armyPower = power;
        }
        Power = armyPower;
        var armyData = ArmyCreatorLibrary.GetArmy(startCell.Data.ConfigOwner);
        var army = ArmyCreator.CreateSimpleEnemyArmy(armyPower, armyData, _player);
        _player.Army.SetArmy(army);
        _startPower = armyPower;
        _noStepNext = true;

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
        if (_endDialog != null)
        {
            return _endDialog();
        }
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyGerReward"), GetRewardsItems,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyWin"), ans);
        return mesData;

    }

    public override MessageDialogData GetDialog(Action FightMovingArmy, MessageDialogData nextDialog)
    {
        if (_startDialog != null)
        {
            return _startDialog(FightMovingArmy);
        }

        if (IsAllies)
        {
            var ans1 = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Ok"), null,  null,false,false),
            };
            if (Reward > 0)
            {
                ans1.Add(new AnswerDialogData(Namings.Format(Namings.Tag("TakeReward"),Reward.ToString("0")), () => TakeReward(MainController.Instance.MainPlayer), null, false, false));
            }

            var costImprove = (int)(Power*1.5f);
            var improve = new AnswerDialogData(Namings.Format(Namings.Tag("ImproveFleet"),costImprove), null, ()=>FleetImproved(costImprove), false, false);
            ans1.Add(improve);

            var mesData1 = new MessageDialogData(Namings.Format(Namings.DialogTag("MovingArmyAllies"), Wins,Reward), ans1);
            return mesData1;
        }
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), FightMovingArmy,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans);
        return mesData;
    }

    private MessageDialogData FleetImproved(int costImprove)
    {
        var player = MainController.Instance.MainPlayer;
        var ans1 = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Ok"), null,  null,false,false),
        };
        if (player.MoneyData.HaveMoney(costImprove))
        {
            player.MoneyData.RemoveMoney(costImprove);
            var myPower = player.Army.GetPower();
            var rnd = (int)MyExtensions.GreateRandom(myPower * 0.1f);
            Power += rnd;
            var mesData = new MessageDialogData(Namings.Tag("myFleetImproved"), ans1);
            return mesData;

        }
        else
        {
            var mesData = new MessageDialogData(Namings.DialogTag("NotEnoughtMoney"), ans1);
            return mesData;
        }

    }

    private GlobalMapCell SubFindCellToMove(HashSet<GlobalMapCell> posibleCells)
    {
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }

        var player = MainController.Instance.MainPlayer;
        var playersCell = player.MapData.CurrentCell;
        var ways = CurCell.GetCurrentPosibleWays();
        var ststus = player.ReputationData.GetStatus(_player.Army.BaseShipConfig);
        var notNoghing = ways.Where(x => !(x.Data is GlobalMapNothing));
        List<SectorCellContainer> posibleWays;
        if (IsAllies)
        {
            bool wannaAttack = MyExtensions.IsTrue01(0.5f);
#if UNITY_EDITOR
            //            wannaAttack = true;
#endif
            if (wannaAttack)
            {
                posibleWays = notNoghing.Where(x => x.Data.CurMovingArmy.HaveEnemiesAmry()).ToList();
                if (posibleWays.Count == 0)
                {
                    posibleWays = notNoghing.Where(x => !x.Data.CurMovingArmy.HaveAlliesAmry()).ToList();
                }
            }
            else
            {
                posibleWays = notNoghing.Where(x => !x.Data.CurMovingArmy.HaveAlliesAmry()).ToList();
            }
            var selectedWay1 = posibleWays.RandomElement();
            return selectedWay1.Data;


        }
        else
        {
            posibleWays = notNoghing.Where(x => !x.CurMovingArmy.HaveEnemiesAmry()).ToList();
        }

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
                if (posibleCells.Contains(selectedWay.Data))
                    return selectedWay.Data;
                break;

            case EReputationStatus.negative:
            case EReputationStatus.enemy:
                foreach (var way in posibleWays)
                {
                    if (way == playersCell.Container)
                    {
                        if (posibleCells.Contains(way.Data))
                            return way.Data;
                    }
                }

                foreach (var way in posibleWays)
                {
                    if (way.Data.Sector.IsMy)
                    {
                        if (posibleCells.Contains(way.Data))
                            return way.Data;
                    }
                }


                int minDelta = 999;
                var cellToGo = posibleWays[0];
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
                if (posibleCells.Contains(cellToGo.Data))
                    return cellToGo.Data;
                break;
        }

        return null;

    }

    public override void AfterStepAction(SectorCellContainer playerTrg)
    {
        if (_nextCellCrash)
        {
            _nextCellCrash = false;
            if (playerTrg != CurCell)
            {
                var newData = new FreeActionGlobalMapCell((int)Power, StartConfig, Utils.GetId(),
                    CurCell.indX, CurCell.indZ, CurCell.Data.Sector, _armiesController, 0, false);
                newData.Complete();
                CurCell.SetData(newData);
            }
        }
    }

    public override GlobalMapCell FindCellToMove(HashSet<GlobalMapCell> posibleCells)
    {
        if (CurCell.Data.Sector.IsMy && !IsAllies)
        {
            if (CurCell.Data is ShopGlobalMapCell || CurCell.Data is RepairStationGlobalCell)
            {
                _nextCellCrash = true;
                return null;
            }
        }

        _nextCellCrash = false;
        var trg = SubFindCellToMove(posibleCells);
        return trg;
    }

}
