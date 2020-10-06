using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_3_Ctp : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_3_Ctp()    
        :base(QuestsLib.CM_MERC_2_3_M1)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig != ShipConfig.droid).ToList().RandomElement();
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_3_M1);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_3_dialog_2_M1");     
        list.Add("cmMerc2_3_dialog_2_A1");     
        list.Add("cmMerc2_3_dialog_2_M2");     
        list.Add("cmMerc2_3_dialog_2_A2");     
        list.Add("cmMerc2_3_dialog_2_M3");     
        list.Add("cmMerc2_3_dialog_2_A3");     
        list.Add("cmMerc2_3_dialog_2_M4");     
        list.Add("cmMerc2_3_dialog_2_A4");     
        list.Add("cmMerc2_3_dialog_2_M5");     
        list.Add("cmMerc2_3_dialog_2_A5");
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
