using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCampStartComeToPoint : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCampStartComeToPoint()    
        :base(QuestsLib.CM_START_QUEST)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.CurrentCell.Sector;

        cell1 = FindAndMarkCellFarest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(),DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_START_QUEST);
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmStartQuestEndM1");
        list.Add("cmStartQuestEndA1");
        list.Add("cmStartQuestEndM2");
        list.Add("cmStartQuestEndA2");
        list.Add("cmStartQuestEndM3");
        list.Add("cmStartQuestEndA3");    
        list.Add("cmStartQuestEndM4");
        list.Add("cmStartQuestEndA4");   
        list.Add("cmStartQuestEndM5");
        list.Add("cmStartQuestEndA5");   
        list.Add("cmStartQuestEndM6");
        list.Add("cmStartQuestEndA6");   
        list.Add("cmStartQuestEndM7");   
        list.Add("cmStartQuestEndA7");
        list.Add("cmStartQuestEndM8");
        list.Add("cmStartQuestEndA8");   
        list.Add("cmStartQuestEndM9");
        list.Add("cmStartQuestEndA9");   
        list.Add("cmStartQuestEndM10");
        list.Add("cmStartQuestEndA10");   
        list.Add("cmStartQuestEndM11");
        list.Add("cmStartQuestEndA11"); 
        list.Add("cmStartQuestEndM12");
        list.Add("cmStartQuestEndA12");
        return list;
    }




    protected override void StageDispose()
    {
        cell1.SetQuestData(null);
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
        return $"{Namings.Tag("cmComeToPoint")}";
    }
}
