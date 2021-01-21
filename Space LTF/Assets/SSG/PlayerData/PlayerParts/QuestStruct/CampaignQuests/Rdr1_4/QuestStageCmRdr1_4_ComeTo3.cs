using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_4_ComeTo3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr1_4_ComeTo3()    
        :base(QuestsLib.QuestStageCmRdr1_4_ComeTo3)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.raiders);
        cell1 = FindAndMarkCellRandom(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_4_ComeTo3);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_4_dialog_4_M1");
        list.Add("DC");
        list.Add("cmRdr1_4_dialog_4_M2");
        list.Add("cmRdr1_4_dialog_4_A2");  
        list.Add("cmRdr1_4_dialog_4_M3");
        list.Add("cmRdr1_4_dialog_4_A3");  
        list.Add("cmRdr1_4_dialog_4_M4");
        list.Add("cmRdr1_4_dialog_4_A4");  
        list.Add("cmRdr1_4_dialog_4_M5");
        list.Add("cmRdr1_4_dialog_4_A5");  
        list.Add("cmRdr1_4_dialog_4_M6");
        list.Add("cmRdr1_4_dialog_4_A6");  
        list.Add("cmRdr1_4_dialog_4_M7");
        list.Add("cmRdr1_4_dialog_4_A7");   
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
