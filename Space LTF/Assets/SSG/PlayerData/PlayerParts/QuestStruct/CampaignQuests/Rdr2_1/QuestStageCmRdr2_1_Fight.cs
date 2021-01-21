using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr2_1_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr2_1_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig == ShipConfig.federation).ToList().RandomElement();
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_1_Fight + _id.ToString());
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
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 4, 
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
                list.Add("cmRdr2_1_dialog_3_M1");
                list.Add("cmRdr2_1_dialog_3_A1");
                list.Add("cmRdr2_1_dialog_3_M2");
                list.Add("cmRdr2_1_dialog_3_A3");
                list.Add("cmRdr2_1_dialog_3_M4");
                list.Add("cmRdr2_1_dialog_3_A5");
                list.Add("cmRdr2_1_dialog_3_M6");
                list.Add("cmRdr2_1_dialog_3_A7");
                break;
            default: 
                list = GetDialogsTagAttack();
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
                list.Add("cmRdr2_1_dialog_4_M1");
                list.Add("cmRdr2_1_dialog_4_A1");
                list.Add("cmRdr2_1_dialog_4_M2");
                list.Add("cmRdr2_1_dialog_4_A3");
                list.Add("cmRdr2_1_dialog_4_M4");
                list.Add("cmRdr2_1_dialog_4_A5");
                list.Add("cmRdr2_1_dialog_4_M6");
                list.Add("cmRdr2_1_dialog_4_A7");
                list.Add("cmRdr2_1_dialog_4_M8");
                list.Add("cmRdr2_1_dialog_4_A9");
                break;
            case 2:
                list.Add("cmRdr2_1_dialog_5_M1");
                list.Add("cmRdr2_1_dialog_5_A1");

                break;
            case 3:
                list.Add("cmRdr2_1_dialog_6_M1");
                list.Add("cmRdr2_1_dialog_6_A1");
                list.Add("cmRdr2_1_dialog_6_M2");
                list.Add("DC");
                list.Add("cmRdr2_1_dialog_6_M3");
                list.Add("cmRdr2_1_dialog_6_A4");
                list.Add("cmRdr2_1_dialog_6_M5");
                list.Add("DC");
                list.Add("cmRdr2_1_dialog_6_M6");
                list.Add("cmRdr2_1_dialog_6_A7");
                list.Add("cmRdr2_1_dialog_6_M8");
                list.Add("cmRdr2_1_dialog_6_A9");
                list.Add("cmRdr2_1_dialog_6_M10");
                list.Add("cmRdr2_1_dialog_6_A11");
                list.Add("cmRdr2_1_dialog_6_M12");
                list.Add("cmRdr2_1_dialog_6_A13");
                list.Add("cmRdr2_1_dialog_6_M14");
                list.Add("DC");
                list.Add("cmRdr2_1_dialog_6_M15");
                list.Add("cmRdr2_1_dialog_6_A16");
                list.Add("cmRdr2_1_dialog_6_M17");
                list.Add("DC");
                list.Add("cmRdr2_1_dialog_6_M18");
                list.Add("cmRdr2_1_dialog_6_A19");

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
