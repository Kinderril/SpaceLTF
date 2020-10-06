using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_1_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr1_1_Fight(int id)    
        :base(QuestsLib.QuestStageCmRdr1_1_Fight + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_1_Fight + _id.ToString());
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
        ShipConfig config = ShipConfig.ocrons;
        var btd = new BattleTypeData(1, 0, ShipConfig.raiders, ShipConfig.ocrons);
        var playerEnemy = new PlayerAIWithBattleEvent("ocrons", false, btd);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 3, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_1_dialog_6_M1");
        list.Add("cmRdr1_1_dialog_6_A1");  
        list.Add("cmRdr1_1_dialog_6_M2");
        list.Add("cmRdr1_1_dialog_6_A2");  
        list.Add("cmRdr1_1_dialog_6_M3");
        list.Add("cmRdr1_1_dialog_6_A3");  
        list.Add("cmRdr1_1_dialog_6_M4");
        list.Add("cmRdr1_1_dialog_6_A4");  
        list.Add("cmRdr1_1_dialog_6_M5");
        list.Add("cmRdr1_1_dialog_6_A5");  
        list.Add("cmRdr1_1_dialog_6_M6");
        list.Add("cmRdr1_1_dialog_6_A6");
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmRdr1_1_dialog_7_M1");
        list.Add("cmRdr1_1_dialog_7_A1"); 
        list.Add("cmRdr1_1_dialog_7_M2");
        list.Add("cmRdr1_1_dialog_7_A2"); 
        list.Add("cmRdr1_1_dialog_7_M3");
        list.Add("cmRdr1_1_dialog_7_A3"); 
        list.Add("cmRdr1_1_dialog_7_M4");
        list.Add("cmRdr1_1_dialog_7_A4");     
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
