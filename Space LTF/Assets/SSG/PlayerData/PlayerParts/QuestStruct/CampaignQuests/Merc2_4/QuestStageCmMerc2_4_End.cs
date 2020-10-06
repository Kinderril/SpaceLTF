﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_4_End : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_4_End()    
        :base(QuestsLib.CM_MERC_2_4_END)
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
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_4_END);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_4_dialog_end_M1");
        list.Add("cmMerc2_4_dialog_end_A1");  
        list.Add("cmMerc2_4_dialog_end_M2");
        list.Add("cmMerc2_4_dialog_end_A2");  
        list.Add("cmMerc2_4_dialog_end_M3");
        list.Add("cmMerc2_4_dialog_end_A3");
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
