using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_3_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc1_3_Fight()    
        :base(QuestsLib.CmMerc1_3_Fight)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.raiders);

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
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc1_3_Fight);
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
        var playerEnemy = new PlayerAIWithBattleEvent("Ocrs", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 3, 
            ArmyCreatorLibrary.GetArmy(ShipConfig.ocrons), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc1_3_dialog_1_M1");
        list.Add("cmMerc1_3_dialog_1_A1");
        list.Add("cmMerc1_3_dialog_1_M2");
        list.Add("cmMerc1_3_dialog_1_A2");  
        list.Add("cmMerc1_3_dialog_1_M3");
        list.Add("cmMerc1_3_dialog_1_A3");
        return list;
    }


    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmMerc1_3_dialog_5_M1");
        list.Add("cmMerc1_3_dialog_5_A1");
        list.Add("cmMerc1_3_dialog_5_M2");
        list.Add("cmMerc1_3_dialog_5_A2");
        list.Add("cmMerc1_3_dialog_5_M3");
        list.Add("cmMerc1_3_dialog_5_A3");  
        list.Add("cmMerc1_3_dialog_5_M4");
        list.Add("cmMerc1_3_dialog_5_A4");
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
