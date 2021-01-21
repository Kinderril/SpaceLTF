using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr2_2_Fight2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _id;
    public QuestStageCmRdr2_2_Fight2(int id)    
        :base(QuestsLib.QuestStageCmRdr2_2_Fight2 + id.ToString())
    {
        _id = id;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.Where(x=>x.ShipConfig == ShipConfig.krios).ToList().RandomElement();
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_2_Fight2 + _id.ToString());
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
        list.Add("cmRdr2_2_dialog_4_M1");
        list.Add("cmRdr2_2_dialog_4_A1");
        list.Add("cmRdr2_2_dialog_4_M2");
        list.Add("cmRdr2_2_dialog_4_A3");
        list.Add("cmRdr2_2_dialog_4_M4");
        list.Add("cmRdr2_2_dialog_4_A5");
        list.Add("cmRdr2_2_dialog_4_M6");
        list.Add("cmRdr2_2_dialog_4_A7");
        list.Add("cmRdr2_2_dialog_4_M8");
        list.Add("cmRdr2_2_dialog_4_A9");
        list.Add("cmRdr2_2_dialog_4_M10");
        list.Add("cmRdr2_2_dialog_4_A11");
        list.Add("cmRdr2_2_dialog_4_M12");
        list.Add("cmRdr2_2_dialog_4_A13");
        list.Add("cmRdr2_2_dialog_4_M14");
        list.Add("cmRdr2_2_dialog_4_A15");

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
