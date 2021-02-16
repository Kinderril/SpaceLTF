using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_4_End : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr2_4_End()    
        :base(QuestsLib.QuestStageCmRdr2_4_End)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.CurrentCell.Sector;
        cell1 = FindAndMarkCellClosest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_4_End);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>(); 
        list.Add("cmRdr2_4_dialog_5_M0");
        list.Add("cmRdr2_4_dialog_5_A1");
        list.Add("cmRdr2_4_dialog_5_M2");
        list.Add("cmRdr2_4_dialog_5_A3");
        list.Add("cmRdr2_4_dialog_5_M4");
        list.Add("cmRdr2_4_dialog_5_A5");
        list.Add("cmRdr2_4_dialog_5_M6");
        list.Add("cmRdr2_4_dialog_5_A7");

        return list;
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
        return $"{Namings.Tag("cmComeToPoint")}";
    }
}
