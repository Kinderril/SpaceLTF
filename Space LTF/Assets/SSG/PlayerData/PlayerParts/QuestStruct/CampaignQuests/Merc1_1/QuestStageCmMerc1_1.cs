using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index = 0;

    public QuestStageCmMerc1_1(int index)    
        :base( $"{QuestsLib.CM_MERC_1_1}{index}")
    {
        _index = index;
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

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        switch (_index)
        {
            case 1:
                return () => DialogsLibrary.GetPairDialogByTag(GetDialogsTagLeave1(),null);
            case 2:
                return () => DialogsLibrary.GetPairDialogByTag(GetDialogsTagLeave2(), null);
            case 3:
                return () => DialogsLibrary.GetPairDialogByTag(GetDialogsTagLeave3(), null);
        }

        return null;
    }

    private List<string> GetDialogsTagLeave1()
    {

        var list = new List<string>();
        list.Add("cmMerc1_1_dialog_1_M1");
        list.Add("cmMerc1_1_dialog_1_A1");  
        list.Add("cmMerc1_1_dialog_1_M2");
        list.Add("cmMerc1_1_dialog_1_A2");  
//        list.Add("cmMerc1_1_dialog_1_M3");
//        list.Add("cmMerc1_1_dialog_1_A3");
        return list;
    }
           
    private List<string> GetDialogsTagLeave2()
    {
        var list = new List<string>();
        list.Add("cmMerc1_1_dialog_2_M1");
        list.Add("cmMerc1_1_dialog_2_A1");  
        list.Add("cmMerc1_1_dialog_2_M2");
        list.Add("cmMerc1_1_dialog_2_A2");
        return list;
    }    
    private List<string> GetDialogsTagLeave3()
    {
        var list = new List<string>();
        list.Add("cmMerc1_1_dialog_3_M1");
        list.Add("cmMerc1_1_dialog_3_A1");  
        list.Add("cmMerc1_1_dialog_3_M2");
        list.Add("cmMerc1_1_dialog_3_A2");  
        list.Add("cmMerc1_1_dialog_3_M3");
        list.Add("cmMerc1_1_dialog_3_A3");  
        list.Add("cmMerc1_1_dialog_3_M4");
        list.Add("cmMerc1_1_dialog_3_A4");  
        list.Add("cmMerc1_1_dialog_3_M5");
        list.Add("cmMerc1_1_dialog_3_A5");  
        list.Add("cmMerc1_1_dialog_3_M6");
        list.Add("cmMerc1_1_dialog_3_A6");
        return list;
    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        var id = $"{QuestsLib.CM_MERC_1_1}{_index}";
        _playerQuest.QuestIdComplete(id);
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
        var power = cell1.Power* 0.8f;
        var playerEnemy = new PlayerAIWithBattleEvent("Mercs", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 3, ArmyCreatorLibrary.GetArmy(ShipConfig.mercenary), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("dialog_armyShallFight");
        list.Add("dialog_Attack"); 
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
        return $"{Namings.Tag("cmMerc1_1_destroy")}";
    }
}
