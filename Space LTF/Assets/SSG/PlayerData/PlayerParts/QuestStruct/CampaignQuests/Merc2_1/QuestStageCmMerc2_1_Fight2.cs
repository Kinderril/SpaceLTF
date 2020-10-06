using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_1_Fight2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc2_1_Fight2()    
        :base(QuestsLib.CmMerc1_2_Fight2)
    {

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
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc1_2_Fight2);
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
        var playerEnemy = new PlayerAIWithBattleEvent("Army", true);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 8, 
            ArmyCreatorLibrary.GetArmy(ShipConfig.droid), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc2_1_dialog_4_M1"); 
        list.Add("DC");
        list.Add("cmMerc2_1_dialog_4_M2");
        list.Add("cmMerc2_1_dialog_4_A2");  
        list.Add("cmMerc2_1_dialog_4_M3");
        list.Add("DC");
        return list;
    }


    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmMerc2_1_dialog_5_M1");  
        list.Add("cmMerc2_1_dialog_5_A1");
        list.Add("cmMerc2_1_dialog_5_M2");
        list.Add("DC");
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return  ()=>dialog;
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
