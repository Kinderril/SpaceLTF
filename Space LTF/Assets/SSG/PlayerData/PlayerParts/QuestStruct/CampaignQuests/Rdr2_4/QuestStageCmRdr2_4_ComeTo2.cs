using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_4_ComeTo2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id = 0;

    public QuestStageCmRdr2_4_ComeTo2(int id)    
        :base(QuestsLib.QuestStageCmRdr2_4_ComeTo2 + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.droid);
//        var sectorId = player.MapData.CurrentCell.Sector;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_4_ComeTo2 + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr2_4_dialog_2_M0");
                list.Add("cmRdr2_4_dialog_2_A1");
                list.Add("cmRdr2_4_dialog_2_M2");
                list.Add("cmRdr2_4_dialog_2_A3");
                list.Add("cmRdr2_4_dialog_2_M4");
                list.Add("cmRdr2_4_dialog_2_A5");
                list.Add("cmRdr2_4_dialog_2_M6");
                list.Add("cmRdr2_4_dialog_2_A7");
                list.Add("cmRdr2_4_dialog_2_M8");
                list.Add("cmRdr2_4_dialog_2_A9");
                list.Add("cmRdr2_4_dialog_2_M10");
                list.Add("cmRdr2_4_dialog_2_A11");
                list.Add("cmRdr2_4_dialog_2_M12");
                list.Add("cmRdr2_4_dialog_2_A13");


                break;
            case 2:
                list.Add("cmRdr2_4_dialog_3_M0");
                list.Add("cmRdr2_4_dialog_3_A1");
                list.Add("cmRdr2_4_dialog_3_M2");
                list.Add("cmRdr2_4_dialog_3_A3");
                list.Add("cmRdr2_4_dialog_3_M4");
                list.Add("cmRdr2_4_dialog_3_A5");
                list.Add("cmRdr2_4_dialog_3_M6");
                list.Add("cmRdr2_4_dialog_3_A7");
                list.Add("cmRdr2_4_dialog_3_M8");
                list.Add("cmRdr2_4_dialog_3_A9");
                list.Add("cmRdr2_4_dialog_3_M10");
                list.Add("cmRdr2_4_dialog_3_A11");
                list.Add("cmRdr2_4_dialog_3_M12");
                list.Add("cmRdr2_4_dialog_3_A13");
                list.Add("cmRdr2_4_dialog_3_M14");
                list.Add("cmRdr2_4_dialog_3_A15");
                list.Add("cmRdr2_4_dialog_3_M16");
                list.Add("cmRdr2_4_dialog_3_A17");
                list.Add("cmRdr2_4_dialog_3_M18");
                list.Add("cmRdr2_4_dialog_3_A19");
                list.Add("cmRdr2_4_dialog_3_M20");
                list.Add("cmRdr2_4_dialog_3_A21");



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
