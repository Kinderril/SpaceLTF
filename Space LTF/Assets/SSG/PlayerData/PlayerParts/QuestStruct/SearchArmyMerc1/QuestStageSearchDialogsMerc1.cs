using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageSearchDialogsMerc1 : QuestStage//  , ISerializable
{

    private FreeActionGlobalMapCell cell1 = null;
    private FreeActionGlobalMapCell cell2 = null;

    private bool cell1Complete;
    private bool cell2Complete;

    public QuestStageSearchDialogsMerc1()    
        :base(QuestsLib.QUEST_MERC_FIND_TARGET1, QuestsLib.QUEST_MERC_FIND_TARGET2)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var posibleSectors = GetSectors(player, 0, 4,2);
        if (posibleSectors.Count < 2)
        {
            return false;
        }

        var rnd1 = posibleSectors.RandomElement();
        posibleSectors.Remove(rnd1);
        cell1 = FindAndMarkCell(posibleSectors.RandomElement(), GetDialog1) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        cell2  = FindAndMarkCell(posibleSectors.RandomElement(), GetDialog2) as FreeActionGlobalMapCell;
        if (cell2 == null)
        {
            return false;
        }

        return true;

    }

    private MessageDialogData GetDialog1()
    {
        return Dialog(QuestsLib.QUEST_MERC_FIND_TARGET1);
    }        
    private MessageDialogData GetDialog2()
    {
        return Dialog(QuestsLib.QUEST_MERC_FIND_TARGET2);
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

    private MessageDialogData Dialog(string keyComplete)
    {
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        string str = Namings.Tag("questSearachFleetMerc");
        ans.Add(new AnswerDialogData(Namings.Tag("Ok"),
            () =>
            {
                if (QuestsLib.QUEST_MERC_FIND_TARGET1 == keyComplete)
                {
                    cell1Complete = true;
                    cell1.SetQuestData(null);

                }
                else
                {
                    cell2Complete = true;
                    cell2.SetQuestData(null);

                }
                //                if (cell1Complete && cell2Complete)     
                TextChangeEvent();
                _playerQuest.QuestIdComplete(keyComplete);
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

        if (!cell2Complete)
        {
            return cell2;
        }

        return null;

    }

    public override string GetDesc()
    {
        int completed = (cell1Complete ? 1 : 0) + (cell2Complete ? 1 : 0);

        return $"{Namings.Tag("questNameSearchFleetMerc")} {completed}/{2}" ;
    }
}
