using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_2_Find : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index;
    public QuestStageCmMerc3_2_Find(int index)    
        :base(QuestsLib.CM_MERC_2_4_TALK2 + index)
    {
        _index = index;
        if (!(index > 0 && index <= 5))
        {
            Debug.LogError($"wrong index QuestStageCmMerc3_2_Find :{index}");
        }
    }

    protected override bool StageActivate(Player player)
    {
        ShipConfig config = ShipConfig.droid;
        switch (_index)
        {
            case 1:
                config = ShipConfig.ocrons;
                break;
            case 2:
                config = ShipConfig.federation;
                break;
            case 3:
                config = ShipConfig.mercenary;
                break;
            case 4:
                config = ShipConfig.raiders;
                break;
            case 5:
                config = ShipConfig.krios;
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_4_TALK2 + _index);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_index)
        {
            case 1:
                list.Add("cmMerc3_2_dialog_1_M1");   
                list.Add("cmMerc3_2_dialog_1_A1");  
                list.Add("cmMerc3_2_dialog_1_M2");   
                list.Add("cmMerc3_2_dialog_1_A2");  
                list.Add("cmMerc3_2_dialog_1_M3");   
                list.Add("cmMerc3_2_dialog_1_A3");  
                list.Add("cmMerc3_2_dialog_1_M4");   
                list.Add("DC");
                break;
            case 2:
                list.Add("cmMerc3_2_dialog_2_M1");      
                list.Add("DC");    
                list.Add("cmMerc3_2_dialog_2_M2");      
                list.Add("cmMerc3_2_dialog_2_A2");    
                list.Add("cmMerc3_2_dialog_2_M3");      
                list.Add("cmMerc3_2_dialog_2_A3");    
                list.Add("cmMerc3_2_dialog_2_M4");      
                list.Add("cmMerc3_2_dialog_2_A4");
                break;
            case 3:
                list.Add("cmMerc3_2_dialog_3_M1");     
                list.Add("DC");   
                list.Add("cmMerc3_2_dialog_3_M2");     
                list.Add("cmMerc3_2_dialog_3_A2");   
                list.Add("cmMerc3_2_dialog_3_M3");     
                list.Add("cmMerc3_2_dialog_3_A3");   
                list.Add("cmMerc3_2_dialog_3_M4");     
                list.Add("cmMerc3_2_dialog_3_A4");   
                list.Add("cmMerc3_2_dialog_3_M5");     
                list.Add("cmMerc3_2_dialog_3_A5");   
                list.Add("cmMerc3_2_dialog_3_M6");     
                list.Add("cmMerc3_2_dialog_3_A6");
                break;
            case 4:
                list.Add("cmMerc3_2_dialog_4_M1");     
                list.Add("cmMerc3_2_dialog_4_A1"); 
                list.Add("cmMerc3_2_dialog_4_M2");     
                list.Add("cmMerc3_2_dialog_4_A2"); 
                list.Add("cmMerc3_2_dialog_4_M3");     
                list.Add("cmMerc3_2_dialog_4_A3"); 
                list.Add("cmMerc3_2_dialog_4_M4");     
                list.Add("cmMerc3_2_dialog_4_A4"); 
                list.Add("cmMerc3_2_dialog_4_M5");     
                list.Add("DC"); 
                list.Add("cmMerc3_2_dialog_4_M6");     
                list.Add("cmMerc3_2_dialog_4_A6");
                break;
            case 5:
                list.Add("cmMerc3_2_dialog_5_M1");      
                list.Add("DC"); 
                list.Add("cmMerc3_2_dialog_5_M2");      
                list.Add("cmMerc3_2_dialog_5_A2"); 
                list.Add("cmMerc3_2_dialog_5_M3");      
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
        return $"{Namings.Tag("cmComeToPoint")}";
    }
}
