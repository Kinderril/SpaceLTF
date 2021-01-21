using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_1_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmRdr2_1_Start()    
        :base(QuestsLib.QuestStageCmRdr2_1_Start)
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_1_Start);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr2_1_dialog_1_M1");
        list.Add("cmRdr2_1_dialog_1_A1");
        list.Add("cmRdr2_1_dialog_1_M2");
        list.Add("cmRdr2_1_dialog_1_A3");
        list.Add("cmRdr2_1_dialog_1_M4");
        list.Add("cmRdr2_1_dialog_1_A5");
        list.Add("cmRdr2_1_dialog_1_M6");
        list.Add("cmRdr2_1_dialog_1_A7");
        list.Add("cmRdr2_1_dialog_1_M8");
        list.Add("DC");
        list.Add("cmRdr2_1_dialog_1_M9");
        list.Add("cmRdr2_1_dialog_1_A10");

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
