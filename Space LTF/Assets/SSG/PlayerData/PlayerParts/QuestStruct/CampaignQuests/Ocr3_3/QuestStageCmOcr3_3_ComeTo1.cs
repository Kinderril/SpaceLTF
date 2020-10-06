using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_3_ComeTo1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private SectorData sectorToOpen;
    public QuestStageCmOcr3_3_ComeTo1()    
        :base(QuestsLib.QuestStageCmOcr3_3_ComeTo1)
    {

    }

    protected override bool StageActivate(Player player)
    {
        sectorToOpen = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
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
        sectorToOpen.UnHide();
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_3_ComeTo1);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_1_M1");
        list.Add("cmOcr3_3_dialog_1_A1"); 
        list.Add("cmOcr3_3_dialog_1_M2");
        list.Add("cmOcr3_3_dialog_1_A2"); 
        list.Add("cmOcr3_3_dialog_1_M3");
        list.Add("DC"); 
        list.Add("cmOcr3_3_dialog_1_M4");
        list.Add("cmOcr3_3_dialog_1_A4"); 
        list.Add("cmOcr3_3_dialog_1_M5");
        list.Add("cmOcr3_3_dialog_1_A5"); 
        list.Add("cmOcr3_3_dialog_1_M6");
        list.Add("cmOcr3_3_dialog_1_A6"); 
        list.Add("cmOcr3_3_dialog_1_M7");
        list.Add("DC"); 
        list.Add("cmOcr3_3_dialog_1_M8");
        list.Add("cmOcr3_3_dialog_1_A8"); 
        list.Add("cmOcr3_3_dialog_1_M9");
        list.Add("DC"); 
        list.Add("cmOcr3_3_dialog_1_M10");
        list.Add("cmOcr3_3_dialog_1_A10"); 
        list.Add("cmOcr3_3_dialog_1_M11");
        list.Add("cmOcr3_3_dialog_1_A11"); 
        list.Add("cmOcr3_3_dialog_1_M12");
        list.Add("cmOcr3_3_dialog_1_A12"); 
        list.Add("cmOcr3_3_dialog_1_M13");
        list.Add("cmOcr3_3_dialog_1_A13");  
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
