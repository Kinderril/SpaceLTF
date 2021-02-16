using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr3_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr3_1_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr3_1_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig == ShipConfig.raiders).ToList().RandomElement();
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr3_1_Fight + _id.ToString());
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
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var data = ArmyCreatorLibrary.GetArmy(ShipConfig.federation, ShipConfig.krios);
        switch (_id)
        {
            case 1:
                data = ArmyCreatorLibrary.GetArmy(ShipConfig.federation);
                break;
            case 2:
                data = ArmyCreatorLibrary.GetArmy(ShipConfig.federation, ShipConfig.krios);
                break;
            case 3:
                data = ArmyCreatorLibrary.GetArmy(ShipConfig.federation, ShipConfig.krios);
                break;
        }

        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 4, 7, data, false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        return GetDialogsTagAttack();
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr3_1_dialog_2_M0");
                list.Add("cmRdr3_1_dialog_2_A1");
                list.Add("cmRdr3_1_dialog_2_M2");
                list.Add("cmRdr3_1_dialog_2_A3");
                list.Add("cmRdr3_1_dialog_2_M4");
                list.Add("cmRdr3_1_dialog_2_A5");
                list.Add("cmRdr3_1_dialog_2_M6");
                list.Add("cmRdr3_1_dialog_2_A7");
                list.Add("cmRdr3_1_dialog_2_M8");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_2_M9");
                list.Add("cmRdr3_1_dialog_2_A10");
                list.Add("cmRdr3_1_dialog_2_M11");
                list.Add("cmRdr3_1_dialog_2_A12");

                break;
            case 2:
                list.Add("cmRdr3_1_dialog_3_M0");
                list.Add("cmRdr3_1_dialog_3_A1");
                list.Add("cmRdr3_1_dialog_3_M2");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_3_M3");
                list.Add("cmRdr3_1_dialog_3_A4");
                list.Add("cmRdr3_1_dialog_3_M5");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_3_M6");
                list.Add("cmRdr3_1_dialog_3_A7");
                break;  
            case 3:
                list.Add("cmRdr3_1_dialog_4_M0");
                list.Add("cmRdr3_1_dialog_4_A1");
                list.Add("cmRdr3_1_dialog_4_M2");
                list.Add("cmRdr3_1_dialog_4_A3");
                list.Add("cmRdr3_1_dialog_4_M4");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_4_M5");
                list.Add("cmRdr3_1_dialog_4_A6");

                break;  
            case 4:
                list.Add("cmRdr3_1_dialog_5_M0");
                list.Add("cmRdr3_1_dialog_5_A1");
                list.Add("cmRdr3_1_dialog_5_M2");
                list.Add("cmRdr3_1_dialog_5_A3");
                list.Add("cmRdr3_1_dialog_5_M4");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_5_M5");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_5_M6");
                list.Add("DC");
                list.Add("cmRdr3_1_dialog_5_M7");
                list.Add("cmRdr3_1_dialog_5_A8");
                list.Add("cmRdr3_1_dialog_5_M9");
                list.Add("cmRdr3_1_dialog_5_A10");
                list.Add("cmRdr3_1_dialog_5_M11");
                list.Add("cmRdr3_1_dialog_5_A12");
                list.Add("cmRdr3_1_dialog_5_M13");
                list.Add("cmRdr3_1_dialog_5_A14");

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
