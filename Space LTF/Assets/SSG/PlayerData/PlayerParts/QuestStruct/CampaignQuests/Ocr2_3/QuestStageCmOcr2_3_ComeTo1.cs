using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_3_ComeTo1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmOcr2_3_ComeTo1()    
        :base(QuestsLib.QuestStageCmOcr2_3_ComeTo1)
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_3_ComeTo1);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_3_dialog_5_M1");
        list.Add("cmOcr2_3_dialog_5_A1");  
        list.Add("cmOcr2_3_dialog_5_M2");
        list.Add("cmOcr2_3_dialog_5_A2");  
        list.Add("cmOcr2_3_dialog_5_M3");
        list.Add("cmOcr2_3_dialog_5_A3");  
        list.Add("cmOcr2_3_dialog_5_M4");
        list.Add("cmOcr2_3_dialog_5_A4");   
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
