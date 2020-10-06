using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_1_ComeToExit2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr1_1_ComeToExit2()    
        :base(QuestsLib.QuestStageCmRdr1_1_ComeToExit2)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig != ShipConfig.ocrons);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_1_ComeToExit2);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_1_dialog_9_M1");
        list.Add("DC");      
        list.Add("cmRdr1_1_dialog_9_M2");
        list.Add("cmRdr1_1_dialog_9_A2");      
        list.Add("cmRdr1_1_dialog_9_M3");
        list.Add("DC");      
        list.Add("cmRdr1_1_dialog_9_M4");
        list.Add("cmRdr1_1_dialog_9_A4");      
        list.Add("cmRdr1_1_dialog_9_M5");
        list.Add("cmRdr1_1_dialog_9_A5");  
        list.Add("cmRdr1_1_dialog_9_M6");
        list.Add("cmRdr1_1_dialog_9_A6");  
        list.Add("cmRdr1_1_dialog_9_M7");
        list.Add("cmRdr1_1_dialog_9_A7");  
        list.Add("cmRdr1_1_dialog_9_M8");
        list.Add("cmRdr1_1_dialog_9_A8");     
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
