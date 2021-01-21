using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_3_Fight3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr2_3_Fight3(int id)    
        :base(QuestsLib.QuestStageCmRdr2_3_Fight3 + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.CurrentCell.Sector;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_3_Fight3 + _id.ToString());
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
        ShipConfig config = ShipConfig.krios;
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
        list = GetDialogsTagAttack();

        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr2_3_dialog_7_M1");
                list.Add("cmRdr2_3_dialog_7_A1");
                list.Add("cmRdr2_3_dialog_7_M2");
                list.Add("cmRdr2_3_dialog_7_A3");
                list.Add("cmRdr2_3_dialog_7_M4");
                list.Add("cmRdr2_3_dialog_7_A5");
                list.Add("cmRdr2_3_dialog_7_M6");
                list.Add("cmRdr2_3_dialog_7_A7");
                list.Add("cmRdr2_3_dialog_7_M8");
                list.Add("cmRdr2_3_dialog_7_A9");
                list.Add("cmRdr2_3_dialog_7_M10");
                list.Add("cmRdr2_3_dialog_7_A11");
                break;
            case 2:
                list.Add("cmRdr2_3_dialog_8_M1");
                list.Add("cmRdr2_3_dialog_8_A1");
                list.Add("cmRdr2_3_dialog_8_M2");
                list.Add("cmRdr2_3_dialog_8_A3");
                list.Add("cmRdr2_3_dialog_8_M4");
                list.Add("cmRdr2_3_dialog_8_A5");
                list.Add("cmRdr2_3_dialog_8_M6");
                list.Add("cmRdr2_3_dialog_8_A7");
                list.Add("cmRdr2_3_dialog_8_M8");
                list.Add("cmRdr2_3_dialog_8_A9");
                list.Add("cmRdr2_3_dialog_8_M10");
                list.Add("cmRdr2_3_dialog_8_A11");
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
