
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmOcr3_1_Fight(int id)    
        :base(QuestsLib.QuestStageCmOcr3_1_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_1_Fight + _id.ToString());
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
        var power = cell1.Power;
        ShipConfig config = ShipConfig.federation;
        switch (_id)
        {
            case 1:
//                power =(int) (power * 0.7f);
                config = ShipConfig.federation;
                break;
            case 2:
//                power = (int)(power * 0.8f);
                config = ShipConfig.mercenary;
                break;
        }
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 3, 6, 
            ArmyCreatorLibrary.GetArmy(config), true,
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
                list = GetDialogsTagAttack();
                break;
            case 2:
                list = GetDialogsTagAttack();
                break;
            case 3:
                list.Add("cmOcr3_1_dialog_3_M1");
                list.Add("cmOcr3_1_dialog_3_A1");   
                list.Add("cmOcr3_1_dialog_3_M2");
                list.Add("cmOcr3_1_dialog_3_A2");   
                list.Add("cmOcr3_1_dialog_3_M3");
                list.Add("cmOcr3_1_dialog_3_A3");   
                list.Add("cmOcr3_1_dialog_3_M4");
                list.Add("cmOcr3_1_dialog_3_A4");    
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
                list.Add("cmOcr3_1_dialog_2_M1");
                list.Add("cmOcr3_1_dialog_2_A1");    
                list.Add("cmOcr3_1_dialog_2_M2");
                list.Add("cmOcr3_1_dialog_2_A2");    
                list.Add("cmOcr3_1_dialog_2_M3");
                list.Add("cmOcr3_1_dialog_2_A3");    
                list.Add("cmOcr3_1_dialog_2_M4");
                list.Add("cmOcr3_1_dialog_2_A4");
                var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
                return () => dialog;
                break;
            case 3:
                list.Add("cmOcr3_1_dialog_5_M1");
                list.Add("cmOcr3_1_dialog_5_A1");  
                list.Add("cmOcr3_1_dialog_5_M2");
                list.Add("cmOcr3_1_dialog_5_A2");  
                list.Add("cmOcr3_1_dialog_5_M3");
                list.Add("cmOcr3_1_dialog_5_A3");  
                list.Add("cmOcr3_1_dialog_5_M4");
                list.Add("cmOcr3_1_dialog_5_A4");  
                list.Add("cmOcr3_1_dialog_5_M5");
                list.Add("cmOcr3_1_dialog_5_A5");  
                list.Add("cmOcr3_1_dialog_5_M6");
                list.Add("cmOcr3_1_dialog_5_A6");  
                list.Add("cmOcr3_1_dialog_5_M7");
                list.Add("cmOcr3_1_dialog_5_A7");  
                list.Add("cmOcr3_1_dialog_5_M8");
                list.Add("cmOcr3_1_dialog_5_A8");  
                list.Add("cmOcr3_1_dialog_5_M9");
                list.Add("cmOcr3_1_dialog_5_A9");  
                list.Add("cmOcr3_1_dialog_5_M10");
                list.Add("cmOcr3_1_dialog_5_A10");  
                list.Add("cmOcr3_1_dialog_5_M11");
                list.Add("cmOcr3_1_dialog_5_A11");  
                list.Add("cmOcr3_1_dialog_5_M12");
                list.Add("cmOcr3_1_dialog_5_A12");  
                list.Add("cmOcr3_1_dialog_5_M13");
                list.Add("cmOcr3_1_dialog_5_A13");  
                list.Add("cmOcr3_1_dialog_5_M14");
                list.Add("cmOcr3_1_dialog_5_A14");
                var dialog1 = DialogsLibrary.GetPairDialogByTag(list, null);
                return () => dialog1;
                break;
        }

        return null;
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
