using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_3_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmOcr2_3_Start()    
        :base(QuestsLib.QuestStageCmOcr2_3_Start)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_3_Start);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_3_dialog_0_M1");
        list.Add("cmOcr2_3_dialog_0_A1");    
        list.Add("cmOcr2_3_dialog_0_M2");
        list.Add("cmOcr2_3_dialog_0_A2");    
        list.Add("cmOcr2_3_dialog_0_M3");
        list.Add("cmOcr2_3_dialog_0_A3");    
        list.Add("cmOcr2_3_dialog_0_M4");
        list.Add("cmOcr2_3_dialog_0_A4");    
        list.Add("cmOcr2_3_dialog_0_M5");
        list.Add("cmOcr2_3_dialog_0_A5");    
        list.Add("cmOcr2_3_dialog_0_M6");
        list.Add("cmOcr2_3_dialog_0_A6");    
        list.Add("cmOcr2_3_dialog_0_M7");
        list.Add("cmOcr2_3_dialog_0_A7");    
        list.Add("cmOcr2_3_dialog_0_M8");
        list.Add("cmOcr2_3_dialog_0_A8");    
        list.Add("cmOcr2_3_dialog_0_M9");
        list.Add("cmOcr2_3_dialog_0_A9");   
        list.Add("cmOcr2_3_dialog_0_M10");
        list.Add("cmOcr2_3_dialog_0_A10");       
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
