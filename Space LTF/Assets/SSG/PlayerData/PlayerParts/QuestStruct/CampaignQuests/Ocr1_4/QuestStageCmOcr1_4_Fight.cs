using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr1_4_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr1_4_Fight()    
        :base(QuestsLib.QuestStageCmOcr1_4_Fight )
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
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), DialogEnds,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans);
        return mesData;
//        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr1_4_Fight );
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
        ShipConfig config = ShipConfig.raiders;
        var playerEnemy = new PlayerAIWithBattleEvent("federation", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();

        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var list = new List<string>();
        list.Add("cmOcr1_4_dialog_1_M1");
        list.Add("cmOcr1_4_dialog_1_A1");     
        list.Add("cmOcr1_4_dialog_1_M2");
        list.Add("cmOcr1_4_dialog_1_A2");     
        list.Add("cmOcr1_4_dialog_1_M3");
        list.Add("cmOcr1_4_dialog_1_A3");     
        list.Add("cmOcr1_4_dialog_1_M4");
        list.Add("cmOcr1_4_dialog_1_A4");     
        list.Add("cmOcr1_4_dialog_1_M5");
        list.Add("cmOcr1_4_dialog_1_A5");
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
