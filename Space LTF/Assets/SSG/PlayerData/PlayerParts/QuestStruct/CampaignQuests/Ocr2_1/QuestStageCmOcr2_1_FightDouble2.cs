using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_1_FightDouble2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr2_1_FightDouble2()    
        :base(QuestsLib.QuestStageCmOcr2_1_FightDouble2)
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
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), DialogEnds,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans,true);
        return mesData;
//        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_1_FightDouble2);
        Fight();
    }

    private void Fight()
    {
        cell1.SetQuestData(null);
        TextChangeEvent();
        MainController.Instance.PreBattle(_player, PlayerToDefeat2(), false, false);

    }


    public Player PlayerToDefeat2()
    {
        var power = cell1.Power;
        ShipConfig config = ShipConfig.krios;
        var playerEnemy = new PlayerAIWithBattleEvent("krios", false,new BattleTypeData(EBattleType.defenceWaves));
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 3, 4, 
            ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }


    protected override Func<MessageDialogData> AfterCompleteDialog()
    {

        MessageDialogData dialog;
        var list = new List<string>();
        list.Add("cmOcr2_1_dialog_11_M1");
        list.Add("cmOcr2_1_dialog_11_A1");
        list.Add("cmOcr2_1_dialog_11_M2");
        list.Add("cmOcr2_1_dialog_11_A2");
        list.Add("cmOcr2_1_dialog_11_M3");
        list.Add("cmOcr2_1_dialog_11_A3");
        list.Add("cmOcr2_1_dialog_11_M4");
        list.Add("DC");
        list.Add("cmOcr2_1_dialog_11_M5");
        list.Add("cmOcr2_1_dialog_11_A5");
        list.Add("cmOcr2_1_dialog_11_M6");
        list.Add("cmOcr2_1_dialog_11_A6");
        list.Add("cmOcr2_1_dialog_11_M7");
        list.Add("cmOcr2_1_dialog_11_A7");
        list.Add("cmOcr2_1_dialog_11_M8");
        list.Add("cmOcr2_1_dialog_11_A8");
        list.Add("cmOcr2_1_dialog_11_M9");
        list.Add("cmOcr2_1_dialog_11_A9");
        list.Add("cmOcr2_1_dialog_11_M10");
        list.Add("cmOcr2_1_dialog_11_A10");
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
