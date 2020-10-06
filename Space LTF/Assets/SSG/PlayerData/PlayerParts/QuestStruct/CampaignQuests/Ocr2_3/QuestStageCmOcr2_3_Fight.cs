using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_3_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr2_3_Fight()    
        :base(QuestsLib.QuestStageCmOcr2_3_Fight)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);

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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_3_Fight);
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
        var playerEnemy = new PlayerAIWithBattleEvent("config", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 5, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }  



    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_3_dialog_2_M1");
        list.Add("cmOcr2_3_dialog_2_A1");      
        list.Add("cmOcr2_3_dialog_2_M2");
        list.Add("cmOcr2_3_dialog_2_A2");   

        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        MessageDialogData dialog;
        var list = new List<string>();
        list.Add("cmOcr2_3_dialog_3_M1");
        list.Add("cmOcr2_3_dialog_3_A1"); 
        list.Add("cmOcr2_3_dialog_3_M2");
        list.Add("cmOcr2_3_dialog_3_A2"); 
        list.Add("cmOcr2_3_dialog_3_M3");
        list.Add("cmOcr2_3_dialog_3_A3"); 
        list.Add("cmOcr2_3_dialog_3_M4");
        list.Add("cmOcr2_3_dialog_3_A4"); 
        list.Add("cmOcr2_3_dialog_3_M5");
        list.Add("cmOcr2_3_dialog_3_A5"); 
        list.Add("cmOcr2_3_dialog_3_M6");
        list.Add("cmOcr2_3_dialog_3_A6"); 
        list.Add("cmOcr2_3_dialog_3_M7");
        list.Add("cmOcr2_3_dialog_3_A7"); 
        list.Add("cmOcr2_3_dialog_3_M8");
        list.Add("cmOcr2_3_dialog_3_A8"); 
        list.Add("cmOcr2_3_dialog_3_M9");
        list.Add("DC");
        dialog = DialogsLibrary.GetPairDialogByTag(list, null);
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
