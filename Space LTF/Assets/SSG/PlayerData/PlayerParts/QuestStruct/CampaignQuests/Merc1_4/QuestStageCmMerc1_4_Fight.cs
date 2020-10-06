using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_4_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc1_4_Fight()    
        :base(QuestsLib.CmMerc1_4_Fight)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);

        cell1 = FindAndMarkCellFarest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc1_4_Fight);
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
        var playerEnemy = new PlayerAIWithBattleEvent("Ocrs", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 4, 
            ArmyCreatorLibrary.GetArmy(ShipConfig.mercenary), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc1_4_dialog_1_M0");
        list.Add("cmMerc1_4_dialog_1_A0");   
        list.Add("cmMerc1_4_dialog_1_M1");
        list.Add("Attack");
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmMerc1_4_dialog_3_M0");
        list.Add("cmMerc1_4_dialog_3_A0");  
        list.Add("cmMerc1_4_dialog_3_M1");
        list.Add("cmMerc1_4_dialog_3_A1");  
        list.Add("cmMerc1_4_dialog_3_M2");
        list.Add("cmMerc1_4_dialog_3_A2");  
        list.Add("cmMerc1_4_dialog_3_M3");
        list.Add("cmMerc1_4_dialog_3_A3");  
        list.Add("cmMerc1_4_dialog_3_M4");
        list.Add("cmMerc1_4_dialog_3_A4");  
        list.Add("cmMerc1_4_dialog_3_M5");
        list.Add("DC");
        list.Add("cmMerc1_4_dialog_3_M5_1");
        list.Add("DC");
        list.Add("cmMerc1_4_dialog_3_M5_2");
        list.Add("cmMerc1_4_dialog_3_A5");  
        list.Add("cmMerc1_4_dialog_3_M6");
        list.Add("cmMerc1_4_dialog_3_A6");  
        list.Add("cmMerc1_4_dialog_3_M7");
        list.Add("cmMerc1_4_dialog_3_A7");  
        list.Add("cmMerc1_4_dialog_3_M8");
        list.Add("cmMerc1_4_dialog_3_A8");  
        list.Add("cmMerc1_4_dialog_3_M9");
        list.Add("DC");
        list.Add("cmMerc1_4_dialog_3_M9_2");
        list.Add("cmMerc1_4_dialog_3_A9");  
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
