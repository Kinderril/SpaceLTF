using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr1_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmOcr1_1_Fight(int id)    
        :base(QuestsLib.QuestStageCmOcr1_1_Fight + id.ToString())
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr1_1_Fight + _id.ToString());
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
                power =(int) (power * 0.7f);
                config = ShipConfig.federation;
                break;
            case 2:
                power = (int)(power * 0.8f);
                config = ShipConfig.federation;
                break;
            case 3:
                power = (int)(power * 1.1f);
                config = ShipConfig.raiders;
                break;
        }
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
                list.Add("cmOcr1_1_dialog_2_M1");
                list.Add("cmOcr1_1_dialog_2_A1");
                list.Add("cmOcr1_1_dialog_2_M2");
                list.Add("cmOcr1_1_dialog_2_A2");
                list.Add("cmOcr1_1_dialog_2_M3");
                list.Add("DC");
                break;
            case 2:
                list.Add("DC");
                list.Add("cmOcr1_1_dialog_3_A1");  
                list.Add("cmOcr1_1_dialog_3_M2");
                list.Add("cmOcr1_1_dialog_3_A2");  
                list.Add("cmOcr1_1_dialog_3_M3");
                list.Add("DC");  
                list.Add("cmOcr1_1_dialog_3_M4");
                list.Add("cmOcr1_1_dialog_3_A4");  
                list.Add("cmOcr1_1_dialog_3_M5");
                list.Add("DC");  
                list.Add("cmOcr1_1_dialog_3_M6");
                list.Add("cmOcr1_1_dialog_3_A6");
                break;
            case 3:
                list.Add("cmOcr1_1_dialog_4_M1");
                list.Add("cmOcr1_1_dialog_4_A1");
                list.Add("cmOcr1_1_dialog_4_M2");
                list.Add("cmOcr1_1_dialog_4_A2");
                list.Add("cmOcr1_1_dialog_4_M3");
                list.Add("cmOcr1_1_dialog_4_A3");
                list.Add("cmOcr1_1_dialog_4_M4");
                list.Add("cmOcr1_1_dialog_4_A4");
                list.Add("cmOcr1_1_dialog_4_M5");
                list.Add("cmOcr1_1_dialog_4_A5");
                list.Add("cmOcr1_1_dialog_4_M6");
                list.Add("cmOcr1_1_dialog_4_A6");
                break;
        } 
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        if (_id == 3)
        {
            var list = new List<string>();
            list.Add("cmOcr1_1_dialog_5_M1");
            list.Add("cmOcr1_1_dialog_5_A1"); 
            list.Add("cmOcr1_1_dialog_5_M2");
            list.Add("cmOcr1_1_dialog_5_A2"); 
            list.Add("cmOcr1_1_dialog_5_M3");
            list.Add("cmOcr1_1_dialog_5_A3");
            var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
            return () => dialog;
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
