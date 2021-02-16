using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr3_3_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    private int _index;
    public QuestStageCmRdr3_3_Fight(int id,int index)    
        :base(QuestsLib.QuestStageCmRdr3_3_Fight + id.ToString())
    {
        _index = index;
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        sectorId.UnHide();
        cell1 = FindAndMarkCell(sectorId, GetDialog, _index) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr3_3_Fight + _id.ToString());
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
        var playerEnemy = new PlayerAIWithBattleEvent("raiders", false);
        var data = ArmyCreatorLibrary.GetArmy(ShipConfig.federation, ShipConfig.raiders);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 4, 7, data, false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr3_3_dialog_2_M0");
        list.Add("cmRdr3_3_dialog_2_A1");
        list.Add("cmRdr3_3_dialog_2_M2");
        list.Add("DC");
        list.Add("cmRdr3_3_dialog_2_M3");
        list.Add("cmRdr3_3_dialog_2_A4");
        list.Add("cmRdr3_3_dialog_2_M5");
        list.Add("cmRdr3_3_dialog_2_A6");
        list.Add("cmRdr3_3_dialog_2_M7");
        list.Add("cmRdr3_3_dialog_2_A8");
        return list;
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        switch (_id)
        {
            case 1:
                list.Add("cmRdr3_3_dialog_3_M0");
                list.Add("cmRdr3_3_dialog_3_A1");
                list.Add("cmRdr3_3_dialog_3_M2");
                list.Add("cmRdr3_3_dialog_3_A3");
                list.Add("cmRdr3_3_dialog_3_M4");
                list.Add("cmRdr3_3_dialog_3_A5");
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
