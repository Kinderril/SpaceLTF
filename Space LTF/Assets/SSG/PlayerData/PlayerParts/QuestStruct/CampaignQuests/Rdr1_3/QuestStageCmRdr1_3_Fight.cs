using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_3_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr1_3_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr1_3_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig != ShipConfig.droid).ToList().RandomElement();

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_3_Fight + _id.ToString());
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
        ShipConfig config = ShipConfig.mercenary;
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 3, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_3_dialog_1_M1");
        list.Add("cmRdr1_3_dialog_1_A1");   
        list.Add("cmRdr1_3_dialog_1_M2");
        list.Add("cmRdr1_3_dialog_1_A2");   
        list.Add("cmRdr1_3_dialog_1_M3");
        list.Add("cmRdr1_3_dialog_1_A3");   
        list.Add("cmRdr1_3_dialog_1_M4");
        list.Add("cmRdr1_3_dialog_1_A4");   
        list.Add("cmRdr1_3_dialog_1_M5");
        list.Add("cmRdr1_3_dialog_1_A5");   
        list.Add("cmRdr1_3_dialog_1_M6");
        list.Add("cmRdr1_3_dialog_1_A6");   
        list.Add("cmRdr1_3_dialog_1_M7");
        list.Add("cmRdr1_3_dialog_1_A7");
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();

        list.Add("cmRdr1_3_dialog_2_M1");
        list.Add("cmRdr1_3_dialog_2_A1"); 
        list.Add("cmRdr1_3_dialog_2_M2");
        list.Add("cmRdr1_3_dialog_2_A2"); 
        list.Add("cmRdr1_3_dialog_2_M3");
        list.Add("cmRdr1_3_dialog_2_A3");    
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
