using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_3_ComeToFinal : QuestStage
{
    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr3_3_ComeToFinal()    
        :base(QuestsLib.QuestStageCmOcr3_3_ComeToFinal)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        cell1 = FindAndMarkCell(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }
    public GlobalMapCell FindAndMarkCell(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        cells.Sort(comparator);

        SectorCellContainer container;
        FreeActionGlobalMapCell cell;
        var index = cells.Count - 1;
        container = cells[index];
        cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;
    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_3_ComeToFinal);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_9_M1");
        list.Add("cmOcr3_3_dialog_9_A1");
        list.Add("cmOcr3_3_dialog_9_M2");
        list.Add("cmOcr3_3_dialog_9_A2");
        list.Add("cmOcr3_3_dialog_9_M3");
        list.Add("cmOcr3_3_dialog_9_A3");
        list.Add("cmOcr3_3_dialog_9_M4");
        list.Add("DC");
        list.Add("cmOcr3_3_dialog_9_M5");
        list.Add("DC");
        list.Add("cmOcr3_3_dialog_9_M6");
        list.Add("cmOcr3_3_dialog_9_A6");
        list.Add("cmOcr3_3_dialog_9_M7");
        list.Add("cmOcr3_3_dialog_9_A7");
        list.Add("cmOcr3_3_dialog_9_M8");
        list.Add("cmOcr3_3_dialog_9_A8");
        list.Add("cmOcr3_3_dialog_9_M9");
        list.Add("cmOcr3_3_dialog_9_A9");
        list.Add("cmOcr3_3_dialog_9_M10");
        list.Add("cmOcr3_3_dialog_9_A10");
        list.Add("cmOcr3_3_dialog_9_M11");
        list.Add("DC");
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
