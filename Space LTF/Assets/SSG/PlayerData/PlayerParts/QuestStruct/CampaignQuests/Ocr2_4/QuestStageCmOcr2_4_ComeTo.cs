using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_4_ComeTo : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;

    public QuestStageCmOcr2_4_ComeTo(int id)    
        :base(QuestsLib.QuestStageCmOcr2_4_ComeTo + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        ShipConfig config = ShipConfig.droid;
        switch (_id)
        {
            case 1:
                config = ShipConfig.ocrons;
                break;
            case 2:
                config = ShipConfig.federation;
                break;
            case 3:
                config = ShipConfig.droid;
                break;
            case 4:
                config = ShipConfig.droid;
                break;
        }

        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == config);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_4_ComeTo + _id.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmOcr2_4_dialog_1_M1");
                list.Add("cmOcr2_4_dialog_1_A1"); 
                list.Add("cmOcr2_4_dialog_1_M2");
                list.Add("cmOcr2_4_dialog_1_A2"); 
                list.Add("cmOcr2_4_dialog_1_M3");
                list.Add("cmOcr2_4_dialog_1_A3"); 
                list.Add("cmOcr2_4_dialog_1_M4");
                list.Add("cmOcr2_4_dialog_1_A4"); 
                list.Add("cmOcr2_4_dialog_1_M5");
                list.Add("cmOcr2_4_dialog_1_A5"); 
                list.Add("cmOcr2_4_dialog_1_M6");
                list.Add("cmOcr2_4_dialog_1_A6");
                break;
            case 2:
                list.Add("cmOcr2_4_dialog_2_M1");
                list.Add("cmOcr2_4_dialog_2_A1");     
                list.Add("cmOcr2_4_dialog_2_M2");
                list.Add("cmOcr2_4_dialog_2_A2");     
                list.Add("cmOcr2_4_dialog_2_M3");
                list.Add("cmOcr2_4_dialog_2_A3");     
                list.Add("cmOcr2_4_dialog_2_M4");
                list.Add("cmOcr2_4_dialog_2_A4");     
                list.Add("cmOcr2_4_dialog_2_M5");
                list.Add("cmOcr2_4_dialog_2_A5");
                break;
            case 3:
                list.Add("cmOcr2_4_dialog_3_M1");
                list.Add("cmOcr2_4_dialog_3_A1");      
                list.Add("cmOcr2_4_dialog_3_M2");
                list.Add("cmOcr2_4_dialog_3_A2");      
                list.Add("cmOcr2_4_dialog_3_M3");
                list.Add("cmOcr2_4_dialog_3_A3");      
                list.Add("cmOcr2_4_dialog_3_M4");
                list.Add("cmOcr2_4_dialog_3_A4");      
                list.Add("cmOcr2_4_dialog_3_M5");
                list.Add("cmOcr2_4_dialog_3_A5");      
                list.Add("cmOcr2_4_dialog_3_M6");
                list.Add("cmOcr2_4_dialog_3_A6");    
                list.Add("cmOcr2_4_dialog_3_M7");
                list.Add("cmOcr2_4_dialog_3_A7");
                break;
            case 4:
                list.Add("cmOcr2_4_dialog_4_M1");
                list.Add("cmOcr2_4_dialog_4_A1");    
                list.Add("cmOcr2_4_dialog_4_M2");
                list.Add("cmOcr2_4_dialog_4_A2");    
                list.Add("cmOcr2_4_dialog_4_M3");
                list.Add("cmOcr2_4_dialog_4_A3");    
                list.Add("cmOcr2_4_dialog_4_M4");
                list.Add("cmOcr2_4_dialog_4_A4");    
                list.Add("cmOcr2_4_dialog_4_M5");
                list.Add("cmOcr2_4_dialog_4_A5");    
                list.Add("cmOcr2_4_dialog_4_M6");
                list.Add("cmOcr2_4_dialog_4_A6");    
                list.Add("cmOcr2_4_dialog_4_M7");
                list.Add("cmOcr2_4_dialog_4_A7");    
                list.Add("cmOcr2_4_dialog_4_M8");
                list.Add("cmOcr2_4_dialog_4_A8");    
                list.Add("cmOcr2_4_dialog_4_M9");
                list.Add("cmOcr2_4_dialog_4_A9");    
                list.Add("cmOcr2_4_dialog_4_M10");
                list.Add("cmOcr2_4_dialog_4_A10");    
                list.Add("cmOcr2_4_dialog_4_M11");
                list.Add("cmOcr2_4_dialog_4_A11");    
                list.Add("cmOcr2_4_dialog_4_M12");
                list.Add("cmOcr2_4_dialog_4_A12");    
                list.Add("cmOcr2_4_dialog_4_M13");
                list.Add("cmOcr2_4_dialog_4_A13");    
                list.Add("cmOcr2_4_dialog_4_M14");
                list.Add("cmOcr2_4_dialog_4_A14");
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
