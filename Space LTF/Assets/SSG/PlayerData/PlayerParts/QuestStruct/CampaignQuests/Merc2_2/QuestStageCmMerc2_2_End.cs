using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_2_End : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_2_End()    
        :base(QuestsLib.CM_MERC_2_2_END)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_2_END);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_2_dialog_end_M1");    
        list.Add("cmMerc2_2_dialog_end_A1");  
        list.Add("cmMerc2_2_dialog_end_M2");    
        list.Add("cmMerc2_2_dialog_end_A2");  
        list.Add("cmMerc2_2_dialog_end_M3");    
        list.Add("cmMerc2_2_dialog_end_A3");  
        list.Add("cmMerc2_2_dialog_end_M4");    
        list.Add("cmMerc2_2_dialog_end_A4");  
        list.Add("cmMerc2_2_dialog_end_M5");    
        list.Add("cmMerc2_2_dialog_end_A5");  
        list.Add("cmMerc2_2_dialog_end_M6");    
        list.Add("cmMerc2_2_dialog_end_A6");  
        list.Add("cmMerc2_2_dialog_end_M7");    
        list.Add("cmMerc2_2_dialog_end_A7");  
        list.Add("cmMerc2_2_dialog_end_M8");    
        list.Add("cmMerc2_2_dialog_end_A8");  
        list.Add("cmMerc2_2_dialog_end_M9");    
        list.Add("cmMerc2_2_dialog_end_A9");
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
