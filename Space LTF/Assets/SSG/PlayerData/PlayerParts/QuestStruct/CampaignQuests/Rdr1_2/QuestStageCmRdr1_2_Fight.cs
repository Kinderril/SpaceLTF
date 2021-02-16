using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_2_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr1_2_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr1_2_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.raiders);

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_2_Fight + _id.ToString());
        Fight();
    }

    private void Fight()
    {
        cell1.SetQuestData(null);
        TextChangeEvent();
        MainController.Instance.PreBattle(_player, PlayerToDefeat(), false, false);

    }

    public Player PlayerToDefeat()
    {
        var power = cell1.Power*(_id == 3?1.2f:0.8f);
        ShipConfig config = ShipConfig.raiders;
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 3, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr1_2_dialog_1_M1");
                list.Add("cmRdr1_2_dialog_1_A1");  
                list.Add("cmRdr1_2_dialog_1_M2");
                list.Add("cmRdr1_2_dialog_1_A2");  
                list.Add("cmRdr1_2_dialog_1_M3");
                list.Add("cmRdr1_2_dialog_1_A3");  
                list.Add("cmRdr1_2_dialog_1_M4");
                list.Add("cmRdr1_2_dialog_1_A4");
                break; 
            case 2:
                list.Add("cmRdr1_2_dialog_3_M1");
                list.Add("cmRdr1_2_dialog_3_A1");  
                list.Add("cmRdr1_2_dialog_3_M2");
                list.Add("cmRdr1_2_dialog_3_A2");
                break; 
            case 3:
                list.Add("cmRdr1_2_dialog_5_M1");
                list.Add("cmRdr1_2_dialog_5_A1");   
                list.Add("cmRdr1_2_dialog_5_M2");
                list.Add("cmRdr1_2_dialog_5_A2");   
                list.Add("cmRdr1_2_dialog_5_M3");
                list.Add("cmRdr1_2_dialog_5_A3");   
                list.Add("cmRdr1_2_dialog_5_M4");
                list.Add("cmRdr1_2_dialog_5_A4");   
                list.Add("cmRdr1_2_dialog_5_M5");
//                list.Add("cmRdr1_2_dialog_5_A5");   
                list.Add("cmRdr1_2_dialog_5_M6");
//                list.Add("cmRdr1_2_dialog_5_A6");   
                list.Add("cmRdr1_2_dialog_5_M7");
                list.Add("cmRdr1_2_dialog_5_A7");   
                list.Add("cmRdr1_2_dialog_5_M8");
                list.Add("cmRdr1_2_dialog_5_A8");
                break;
        }
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr1_2_dialog_2_M1");
//                list.Add("cmRdr1_2_dialog_2_A1");    
                list.Add("cmRdr1_2_dialog_2_M2");
                list.Add("cmRdr1_2_dialog_2_A2");    
                list.Add("cmRdr1_2_dialog_2_M3");
                list.Add("cmRdr1_2_dialog_2_A3");    
                list.Add("cmRdr1_2_dialog_2_M4");
                list.Add("cmRdr1_2_dialog_2_A4");    
                list.Add("cmRdr1_2_dialog_2_M5");
                list.Add("cmRdr1_2_dialog_2_A5");    
                list.Add("cmRdr1_2_dialog_2_M6");
                list.Add("cmRdr1_2_dialog_2_A6");   
                list.Add("DC");
                break;
            case 2:
                list.Add("cmRdr1_2_dialog_4_M1");
                list.Add("DC");   
                list.Add("cmRdr1_2_dialog_4_M2");
                list.Add("cmRdr1_2_dialog_4_A2");   
                list.Add("cmRdr1_2_dialog_4_M3");
                list.Add("cmRdr1_2_dialog_4_A3");   
                list.Add("cmRdr1_2_dialog_4_M4");
                list.Add("cmRdr1_2_dialog_4_A4");   
                list.Add("cmRdr1_2_dialog_4_M5");
                list.Add("DC");   
                list.Add("cmRdr1_2_dialog_4_M6");
                list.Add("DC");   
                list.Add("cmRdr1_2_dialog_4_M7");
                list.Add("cmRdr1_2_dialog_4_A7");   
                list.Add("cmRdr1_2_dialog_4_M8");
                list.Add("cmRdr1_2_dialog_4_A8");   
                list.Add("cmRdr1_2_dialog_4_M9");
                list.Add("cmRdr1_2_dialog_4_A9");   
                list.Add("cmRdr1_2_dialog_4_M10");
                list.Add("cmRdr1_2_dialog_4_A10");   
                list.Add("cmRdr1_2_dialog_4_M11");
                list.Add("cmRdr1_2_dialog_4_A11");
                break;
            case 3:
                list.Add("cmRdr1_2_dialog_6_M1");
                list.Add("cmRdr1_2_dialog_6_A1"); 
                list.Add("cmRdr1_2_dialog_6_M2");
                list.Add("cmRdr1_2_dialog_6_A2"); 
                list.Add("cmRdr1_2_dialog_6_M3");
                list.Add("cmRdr1_2_dialog_6_A3"); 
                list.Add("cmRdr1_2_dialog_6_M4");
                list.Add("cmRdr1_2_dialog_6_A4"); 
                list.Add("cmRdr1_2_dialog_6_M5");
                list.Add("cmRdr1_2_dialog_6_A5");
                break;
        }    
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return () => dialog;
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
        return $"{Namings.Tag("cmStartAttackQuestDesc")}";
    }
}
