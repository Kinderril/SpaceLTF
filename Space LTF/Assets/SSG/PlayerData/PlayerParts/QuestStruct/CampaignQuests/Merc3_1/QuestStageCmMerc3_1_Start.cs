using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_1_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _step;

    public QuestStageCmMerc3_1_Start(int step)    
        :base(QuestsLib.CM_MERC_3_1_START + step)
    {
        _step = step;
        if (!(_step == 1 || step == 2))
        {
            Debug.LogError("wrong QuestStageCmMerc3_1_Start ");
        }
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.CurrentCell.Sector;
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_3_1_START + _step);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_step)
        {
            case 1:
                list.Add("cmMerc3_1_dialog_0_M1");   
                list.Add("cmMerc3_1_dialog_0_A1");   
                list.Add("cmMerc3_1_dialog_0_M2");   
                list.Add("cmMerc3_1_dialog_0_A2");   
                list.Add("cmMerc3_1_dialog_0_M3");   
                list.Add("cmMerc3_1_dialog_0_A3");
                break;
            case 2:
                list.Add("cmMerc3_1_dialog_1_M1");      //NO LOC
                list.Add("cmMerc3_1_dialog_1_A1");          
                list.Add("cmMerc3_1_dialog_1_M2");      //NO LOC
                list.Add("cmMerc3_1_dialog_1_A2");          
                list.Add("cmMerc3_1_dialog_1_M3");      //NO LOC
                list.Add("cmMerc3_1_dialog_1_A3");          
                list.Add("cmMerc3_1_dialog_1_M4");      //NO LOC
                list.Add("cmMerc3_1_dialog_1_A4");
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
