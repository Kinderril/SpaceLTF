﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StandartMovingArmy : MovingArmy
{
    private float _powerPerTurn;
    private bool canAskHelp = true;
    public StandartMovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback
        ,float startPower,GalaxyEnemiesArmyController enemiesArmyController, float powerPerTurn) 
        : base(startCell, destroyCallback, enemiesArmyController,false)
    {
        _powerPerTurn = powerPerTurn;
        _startPower = startPower;
        SubUpdatePower();
        //        var humanPlayer = MainController.Instance.MainPlayer;
        //        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);
        CurCell = startCell;
        var posibleBattleEvent = !startCell.Sector.IsStart;
        var name = $"{Namings.Tag("SimpleMovingArmy")}:{MyExtensions.Random(3, 9999)}";
        _player = new PlayerAIWithBattleEvent(name, posibleBattleEvent);
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
    public override void UpdateAllPowersAdditional( float additionalPower)
    {
        _additionalPower = additionalPower;
        SubUpdatePower();
    }
    public override void UpdateAllPowersCollected(float collectedPower)
    {
        _collectedPower += collectedPower;
        SubUpdatePower();
    }

    private void SubUpdatePower()
    {
        var battleTypePowerCoef = 1f;
        var playerAi = _player as IPlayerAIWithBattleEvent;
        if (playerAi != null)
        {
            switch (playerAi.BattleTypeData.EBattleType)
            {
                case EBattleType.defenceWaves:
                    battleTypePowerCoef = 1.15f;
                    break;
                case EBattleType.defenceOfShip:
                    battleTypePowerCoef = 1.1f;
                    break;
                case EBattleType.baseDefence:
                    battleTypePowerCoef = 1f  + DefenceBaseEvent.POWER_ICN;
                    break;
            }
        }
        Power =  (_startPower + _collectedPower + _additionalPower) * battleTypePowerCoef;
    }


    public override Player GetArmyToFight()
    {
        if (_player.Army == null || _player.Army.Army == null || _player.Army.Army.Count == 0)
        {
            CacheArmy();
        }

        return _player;
    }

    public override MessageDialogData GetDialog(Action FightMovingArmy, MessageDialogData nextDialog)
    {

        return GetDialogInner(FightMovingArmy, nextDialog);
    }

    protected MessageDialogData GetDialogInner(Action FightMovingArmy, MessageDialogData nextDialog)
    {
        var myPlaer = MainController.Instance.MainPlayer;
        var status = myPlaer.ReputationData.GetStatus(StartConfig);
        bool isAllies = myPlaer.ReputationData.IsAllies(StartConfig);
        bool isFriends = status == EReputationStatus.friend;

        var ans = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[StartConfig];
        var army = GetArmyToFight();
        string scoutsField = "";
        if (army.ScoutData != null)
        {
            EBattlefildEventType? _eventType = CurCell.EventType;
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
                scoutsField = $"{scoutsField}\n{info}";
            }
        }

        var reputation = Namings.Format(Namings.DialogTag("ArmyReputation"), rep);
        bool haveNextDialog = nextDialog != null;
        var leaveStr = haveNextDialog? Namings.Tag("leaveWithNext") : Namings.Tag("leave");
        scoutsField = $"{scoutsField}\n{reputation}";
        if (isAllies)
        {
            ans.Add(new AnswerDialogData(leaveStr, null,()=> nextDialog));
            if (canAskHelp)
            {
                canAskHelp = false;
                ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyAskHelp"),
                    rep, Library.ASK_HELP_REPUTATION), null, () => DimlomatyOption(nextDialog)));
            }
        }
        else
        {
            ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), FightMovingArmy));
            if (isFriends)
            {
                ans.Add(new AnswerDialogData(leaveStr, null, () => nextDialog));
            }
            else
            {
                int buyoutCost = (int)Power;

                if (myPlaer.MoneyData.HaveMoney(buyoutCost) && (StartConfig == ShipConfig.mercenary || StartConfig == ShipConfig.raiders))
                {
                    ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyBuyOut"), buyoutCost), null,
                        () => BuyOutOption(buyoutCost)));
                }
                ans.Add(new AnswerDialogData(
                    Namings.Format(Namings.DialogTag("armyRun"), scoutsField),
                    () =>
                    {
                    }, null, false, true));
            }


        }

        var masinMsg = GetBaseFightStartMessage();
        masinMsg = $"{masinMsg}\n{scoutsField}";

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

    private string GetBaseFightStartMessage()
    {
        var myPlaer = MainController.Instance.MainPlayer;
        var status = myPlaer.ReputationData.GetStatus(StartConfig);
        var playersPower = ArmyCreator.CalcArmyPower(myPlaer.Army);
        string masinMsg;
        var playerAi = _player as IPlayerAIWithBattleEvent;
        EBattleType battleType = EBattleType.standart;
        if (playerAi != null)
        {
            battleType = playerAi.BattleTypeData.EBattleType;
        }
        switch (battleType)
        {
            case EBattleType.defenceWaves:
                masinMsg = Namings.Tag("defenceWavesStart");
                break;
            case EBattleType.destroyShipPeriod:
                masinMsg = Namings.Tag("destroyShipPeriodStart");
                break;
            case EBattleType.defenceOfShip:
                masinMsg = Namings.Tag("defenceOfShipStart");
                break; 
            case EBattleType.massiveFight:
                masinMsg = Namings.Tag("massiveFightStart");
                break;
            case EBattleType.baseDefence:
                masinMsg = Namings.Tag("baseDefenceStart");
                break;
            case EBattleType.standart:
            default:
                if (status == EReputationStatus.neutral)
                {
                    masinMsg = Namings.DialogTag("armyNeutral");
                }
                else
                {
                    if (playersPower < Power)
                    {
                        masinMsg = Namings.DialogTag("armyStronger");
                    }
                    else
                    {
                        masinMsg = Namings.DialogTag("armyShallFight");
                    }
                }
                break;
        }

        return masinMsg;
    }

    private MessageDialogData DimlomatyOption(MessageDialogData nextDialog)
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok"),null,()=> nextDialog));
//        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[StartConfig];
//        bool doDip = MyExtensions.IsTrue01((float)rep / PlayerReputationData.MAX_REP);
//        if (doDip)
//        {
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
                var money = GlobalMapCell.AddMoney((int)(Power / 3), (int)(Power / 2)) * _player.SafeLinks.CreditsCoef;
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
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(StartConfig, Library.ASK_HELP_REPUTATION);
            var mesData = new MessageDialogData(pr, ans);
            return mesData;
//        }
//        else
//        {
//            var mesData = new MessageDialogData("They can't help you now", ans);
//            return mesData;
//        }
    }



    public override MessageDialogData MoverArmyLeaverEnd()
    {
        var playerAI = _player as IPlayerAIWithBattleEvent;
        EBattleType type = EBattleType.standart;
        if (playerAI != null)
        {
            type = playerAI.BattleTypeData.EBattleType;
        }

        if (type == EBattleType.standart)
        {
            if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
            {
                var player = MainController.Instance.MainPlayer;
                player.ReputationData.WinBattleAgainst(StartConfig);
                var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power, StartConfig, player);
                return msg;
            }
        }
        else
        {
            var fullWin = MainController.Instance.Statistics.LastBattle == EndBattleType.winFull;
            return BattleEventsDialogsLib.GetDialog(type, fullWin, Power,MainController.Instance.MainPlayer);
        }

        return null;
    }
}
