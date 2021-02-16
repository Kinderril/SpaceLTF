using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr3_3_ComeTo1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id = 0;
    private int _index = 0;

    public QuestStageCmRdr3_3_ComeTo1(int id,int index)    
        :base(QuestsLib.QuestStageCmRdr3_3_ComeTo1 + id.ToString())
    {
        _id = id;
        _index = index;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        sectorId.UnHide();
        cell1 = FindAndMarkCell(sectorId, GetDialog, _index) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr3_3_ComeTo1 + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr3_3_dialog_4_M0");
                list.Add("cmRdr3_3_dialog_4_A1");
                list.Add("cmRdr3_3_dialog_4_M2");
                list.Add("cmRdr3_3_dialog_4_A3");
                list.Add("cmRdr3_3_dialog_4_M4");
                list.Add("cmRdr3_3_dialog_4_A5");

                break;
        }



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
