using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_2_Start2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmOcr2_2_Start2()    
        :base(QuestsLib.QuestStageCmOcr2_2_Start2)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.federation);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_Start2);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_2_M1");
        list.Add("cmOcr2_2_dialog_2_A1"); 
        list.Add("cmOcr2_2_dialog_2_M2");
        list.Add("cmOcr2_2_dialog_2_A2"); 
        list.Add("cmOcr2_2_dialog_2_M3");
        list.Add("cmOcr2_2_dialog_2_A3"); 
        list.Add("cmOcr2_2_dialog_2_M4");
        list.Add("cmOcr2_2_dialog_2_A4"); 
        list.Add("cmOcr2_2_dialog_2_M5");
        list.Add("cmOcr2_2_dialog_2_A5");          
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
