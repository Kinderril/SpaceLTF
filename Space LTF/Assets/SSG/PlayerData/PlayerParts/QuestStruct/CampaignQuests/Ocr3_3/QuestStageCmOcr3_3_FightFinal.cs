using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_3_FightFinal : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmOcr3_3_FightFinal()    
        :base(QuestsLib.QuestStageCmOcr3_3_FightFinal)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        sectorId.UnHide();

        cell1 = FindAndMarkCell(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
    public GlobalMapCell FindAndMarkCell(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        cells.Sort(comparator);

        SectorCellContainer container;
        FreeActionGlobalMapCell cell;
        var index = cells.Count - 2;
        container = cells[index];
        cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;

    }
    private void DialogEnds()
    {
        Fight();
    }

    public Player PlayerToDefeat2()
    {
        var power = cell1.Power * 1.2f;
        ShipConfig config = ShipConfig.ocrons;
        var btd = new BattleTypeData(5, 5, ShipConfig.ocrons, ShipConfig.ocrons);
        var playerEnemy = new PlayerAIWithBattleEvent("ocrons", false, btd);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 4, 9,
            ArmyCreatorLibrary.GetArmy(config), true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }
//    protected override Func<MessageDialogData> AfterCompleteDialog()
//    {
//        MessageDialogData dialog;
//        var list = new List<string>();
//        list.Add("cmOcr3_3_dialog_9_M1");
//        list.Add("cmOcr3_3_dialog_9_A1");
//        list.Add("cmOcr3_3_dialog_9_M2");
//        list.Add("cmOcr3_3_dialog_9_A2");
//        list.Add("cmOcr3_3_dialog_9_M3");
//        list.Add("cmOcr3_3_dialog_9_A3");
//        list.Add("cmOcr3_3_dialog_9_M4");
//        list.Add("DC");
//        list.Add("cmOcr3_3_dialog_9_M5");
//        list.Add("DC");
//        list.Add("cmOcr3_3_dialog_9_M6");
//        list.Add("cmOcr3_3_dialog_9_A6");
//        list.Add("cmOcr3_3_dialog_9_M7");
//        list.Add("cmOcr3_3_dialog_9_A7");
//        list.Add("cmOcr3_3_dialog_9_M8");
//        list.Add("cmOcr3_3_dialog_9_A8");
//        list.Add("cmOcr3_3_dialog_9_M9");
//        list.Add("cmOcr3_3_dialog_9_A9");
//        list.Add("cmOcr3_3_dialog_9_M10");
//        list.Add("cmOcr3_3_dialog_9_A10");
//        list.Add("cmOcr3_3_dialog_9_M11");
//        list.Add("DC");
////        list.Add("cmOcr3_3_dialog_9_M12");
////        list.Add("DC");
//        dialog = DialogsLibrary.GetPairDialogByTag(list, null);
//        return () => dialog;
//    }

    private void CompleteQuest()
    {

        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_3_FightFinal);
    }
                
    private void Fight()
    {
        CompleteQuest();
        TextChangeEvent();
        cell1.SetQuestData(null);
        MainController.Instance.PreBattle(_player, PlayerToDefeat2(), false, false);

    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_8_M1");
        list.Add("cmOcr3_3_dialog_8_A1");
        list.Add("cmOcr3_3_dialog_8_M2");
        list.Add("cmOcr3_3_dialog_8_A2");
        list.Add("cmOcr3_3_dialog_8_M3");
        list.Add("cmOcr3_3_dialog_8_A3");
        list.Add("cmOcr3_3_dialog_8_M4");
        list.Add("cmOcr3_3_dialog_8_A4");
        list.Add("cmOcr3_3_dialog_8_M5");
        list.Add("cmOcr3_3_dialog_8_A5");
        list.Add("cmOcr3_3_dialog_8_M6");
        list.Add("DC");
        list.Add("cmOcr3_3_dialog_8_M7");
        list.Add("cmOcr3_3_dialog_8_A7");
        list.Add("cmOcr3_3_dialog_8_M8");
        list.Add("DC");
        list.Add("cmOcr3_3_dialog_8_M9");
        list.Add("cmOcr3_3_dialog_8_A9");
        return list;       
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
