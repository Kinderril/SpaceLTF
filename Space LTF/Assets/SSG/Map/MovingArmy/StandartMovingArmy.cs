using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StandartMovingArmy : MovingArmy
{
    public StandartMovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback,float startPower,GalaxyEnemiesArmyController enemiesArmyController) 
        : base(startCell, destroyCallback, enemiesArmyController)
    {
        Power = startPower;
        //        var humanPlayer = MainController.Instance.MainPlayer;
        //        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);
        CurCell = startCell;
                _player = new PlayerAI($"{ Namings.Tag("SimpleMovingArmy")}:{MyExtensions.Random(3, 9999)}");
//        var armyPower = humanPower * Library.MOVING_ARMY_POWER_COEF;
//        var armyData = ArmyCreatorLibrary.GetArmy(startCell.ConfigOwner);
//        var army = ArmyCreator.CreateSimpleEnemyArmy(armyPower, armyData, _player);
//        _player.Army.SetArmy(army);
    }
    protected void CacheArmy()
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(StartConfig);
//        var player = new PlayerAI(name);
        var army = ArmyCreator.CreateSimpleEnemyArmy(Power, data, _player);
        if (army.Count == 0)
        {
            Debug.LogError("can't create standart moving army");
            return;
        }
        _player.Army.SetArmy(army);
    }
    public override string ShortDesc()
    {
        var coreTag = Namings.Tag("SimpleMovingArmy");
        return SubStr(coreTag);
    }
    public override GlobalMapCell FindCellToMove(HashSet<GlobalMapCell> posibleCells)
    {
        if (MyExtensions.IsTrue01(.5f))
        {
            return null;
        }

        if (StartConfig == ShipConfig.droid)
        {
            return null;
        }
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }
        if (CurCell == null)
        {
            Debug.LogError("Standart moving army have not cell");
            return null;
        }
        var ways = CurCell.GetCurrentPosibleWays();

        var waysToRandom = new List<GlobalMapCell>();
        foreach (var cell in CurCell.Sector.ListCells)
        {
            if (ways.Contains(cell.Data) && posibleCells.Contains(cell.Data) && !(cell.Data is GlobalMapNothing))
            {
                waysToRandom.Add(cell.Data);
            }
        }

        if (waysToRandom.Count == 0)
        {
            return null;
        }

        var rnd = waysToRandom.RandomElement();
        return rnd;
    }

    public override Player GetArmyToFight()
    {
        if (_player.Army.Army.Count == 0)
        {
            CacheArmy();
        }

        return _player;
    }

    public override MessageDialogData GetDialog(Action FightMovingArmy)
    {

        return GetDialogInner(FightMovingArmy);
    }

    protected MessageDialogData GetDialogInner(Action FightMovingArmy)
    {
        var myPlaer = MainController.Instance.MainPlayer;
        var status = myPlaer.ReputationData.GetStatus(StartConfig);
        bool isFriends = status == EReputationStatus.friend;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[StartConfig];
        var army = GetArmyToFight();
        string scoutsField = "";
        if (army.ScoutData != null)
        {
            BattlefildEventType? _eventType = CurCell.EventType;
            var scoutData = army.ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
            if (_eventType.HasValue)
            {
                scoutsField = Namings.Format(Namings.DialogTag("armySectorEvent"), Namings.BattleEvent(_eventType.Value)); ;
            }
            else
            {
                scoutsField = "";
            }
            for (int i = 0; i < scoutData.Count; i++)
            {
                var info = scoutData[i];
                scoutsField = $"{scoutsField}\n{info}\n";
            }
        }

        var reputation = Namings.Format(Namings.DialogTag("ArmyReputation"), rep);
        scoutsField = $"{scoutsField}\n{reputation}";
        ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), FightMovingArmy));
        if (isFriends)
        {
            masinMsg = Namings.Format(Namings.DialogTag("armyFrendly"), scoutsField); ;
            ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyAskHelp"), rep), null, DimlomatyOption));
        }
        else
        {
            int buyoutCost = (int)Power;
            var playersPower = ArmyCreator.CalcArmyPower(myPlaer.Army);

            if (myPlaer.MoneyData.HaveMoney(buyoutCost) && (StartConfig == ShipConfig.mercenary || StartConfig == ShipConfig.raiders))
            {
                ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyBuyOut"), buyoutCost), null,
                    () => BuyOutOption(buyoutCost)));
            }
            if (status == EReputationStatus.neutral)
            {
                masinMsg = Namings.Format(Namings.DialogTag("armyNeutral"), scoutsField);
            }
            else
            {
                if (playersPower < Power)
                {
                    masinMsg = Namings.Format(Namings.DialogTag("armyStronger"), scoutsField);
                }
                else
                {
                    masinMsg = Namings.Format(Namings.DialogTag("armyShallFight"), scoutsField);
                }
            }
        }


        if (isFriends || status == EReputationStatus.neutral)
        {
            ans.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        }
        else
        {
            ans.Add(new AnswerDialogData(
                Namings.Format(Namings.DialogTag("armyRun"), scoutsField),
                () =>
                {
                }, null, false, true));
        }

        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }
    private MessageDialogData BuyOutOption(int buyoutCost)
    {
        var player = MainController.Instance.MainPlayer;
        var ans = new List<AnswerDialogData>();
        player.MoneyData.RemoveMoney(buyoutCost);
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("armyBuyoutComplete"), buyoutCost), ans);
        return mesData;
    }

    private MessageDialogData DimlomatyOption()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[StartConfig];
        bool doDip = MyExtensions.IsTrue01((float)rep / PlayerReputationData.MAX_REP);
        if (doDip)
        {
            int a = 0;
            switch (StartConfig)
            {
                case ShipConfig.droid:
                case ShipConfig.raiders:
                case ShipConfig.mercenary:
                    a = 0;
                    break;
                case ShipConfig.federation:
                case ShipConfig.ocrons:
                case ShipConfig.krios:
                    a = 1;
                    break;
            }

            string helpInfo;
            if (a == 0)
            {
                var money = GlobalMapCell.AddMoney((int)(Power / 3), (int)(Power / 2));
                helpInfo = Namings.Format("They give you some credits {0}", money);
            }
            else
            {
                var player = MainController.Instance.MainPlayer;
                //                IItemInv item;
                bool canAdd;
                string itemName;
                int slot;
                if (MyExtensions.IsTrue01(.5f))
                {
                    var m = Library.CreatSimpleModul(2);
                    itemName = Namings.SimpleModulName(m.Type);
                    canAdd = player.Inventory.GetFreeSimpleSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddSimpleModul(m, slot);
                    }
                }
                else
                {
                    var w = Library.CreateDamageWeapon(false);
                    itemName = Namings.Weapon(w.WeaponType);
                    canAdd = player.Inventory.GetFreeWeaponSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddWeaponModul(w, slot);
                    }
                }

                if (canAdd)
                {
                    helpInfo = Namings.Format("They give {0}.", itemName);
                }
                else
                {
                    helpInfo = "But you have no free space";
                }
            }

            var pr = $"They help you as much as they can. {helpInfo}";
            var mesData = new MessageDialogData(pr, ans);
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData("They can't help you now", ans);
            return mesData;
        }
    }



    public override MessageDialogData MoverArmyLeaverEnd()
    {
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
        {
            var player = MainController.Instance.MainPlayer;
            player.ReputationData.WinBattleAgainst(StartConfig);
            var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power, StartConfig);
            return msg;
        }
        else
        {
            return null;
        }
    }
}
