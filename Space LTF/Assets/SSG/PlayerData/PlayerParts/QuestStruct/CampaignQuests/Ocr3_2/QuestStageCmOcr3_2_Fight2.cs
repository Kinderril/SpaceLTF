using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_2_Fight2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr3_2_Fight2()    
        :base(QuestsLib.QuestStageCmOcr3_2_Fight)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.federation);

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_2_Fight);
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
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 3, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr1_2_dialog_2_M1");
        list.Add("cmOcr1_2_dialog_2_A1");
        list.Add("cmOcr1_2_dialog_2_M2");
        list.Add("cmOcr1_2_dialog_2_A2");
        list.Add("cmOcr1_2_dialog_2_M3");
        list.Add("cmOcr1_2_dialog_2_A3");
        list.Add("cmOcr1_2_dialog_2_M4");
        list.Add("cmOcr1_2_dialog_2_A4");
        list.Add("cmOcr1_2_dialog_2_M5");
        list.Add("cmOcr1_2_dialog_2_A5");
        list.Add("cmOcr1_2_dialog_2_M6");
        list.Add("DC");
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
//        return null;
        var list = new List<string>();
        list.Add("cmOcr1_2_dialog_5_M1");
        list.Add("cmOcr1_2_dialog_5_A1");
        list.Add("cmOcr1_2_dialog_5_M2");
        list.Add("cmOcr1_2_dialog_5_A2");
        list.Add("cmOcr1_2_dialog_5_M3");
        list.Add("cmOcr1_2_dialog_5_A3");     
        list.Add("cmOcr1_2_dialog_5_M4");
        list.Add("cmOcr1_2_dialog_5_A4");
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
        return $"{Namings.Tag("cmOcrDream")}";
    }
}
