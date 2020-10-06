using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_2_Start3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmOcr2_2_Start3()    
        :base(QuestsLib.QuestStageCmOcr2_2_Start3)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.krios);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_Start3);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_6_M1");
        list.Add("cmOcr2_2_dialog_6_A1"); 
        list.Add("cmOcr2_2_dialog_6_M2");
        list.Add("cmOcr2_2_dialog_6_A2"); 
        list.Add("cmOcr2_2_dialog_6_M3");
        list.Add("cmOcr2_2_dialog_6_A3"); 
        list.Add("cmOcr2_2_dialog_6_M4");
        list.Add("cmOcr2_2_dialog_6_A4"); 
        list.Add("cmOcr2_2_dialog_6_M5");
        list.Add("cmOcr2_2_dialog_6_A5"); 
        list.Add("cmOcr2_2_dialog_6_M6");
        list.Add("cmOcr2_2_dialog_6_A6"); 
        list.Add("cmOcr2_2_dialog_6_M7");
        list.Add("cmOcr2_2_dialog_6_A7"); 
        list.Add("cmOcr2_2_dialog_6_M8");
        list.Add("cmOcr2_2_dialog_6_A8"); 
        list.Add("cmOcr2_2_dialog_6_M9");
        list.Add("cmOcr2_2_dialog_6_A9"); 
        list.Add("cmOcr2_2_dialog_6_M10");
        list.Add("cmOcr2_2_dialog_6_A10");        
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
