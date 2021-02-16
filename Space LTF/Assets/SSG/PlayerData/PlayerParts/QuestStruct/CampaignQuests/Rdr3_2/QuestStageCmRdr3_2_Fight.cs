using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr3_2_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr3_2_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr3_2_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig !=  ShipConfig.raiders && x.ShipConfig != ShipConfig.droid && !x.IsHide).ToList().RandomElement();
        cell1 = FindAndMarkCellClosest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr3_2_Fight + _id.ToString());
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
        switch (_id)
        {
            case 1:
                list.Add("cmRdr3_2_dialog_1_M0");
                list.Add("cmRdr3_2_dialog_1_A1");
                list.Add("cmRdr3_2_dialog_1_M2");
                list.Add("cmRdr3_2_dialog_1_A3");
                list.Add("cmRdr3_2_dialog_1_M4");
                list.Add("cmRdr3_2_dialog_1_A5");
                list.Add("cmRdr3_2_dialog_1_M6");
                list.Add("cmRdr3_2_dialog_1_A7");
                list.Add("cmRdr3_2_dialog_1_M8");
                list.Add("cmRdr3_2_dialog_1_A9");
                list.Add("cmRdr3_2_dialog_1_M10");
                list.Add("cmRdr3_2_dialog_1_A11");
                list.Add("cmRdr3_2_dialog_1_M12");
                list.Add("cmRdr3_2_dialog_1_A13");
                list.Add("cmRdr3_2_dialog_1_M14");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_1_M15");
                list.Add("cmRdr3_2_dialog_1_A16");
                list.Add("cmRdr3_2_dialog_1_M17");
                list.Add("cmRdr3_2_dialog_1_A18");
                list.Add("cmRdr3_2_dialog_1_M19");
                list.Add("cmRdr3_2_dialog_1_A20");
                list.Add("cmRdr3_2_dialog_1_M21");
                list.Add("cmRdr3_2_dialog_1_A22");
                break;
            case 2:
                list.Add("cmRdr3_2_dialog_3_M0");
                list.Add("cmRdr3_2_dialog_3_A1");
                list.Add("cmRdr3_2_dialog_3_M2");
                list.Add("cmRdr3_2_dialog_3_A3");
                list.Add("cmRdr3_2_dialog_3_M4");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_3_M5");
                list.Add("cmRdr3_2_dialog_3_A6");
                list.Add("cmRdr3_2_dialog_3_M7");
                list.Add("cmRdr3_2_dialog_3_A8");
                list.Add("cmRdr3_2_dialog_3_M9");
                list.Add("cmRdr3_2_dialog_3_A10");
                list.Add("cmRdr3_2_dialog_3_M11");
                list.Add("cmRdr3_2_dialog_3_A12");
                list.Add("cmRdr3_2_dialog_3_M13");
                list.Add("cmRdr3_2_dialog_3_A14");
                list.Add("cmRdr3_2_dialog_3_M15");
                list.Add("cmRdr3_2_dialog_3_A16");

                break;
            case 3:
                list.Add("cmRdr3_2_dialog_5_M0");
                list.Add("cmRdr3_2_dialog_5_A1");
                list.Add("cmRdr3_2_dialog_5_M2");
                list.Add("cmRdr3_2_dialog_5_A3");
                list.Add("cmRdr3_2_dialog_5_M4");
                list.Add("cmRdr3_2_dialog_5_A5");
                list.Add("cmRdr3_2_dialog_5_M6");
                list.Add("cmRdr3_2_dialog_5_A7");
                list.Add("cmRdr3_2_dialog_5_M8");
                list.Add("cmRdr3_2_dialog_5_A9");

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
                list.Add("cmRdr3_2_dialog_2_M0");
                list.Add("cmRdr3_2_dialog_2_A1");


                break;
            case 2:
                list.Add("cmRdr3_2_dialog_4_M0");
                list.Add("cmRdr3_2_dialog_4_A1");
                list.Add("cmRdr3_2_dialog_4_M2");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M3");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M4");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M5");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M6");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M7");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_4_M8");
                list.Add("cmRdr3_2_dialog_4_A9");
                list.Add("cmRdr3_2_dialog_4_M10");
                list.Add("cmRdr3_2_dialog_4_A11");
                list.Add("cmRdr3_2_dialog_4_M12");
                list.Add("cmRdr3_2_dialog_4_A13");

                break;
            case 3:
                list.Add("cmRdr3_2_dialog_6_M0");
                list.Add("cmRdr3_2_dialog_6_A1");
                list.Add("cmRdr3_2_dialog_6_M2");
                list.Add("cmRdr3_2_dialog_6_A3");
                list.Add("cmRdr3_2_dialog_6_M4");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M5");
                list.Add("cmRdr3_2_dialog_6_A6");
                list.Add("cmRdr3_2_dialog_6_M7");
                list.Add("cmRdr3_2_dialog_6_A8");
                list.Add("cmRdr3_2_dialog_6_M9");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M10");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M11");
                list.Add("cmRdr3_2_dialog_6_A12");
                list.Add("cmRdr3_2_dialog_6_M13");
                list.Add("cmRdr3_2_dialog_6_A14");
                list.Add("cmRdr3_2_dialog_6_M15");
                list.Add("cmRdr3_2_dialog_6_A16");
                list.Add("cmRdr3_2_dialog_6_M17");
                list.Add("cmRdr3_2_dialog_6_A18");
                list.Add("cmRdr3_2_dialog_6_M19");
                list.Add("cmRdr3_2_dialog_6_A20");
                list.Add("cmRdr3_2_dialog_6_M21");
                list.Add("cmRdr3_2_dialog_6_A22");
                list.Add("cmRdr3_2_dialog_6_M23");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M24");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M25");
                list.Add("DC");
                list.Add("cmRdr3_2_dialog_6_M26");
                list.Add("cmRdr3_2_dialog_6_A27");
                list.Add("cmRdr3_2_dialog_6_M28");
                list.Add("cmRdr3_2_dialog_6_A29");
                list.Add("cmRdr3_2_dialog_6_M30");
                list.Add("cmRdr3_2_dialog_6_A31");
                list.Add("cmRdr3_2_dialog_6_M32");
                list.Add("cmRdr3_2_dialog_6_A33");
                list.Add("cmRdr3_2_dialog_6_M34");
                list.Add("cmRdr3_2_dialog_6_A35");
                list.Add("cmRdr3_2_dialog_6_M36");
                list.Add("cmRdr3_2_dialog_6_A37");
                list.Add("cmRdr3_2_dialog_6_M38");
                list.Add("cmRdr3_2_dialog_6_A39");

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
