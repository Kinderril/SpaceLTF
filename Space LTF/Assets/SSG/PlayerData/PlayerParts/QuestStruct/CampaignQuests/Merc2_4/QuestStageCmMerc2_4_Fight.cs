using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_4_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_4_Fight()    
        :base(QuestsLib.CmMerc2_4_Fight)
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
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc2_4_Fight);
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
        var playerEnemy = new PlayerAIWithBattleEvent("Army", false,new BattleTypeData(EBattleType.defenceOfShip));
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6, 
            ArmyCreatorLibrary.GetArmy(ShipConfig.krios,ShipConfig.federation), true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_4_dialog_5_M1");
        list.Add("cmMerc2_4_dialog_5_A1");      
        list.Add("cmMerc2_4_dialog_5_M2");
        list.Add("DC");      
        list.Add("cmMerc2_4_dialog_5_M3");
        list.Add("cmMerc2_4_dialog_5_A3");      
        list.Add("cmMerc2_4_dialog_5_M4");
        list.Add("DC");      
        list.Add("cmMerc2_4_dialog_5_M5");
        list.Add("cmMerc2_4_dialog_5_A5");      
        list.Add("cmMerc2_4_dialog_5_M6");
        list.Add("cmMerc2_4_dialog_5_A6");      
        list.Add("cmMerc2_4_dialog_5_M7");
        list.Add("DC");      
        list.Add("cmMerc2_4_dialog_5_M8");
        list.Add("cmMerc2_4_dialog_5_A8");      
        list.Add("cmMerc2_4_dialog_5_M9");
        list.Add("cmMerc2_4_dialog_5_A9");      
        list.Add("cmMerc2_4_dialog_5_M10");
        list.Add("DC");      
        list.Add("cmMerc2_4_dialog_5_M11");
        list.Add("cmMerc2_4_dialog_5_A11");      
        list.Add("cmMerc2_4_dialog_5_M12");
        list.Add("cmMerc2_4_dialog_5_A12");   
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmMerc2_4_dialog_6_M0");
        list.Add("cmMerc2_4_dialog_6_A0");       
        list.Add("cmMerc2_4_dialog_6_M1");
        list.Add("DC");       
        list.Add("cmMerc2_4_dialog_6_M2");
        list.Add("cmMerc2_4_dialog_6_A2");       
        list.Add("cmMerc2_4_dialog_6_M3");
        list.Add("cmMerc2_4_dialog_6_A3");       
        list.Add("cmMerc2_4_dialog_6_M4");
        list.Add("DC");       
        list.Add("cmMerc2_4_dialog_6_M5");
        list.Add("DC");       
        list.Add("cmMerc2_4_dialog_6_M6");
        list.Add("cmMerc2_4_dialog_6_A6");       
        list.Add("cmMerc2_4_dialog_6_M7");
        list.Add("cmMerc2_4_dialog_6_A7");       
        list.Add("cmMerc2_4_dialog_6_M8");
        list.Add("cmMerc2_4_dialog_6_A8");  
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
