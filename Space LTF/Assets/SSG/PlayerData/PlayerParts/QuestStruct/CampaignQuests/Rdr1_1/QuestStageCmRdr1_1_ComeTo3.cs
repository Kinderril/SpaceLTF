﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_1_ComeTo3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr1_1_ComeTo3()    
        :base(QuestsLib.QuestStageCmRdr1_1_ComeTo3)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_1_ComeTo3);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_1_dialog_3_M1");
        list.Add("cmRdr1_1_dialog_3_A1");      
        list.Add("cmRdr1_1_dialog_3_M2");
        list.Add("cmRdr1_1_dialog_3_A2");      
        list.Add("cmRdr1_1_dialog_3_M3");
        list.Add("cmRdr1_1_dialog_3_A3");      
        list.Add("cmRdr1_1_dialog_3_M4");
        list.Add("cmRdr1_1_dialog_3_A4");      
        list.Add("cmRdr1_1_dialog_3_M5");
        list.Add("cmRdr1_1_dialog_3_A5");      
        list.Add("cmRdr1_1_dialog_3_M6");
        list.Add("cmRdr1_1_dialog_3_A6");    
        list.Add("cmRdr1_1_dialog_3_M7");
        list.Add("cmRdr1_1_dialog_3_A7");    
        list.Add("cmRdr1_1_dialog_3_M8");
        list.Add("cmRdr1_1_dialog_3_A8");    
        list.Add("cmRdr1_1_dialog_3_M9");
        list.Add("cmRdr1_1_dialog_3_A9");   
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
