using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageComeAndDays : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;


    public QuestStageComeAndDays()    
        :base(QuestsLib.QUEST_START_PROTECT_FORTRESS1)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var posibleSectors = GetSectors(player, 0, 4,1);
        if (posibleSectors.Count < 1)
        {
            return false;
        }


        cell1 = FindAndMarkCell( posibleSectors.RandomElement(), Dialog) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data  != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
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
        string str = Namings.Tag("questReadyStartProtect");
        //        var player = MainController.Instance.MainPlayer;

        ans.Add(new AnswerDialogData(Namings.Tag("Ok"),
            () =>
            {
                cell1.SetQuestData(null);
                TextChangeEvent();
                _playerQuest.QuestIdComplete(QuestsLib.QUEST_START_PROTECT_FORTRESS1);
            },
            null, true, false));
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
        return cell1;

    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("questProtectFortressFind")}";
    }
}
