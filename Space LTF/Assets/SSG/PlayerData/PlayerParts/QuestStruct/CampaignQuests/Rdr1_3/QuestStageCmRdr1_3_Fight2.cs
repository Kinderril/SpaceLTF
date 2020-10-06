using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_3_Fight2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr1_3_Fight2(int id)    
        :base(QuestsLib.QuestStageCmRdr1_3_Fight2 + id.ToString())
    {
        _id = id;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_3_Fight2 + _id.ToString());
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
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false, new BattleTypeData(1,1,ShipConfig.raiders,ShipConfig.droid));
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 4, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        if (_id == 1)
        {
            list.Add("cmRdr1_3_dialog_4_M1");
            list.Add("cmRdr1_3_dialog_4_A1");
            list.Add("cmRdr1_3_dialog_4_M2");
            list.Add("cmRdr1_3_dialog_4_A2");
            list.Add("cmRdr1_3_dialog_4_M3");
            list.Add("DC");
            list.Add("cmRdr1_3_dialog_4_M4");
            list.Add("cmRdr1_3_dialog_4_A4");
            list.Add("cmRdr1_3_dialog_4_M5");
            list.Add("cmRdr1_3_dialog_4_A5");
            list.Add("cmRdr1_3_dialog_4_M6");
            list.Add("cmRdr1_3_dialog_4_A6");
        }
        else
        {

            list.Add("cmRdr1_3_dialog_6_M1");
            list.Add("cmRdr1_3_dialog_6_A1");    
            list.Add("cmRdr1_3_dialog_6_M2");
            list.Add("cmRdr1_3_dialog_6_A2");    
            list.Add("cmRdr1_3_dialog_6_M3");
            list.Add("cmRdr1_3_dialog_6_A3");    
            list.Add("cmRdr1_3_dialog_6_M4");
            list.Add("cmRdr1_3_dialog_6_A4");    
            list.Add("cmRdr1_3_dialog_6_M5");
            list.Add("cmRdr1_3_dialog_6_A5");    
            list.Add("cmRdr1_3_dialog_6_M6");
//            list.Add("cmRdr1_3_dialog_6_A6");    
            list.Add("cmRdr1_3_dialog_6_M7");
            list.Add("cmRdr1_3_dialog_6_A7");    
            list.Add("cmRdr1_3_dialog_6_M8");
//            list.Add("cmRdr1_3_dialog_6_A8");    
            list.Add("cmRdr1_3_dialog_6_M9");
            list.Add("cmRdr1_3_dialog_6_A9");    
            list.Add("cmRdr1_3_dialog_6_M10");
            list.Add("cmRdr1_3_dialog_6_A10");
        } 
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        if (_id == 1)
        {
            list.Add("cmRdr1_3_dialog_5_M1");
            list.Add("cmRdr1_3_dialog_5_A1");
            list.Add("cmRdr1_3_dialog_5_M2");
            list.Add("DC");
            list.Add("cmRdr1_3_dialog_5_M3");
            list.Add("cmRdr1_3_dialog_5_A3");
            list.Add("cmRdr1_3_dialog_5_M4");
            list.Add("cmRdr1_3_dialog_5_A4");
            list.Add("cmRdr1_3_dialog_5_M5");
            list.Add("cmRdr1_3_dialog_5_A5");
        }
        else
        {
            list.Add("cmRdr1_3_dialog_7_M1");
            list.Add("cmRdr1_3_dialog_7_A1");   
            list.Add("cmRdr1_3_dialog_7_M2");
            list.Add("cmRdr1_3_dialog_7_A2");   
            list.Add("cmRdr1_3_dialog_7_M3");
            list.Add("cmRdr1_3_dialog_7_A3");   
            list.Add("cmRdr1_3_dialog_7_M4");
            list.Add("cmRdr1_3_dialog_7_A4");   
            list.Add("cmRdr1_3_dialog_7_M5");
            list.Add("cmRdr1_3_dialog_7_A5");   
            list.Add("cmRdr1_3_dialog_7_M6");
            list.Add("DC");
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
