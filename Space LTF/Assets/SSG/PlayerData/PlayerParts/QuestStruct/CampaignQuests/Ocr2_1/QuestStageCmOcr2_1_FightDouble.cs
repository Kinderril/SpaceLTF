using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_1_FightDouble : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr2_1_FightDouble()    
        :base(QuestsLib.QuestStageCmOcr2_1_Fight)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.droid);

        cell1 = FindAndMarkCellClosest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
//        var ans = new List<AnswerDialogData>()
//        {
//            new AnswerDialogData(Namings.DialogTag("MovingArmyFight"), DialogEnds,  null,false,false),
//        };
//        var mesData = new MessageDialogData(Namings.DialogTag("MovingArmyStart"), ans);
//        return mesData;
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_1_Fight);
        Fight();
    }

    private void Fight()
    {
        cell1.SetQuestData(null);
        TextChangeEvent();
        MainController.Instance.PreBattle(_player, PlayerToDefeat(), false, false);

    }

//    private int _fight2Complete = 0;



    public Player PlayerToDefeat()
    {
        var power = cell1.Power;
        ShipConfig config = ShipConfig.krios;
        var playerEnemy = new PlayerAIWithBattleEvent("krios", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 1, 3, 
            ArmyCreatorLibrary.GetArmy(config), true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }  


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_1_dialog_8_M1");
        list.Add("cmOcr2_1_dialog_8_A1");   
        list.Add("cmOcr2_1_dialog_8_M2");
        list.Add("cmOcr2_1_dialog_8_A2");   
        list.Add("cmOcr2_1_dialog_8_M3");
        list.Add("cmOcr2_1_dialog_8_A3");   
        list.Add("cmOcr2_1_dialog_8_M4");
        list.Add("cmOcr2_1_dialog_8_A4");   
        list.Add("cmOcr2_1_dialog_8_M5");
        list.Add("cmOcr2_1_dialog_8_A5");   
        list.Add("cmOcr2_1_dialog_8_M6");
        list.Add("cmOcr2_1_dialog_8_A6");   
        list.Add("cmOcr2_1_dialog_8_M7");
        list.Add("Attack");

        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {


        MessageDialogData dialog;
var list = new List<string>();

            list.Add("cmOcr2_1_dialog_9_M1");
            list.Add("cmOcr2_1_dialog_9_A1");
            list.Add("cmOcr2_1_dialog_9_M2");
            list.Add("cmOcr2_1_dialog_9_A2");
            list.Add("cmOcr2_1_dialog_9_M3");
            list.Add("cmOcr2_1_dialog_9_A3");
            list.Add("cmOcr2_1_dialog_9_M4");
            list.Add("cmOcr2_1_dialog_9_A4");
            list.Add("cmOcr2_1_dialog_9_M5");
            list.Add("cmOcr2_1_dialog_9_A5");
            list.Add("cmOcr2_1_dialog_9_M6");
            list.Add("cmOcr2_1_dialog_9_A6");
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
