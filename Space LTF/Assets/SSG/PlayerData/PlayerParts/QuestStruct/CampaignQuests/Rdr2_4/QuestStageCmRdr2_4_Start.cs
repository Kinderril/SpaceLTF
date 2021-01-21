using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_4_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr2_4_Start()    
        :base(QuestsLib.QuestStageCmRdr2_4_Start)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.raiders);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_4_Start);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>(); 
        list.Add("cmRdr2_4_dialog_1_M0");
        list.Add("DC");
        list.Add("cmRdr2_4_dialog_1_M1");
        list.Add("cmRdr2_4_dialog_1_A2");
        list.Add("cmRdr2_4_dialog_1_M3");
        list.Add("cmRdr2_4_dialog_1_A4");
        list.Add("cmRdr2_4_dialog_1_M5");
        list.Add("cmRdr2_4_dialog_1_A6");
        list.Add("cmRdr2_4_dialog_1_M7");
        list.Add("cmRdr2_4_dialog_1_A8");
        list.Add("cmRdr2_4_dialog_1_M9");
        list.Add("cmRdr2_4_dialog_1_A10");
        list.Add("cmRdr2_4_dialog_1_M11");
        list.Add("cmRdr2_4_dialog_1_A12");


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
