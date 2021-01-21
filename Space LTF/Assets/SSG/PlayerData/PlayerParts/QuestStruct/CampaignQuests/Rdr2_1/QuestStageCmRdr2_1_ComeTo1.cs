using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_1_ComeTo1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr2_1_ComeTo1()    
        :base(QuestsLib.QuestStageCmRdr2_1_ComeTo1)
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_1_ComeTo1);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr2_1_dialog_2_M1");
        list.Add("cmRdr2_1_dialog_2_A1");
        list.Add("cmRdr2_1_dialog_2_M2");
        list.Add("cmRdr2_1_dialog_2_A3");
        list.Add("cmRdr2_1_dialog_2_M4");
        list.Add("cmRdr2_1_dialog_2_A5");
        list.Add("cmRdr2_1_dialog_2_M6");
        list.Add("cmRdr2_1_dialog_2_A7");
        list.Add("cmRdr2_1_dialog_2_M8");
        list.Add("cmRdr2_1_dialog_2_A9");
        list.Add("cmRdr2_1_dialog_2_M10");
        list.Add("DC");
        list.Add("cmRdr2_1_dialog_2_M11");
        list.Add("cmRdr2_1_dialog_2_A12");

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
