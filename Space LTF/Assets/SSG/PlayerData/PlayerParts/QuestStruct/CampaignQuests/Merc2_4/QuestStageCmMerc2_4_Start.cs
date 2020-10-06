using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_4_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_4_Start()    
        :base(QuestsLib.CM_MERC_2_4_START)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_4_START);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_4_dialog_start_M1");   
        list.Add("cmMerc2_4_dialog_start_A1");   
        list.Add("cmMerc2_4_dialog_start_M2");   
        list.Add("cmMerc2_4_dialog_start_A2");   
        list.Add("cmMerc2_4_dialog_start_M3");   
        list.Add("cmMerc2_4_dialog_start_A3");   
        list.Add("cmMerc2_4_dialog_start_M4");   
        list.Add("cmMerc2_4_dialog_start_A4");   
        list.Add("cmMerc2_4_dialog_start_M5");   
        list.Add("cmMerc2_4_dialog_start_A5");   
        list.Add("cmMerc2_4_dialog_start_M6");   
        list.Add("cmMerc2_4_dialog_start_A6");
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
