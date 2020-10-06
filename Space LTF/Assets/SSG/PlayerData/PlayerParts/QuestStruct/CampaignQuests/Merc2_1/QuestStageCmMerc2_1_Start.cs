using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_1_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_1_Start()    
        :base(QuestsLib.CM_MERC_2_1_START)
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_1_START);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_1_dialog_1_M1");   
        list.Add("cmMerc2_1_dialog_1_A1");  
        list.Add("cmMerc2_1_dialog_1_M2");      
        list.Add("cmMerc2_1_dialog_1_A2"); 
        list.Add("cmMerc2_1_dialog_1_M3");      
        list.Add("cmMerc2_1_dialog_1_A3"); 
        list.Add("cmMerc2_1_dialog_1_M4");      
        list.Add("cmMerc2_1_dialog_1_A4"); 
        list.Add("cmMerc2_1_dialog_1_M5");      
        list.Add("cmMerc2_1_dialog_1_A5"); 
        list.Add("cmMerc2_1_dialog_1_M6");      
        list.Add("cmMerc2_1_dialog_1_A6"); 
        list.Add("cmMerc2_1_dialog_1_M7");      
        list.Add("DC");
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
