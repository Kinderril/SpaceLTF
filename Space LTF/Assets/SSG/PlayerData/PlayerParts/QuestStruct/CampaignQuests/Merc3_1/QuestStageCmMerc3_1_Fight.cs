using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc3_1_Fight()    
        :base(QuestsLib.CmMerc3_1_Fight)
    {

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
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc3_1_Fight);
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
        var btd = new BattleTypeData(EBattleType.defenceOfShip);
        var playerEnemy = new PlayerAIWithBattleEvent("Army", false, btd);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6, 
            ArmyCreatorLibrary.GetArmy(ShipConfig.mercenary), true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc3_1_dialog_2_M1");
        list.Add("DC");  
        list.Add("cmMerc3_1_dialog_2_M2");
        list.Add("cmMerc3_1_dialog_2_A2");  
        list.Add("cmMerc3_1_dialog_2_M3");
        list.Add("DC");  
        list.Add("cmMerc3_1_dialog_2_M4");
        list.Add("cmMerc3_1_dialog_2_A4");  
        list.Add("cmMerc3_1_dialog_2_M5");
        list.Add("cmMerc3_1_dialog_2_A5");   
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmMerc3_1_dialog_3_M1");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M2");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M3");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M4");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M5");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M6");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M7");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M8");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M9");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M10");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M11");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M12");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M13");
        list.Add("cmMerc3_1_dialog_3_A13");   
        list.Add("cmMerc3_1_dialog_3_M14");
        list.Add("DC");   
        list.Add("cmMerc3_1_dialog_3_M15");
        list.Add("cmMerc3_1_dialog_3_A15");   
        list.Add("cmMerc3_1_dialog_3_M16");
        list.Add("cmMerc3_1_dialog_3_A16");  
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return  ()=>dialog;
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
