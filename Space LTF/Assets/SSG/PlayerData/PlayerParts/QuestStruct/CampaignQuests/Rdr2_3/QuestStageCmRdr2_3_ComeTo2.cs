﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_3_ComeTo2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id = 0;

    public QuestStageCmRdr2_3_ComeTo2(int id)    
        :base(QuestsLib.QuestStageCmRdr2_3_ComeTo1 + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
//        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);
        var sectorId = player.MapData.CurrentCell.Sector;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_3_ComeTo1 + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr2_3_dialog_5_M0");
                list.Add("DC");
                list.Add("cmRdr2_3_dialog_5_M1");
                list.Add("cmRdr2_3_dialog_5_A2");
                list.Add("cmRdr2_3_dialog_5_M3");
                list.Add("cmRdr2_3_dialog_5_A4");
                list.Add("cmRdr2_3_dialog_5_M5");
                list.Add("cmRdr2_3_dialog_5_A6");
                list.Add("cmRdr2_3_dialog_5_M7");
                list.Add("cmRdr2_3_dialog_5_A8");
                list.Add("cmRdr2_3_dialog_5_M9");
                list.Add("DC");
                list.Add("cmRdr2_3_dialog_5_M10");
                list.Add("cmRdr2_3_dialog_5_A11");
                list.Add("cmRdr2_3_dialog_5_M12");
                list.Add("cmRdr2_3_dialog_5_A13");

                break;
            case 2:
                list.Add("cmRdr2_3_dialog_6_M1");
                list.Add("cmRdr2_3_dialog_6_A1");
                list.Add("cmRdr2_3_dialog_6_M2");
                list.Add("cmRdr2_3_dialog_6_A3");
                list.Add("cmRdr2_3_dialog_6_M4");
                list.Add("cmRdr2_3_dialog_6_A5");
                list.Add("cmRdr2_3_dialog_6_M6");
                list.Add("cmRdr2_3_dialog_6_A7");
                list.Add("cmRdr2_3_dialog_6_M8");
                list.Add("cmRdr2_3_dialog_6_A9");


                break;
        }



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
