using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_3_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc1_3_Start()    
        :base(QuestsLib.CM_MERC_1_3_START)
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_1_3_START);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc1_3_dialog_0_M1");
        list.Add("cmMerc1_3_dialog_0_A1");  
        list.Add("cmMerc1_3_dialog_0_M2");
        list.Add("cmMerc1_3_dialog_0_A2");  
        list.Add("cmMerc1_3_dialog_0_M3");
        list.Add("DC");  
        list.Add("cmMerc1_3_dialog_0_M4");
        list.Add("DC");  
        list.Add("cmMerc1_3_dialog_0_M5");
        list.Add("cmMerc1_3_dialog_0_A5"); 
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
