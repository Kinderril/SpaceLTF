using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_2_ComeTo1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id = 0;

    public QuestStageCmRdr2_2_ComeTo1(int id)    
        :base(QuestsLib.QuestStageCmRdr2_2_ComeTo1 + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
//        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_2_ComeTo1 + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr2_2_dialog_1_M1");
                list.Add("cmRdr2_2_dialog_1_A1");
                list.Add("cmRdr2_2_dialog_1_M2");
                list.Add("cmRdr2_2_dialog_1_A3");
                list.Add("cmRdr2_2_dialog_1_M4");
                list.Add("cmRdr2_2_dialog_1_A5");
                list.Add("cmRdr2_2_dialog_1_M6");
                list.Add("cmRdr2_2_dialog_1_A7");
                list.Add("cmRdr2_2_dialog_1_M8");
                list.Add("DC");
                list.Add("cmRdr2_2_dialog_1_M9");
                list.Add("cmRdr2_2_dialog_1_A10");
                list.Add("cmRdr2_2_dialog_1_M11");
                list.Add("DC");
                list.Add("cmRdr2_2_dialog_1_M12");
                list.Add("cmRdr2_2_dialog_1_A13");
                break;
            case 2:
                list.Add("cmRdr2_2_dialog_2_M0");
                list.Add("DC");
                list.Add("cmRdr2_2_dialog_2_M1");
                list.Add("cmRdr2_2_dialog_2_A2");
                list.Add("cmRdr2_2_dialog_2_M3");
                list.Add("cmRdr2_2_dialog_2_A4");
                list.Add("cmRdr2_2_dialog_2_M5");
                list.Add("cmRdr2_2_dialog_2_A6");
                list.Add("cmRdr2_2_dialog_2_M7");
                list.Add("cmRdr2_2_dialog_2_A8");
                list.Add("cmRdr2_2_dialog_2_M9");
                list.Add("cmRdr2_2_dialog_2_A10");
                list.Add("cmRdr2_2_dialog_2_M11");
                list.Add("cmRdr2_2_dialog_2_A12");
                list.Add("cmRdr2_2_dialog_2_M13");
                list.Add("cmRdr2_2_dialog_2_A14");
                list.Add("cmRdr2_2_dialog_2_M15");
                list.Add("cmRdr2_2_dialog_2_A16");
                list.Add("cmRdr2_2_dialog_2_M17");
                list.Add("cmRdr2_2_dialog_2_A18");
                list.Add("cmRdr2_2_dialog_2_M19");
                list.Add("cmRdr2_2_dialog_2_A20");
                list.Add("cmRdr2_2_dialog_2_M21");
                list.Add("cmRdr2_2_dialog_2_A22");

                break;
            case 3:
                list.Add("cmRdr2_2_dialog_3_M1");
                list.Add("cmRdr2_2_dialog_3_A1");
                list.Add("cmRdr2_2_dialog_3_M2");
                list.Add("cmRdr2_2_dialog_3_A3");
                list.Add("cmRdr2_2_dialog_3_M4");
                list.Add("cmRdr2_2_dialog_3_A5");
                list.Add("cmRdr2_2_dialog_3_M6");
                list.Add("cmRdr2_2_dialog_3_A7");
                list.Add("cmRdr2_2_dialog_3_M8");
                list.Add("cmRdr2_2_dialog_3_A9");
                list.Add("cmRdr2_2_dialog_3_M10");
                list.Add("cmRdr2_2_dialog_3_A11");
                list.Add("cmRdr2_2_dialog_3_M12");
                list.Add("cmRdr2_2_dialog_3_A13");

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
