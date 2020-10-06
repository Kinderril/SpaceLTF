using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_3_End : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc3_3_End()    
        :base(QuestsLib.CM_MERC_3_3_END)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        sectorId.UnHide();
        cell1 = FindAndMarkCell(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
    public GlobalMapCell FindAndMarkCell(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data != playerCell && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        int maxID = 0;
        SectorCellContainer container = null;
        foreach (var sectorCellContainer in cells)
        {
            var freeAction = sectorCellContainer.Data as FreeActionGlobalMapCell;
            if (freeAction.Id > maxID)
            {
                maxID = freeAction.Id;
                container = sectorCellContainer;
            }
        }


//        Debug.LogError($"End cell id:{maxID}");
        var cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;

    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_3_3_END);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc3_3_dialog_end_M1");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M2");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M3");
        list.Add("cmMerc3_3_dialog_end_A3");    
        list.Add("cmMerc3_3_dialog_end_M4");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M5");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M6");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M7");
        list.Add("cmMerc3_3_dialog_end_A7");    
        list.Add("cmMerc3_3_dialog_end_M8");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M9");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M10");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M11");
        list.Add("DC");    
        list.Add("cmMerc3_3_dialog_end_M12");
        list.Add("cmMerc3_3_dialog_end_A12");    
        list.Add("cmMerc3_3_dialog_end_M13");
        list.Add("DC");       
        list.Add("cmMerc3_3_dialog_end_M14");
        list.Add("cmMerc3_3_dialog_end_A14");      
        list.Add("cmMerc3_3_dialog_end_M15");
        list.Add("cmMerc3_3_dialog_end_A15");   
        list.Add("cmMerc3_3_dialog_end_M16");
        list.Add("cmMerc3_3_dialog_end_A16");   
        list.Add("cmMerc3_3_dialog_end_M17");
        list.Add("cmMerc3_3_dialog_end_A17");
        list.Add("cmMerc3_3_dialog_end_M18");
        list.Add("cmMerc3_3_dialog_end_A18");
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
