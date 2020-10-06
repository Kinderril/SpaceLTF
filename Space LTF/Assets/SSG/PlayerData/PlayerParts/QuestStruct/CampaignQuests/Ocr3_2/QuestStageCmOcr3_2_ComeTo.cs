using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_2_ComeTo : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmOcr3_2_ComeTo(int id)    
        :base(QuestsLib.QuestStageCmOcr3_2_ComeTo + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        ShipConfig cfg = ShipConfig.droid;
        switch (_id)
        {
            case 1:
                break;
            case 2:
                cfg = ShipConfig.krios;
                break;
            case 3:
                cfg = ShipConfig.federation;
                break;
            
        }

        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == cfg);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_2_ComeTo + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmOcr3_2_dialog_1_M1");
                list.Add("cmOcr3_2_dialog_1_A1");  
                list.Add("cmOcr3_2_dialog_1_M2");
                list.Add("cmOcr3_2_dialog_1_A2");  
                list.Add("cmOcr3_2_dialog_1_M3");
                list.Add("cmOcr3_2_dialog_1_A3");  
                list.Add("cmOcr3_2_dialog_1_M4");
                list.Add("cmOcr3_2_dialog_1_A4");  
                list.Add("cmOcr3_2_dialog_1_M5");
                list.Add("cmOcr3_2_dialog_1_A5");  
                list.Add("cmOcr3_2_dialog_1_M6");
                list.Add("DC");  
                list.Add("cmOcr3_2_dialog_1_M7");
                list.Add("cmOcr3_2_dialog_1_A7");
                break;
            case 2:
                list.Add("cmOcr3_2_dialog_2_M1");
                list.Add("cmOcr3_2_dialog_2_A1");   
                list.Add("cmOcr3_2_dialog_2_M2");
                list.Add("cmOcr3_2_dialog_2_A2");   
                list.Add("cmOcr3_2_dialog_2_M3");
                list.Add("cmOcr3_2_dialog_2_A3");   
                list.Add("cmOcr3_2_dialog_2_M4");
                list.Add("cmOcr3_2_dialog_2_A4");   
                list.Add("cmOcr3_2_dialog_2_M5");
                list.Add("cmOcr3_2_dialog_2_A5");   
                list.Add("cmOcr3_2_dialog_2_M6");
                list.Add("cmOcr3_2_dialog_2_A6");   
                list.Add("cmOcr3_2_dialog_2_M7");
                list.Add("cmOcr3_2_dialog_2_A7");   
                list.Add("cmOcr3_2_dialog_2_M8");
                list.Add("cmOcr3_2_dialog_2_A8");   
                list.Add("cmOcr3_2_dialog_2_M9");
                list.Add("cmOcr3_2_dialog_2_A9");   
                list.Add("cmOcr3_2_dialog_2_M10");
                list.Add("cmOcr3_2_dialog_2_A10");   
                list.Add("cmOcr3_2_dialog_2_M11");
                list.Add("cmOcr3_2_dialog_2_A11");
                break;
            case 3:
                list.Add("cmOcr3_2_dialog_3_M1");
                list.Add("cmOcr3_2_dialog_3_A1"); 
                list.Add("cmOcr3_2_dialog_3_M2");
                list.Add("cmOcr3_2_dialog_3_A2"); 
                list.Add("cmOcr3_2_dialog_3_M3");
                list.Add("cmOcr3_2_dialog_3_A3"); 
                list.Add("cmOcr3_2_dialog_3_M4");
                list.Add("cmOcr3_2_dialog_3_A4"); 
                list.Add("cmOcr3_2_dialog_3_M5");
                list.Add("DC");
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
        return $"{Namings.Tag("cmOcrDream")}";
    }
}
