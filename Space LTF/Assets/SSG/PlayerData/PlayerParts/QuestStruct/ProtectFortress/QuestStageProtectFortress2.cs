﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageProtectFortress2 : QuestStage//  , ISerializable
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageProtectFortress2()    
        :base(QuestsLib.QUEST_START_PROTECT_FORTRESS2)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var posibleSectors = GetSectors(player, 0, 4,1);
        if (posibleSectors.Count < 1)
        {
            return false;
        }
        cell1 = FindAndMarkCell(posibleSectors.RandomElement(), Dialog) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }

        return true;

    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc)
    {
        var player = MainController.Instance.MainPlayer.MapData;
        var cells = posibleSector.ListCells.Where(x =>x.indX != player.CurrentCell.indX && x.Data  != null 
                                                       && x.Data is FreeActionGlobalMapCell 
                                                       && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        var cell = cells.RandomElement().Data as FreeActionGlobalMapCell;
        if (cell == null)
        {
            return null;
        }

        cell.SetQuestData(dialogFunc);
        return cell;

    }

    private MessageDialogData Dialog()
    {
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        string str = Namings.Tag("questprotectFortressDialogStart");
        ans.Add(new AnswerDialogData(Namings.Tag("Protect"),
            () =>
            {
                cell1.SetQuestData(null);
                TextChangeEvent();
                _playerQuest.QuestIdComplete(QuestsLib.QUEST_START_PROTECT_FORTRESS2);
                MainController.Instance.PreBattle(MainController.Instance.MainPlayer, PlayerToDefeat(),false,false);
            },
            null,true,false));
        ans.Add(new AnswerDialogData(Namings.Tag("leave")));
        var msg = new MessageDialogData(str,ans,true);
        return msg;
    }

    public Player PlayerToDefeat()
    {
        var player = MainController.Instance.MainPlayer;
        var power = player.Army.GetPower() * 1.2f;
        var config = player.ReputationData.WorstFaction(ShipConfig.droid);
        var withBase = power > 25;
        var playerEnemy = new PlayerAIWithBattleEvent("FortressProtect",true,EBattleType.baseDefence);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6, ArmyCreatorLibrary.GetArmy(config), withBase,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return player;
    }

    protected override void StageDispose()
    {

    }

    public override bool CloseWindowOnClick => true;
    public override void OnClick()
    {
        TryNavigateToCell(GetCurCellTarget());
    }

    public GlobalMapCell GetCurCellTarget()
    {
    
            return cell1;

    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("questNameProtectStage")}";
    }
}
