using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageStartDeliver : QuestStage//  , ISerializable
{

    private FreeActionGlobalMapCell cell1 = null;
//    private FreeActionGlobalMapCell cell2 = null;

    private bool cell1Complete;
//    private bool cell2Complete;

    public QuestStageStartDeliver()    
        :base(QuestsLib.QUEST_START_DELIVER1)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var posibleSectors = GetSectors(player, 0, 2,1);
        if (posibleSectors.Count < 1)
        {
            return false;
        }


        cell1 = FindAndMarkCell(posibleSectors.RandomElement(), Dialog) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
//        cell2  = FindAndMarkCell(posibleSectors[1], GetDialog2) as FreeActionGlobalMapCell;
//        if (cell2 == null)
//        {
//            return false;
//        }

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
        string str = Namings.Tag("questStartDeliverDialog");
        ans.Add(new AnswerDialogData(Namings.Tag("Ok"),
            () =>
            {
                cell1Complete = true;
                cell1.SetQuestData(null);
                TextChangeEvent();
                _playerQuest.QuestIdComplete(QuestsLib.QUEST_START_DELIVER1);
            },
            null,true,false));
        var msg = new MessageDialogData(str,ans,true);
        return msg;
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
        if (!cell1Complete)
        {
            return cell1;
        }

        return null;

    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("questStartDeliverFindCoordinator")}";
    }
}
