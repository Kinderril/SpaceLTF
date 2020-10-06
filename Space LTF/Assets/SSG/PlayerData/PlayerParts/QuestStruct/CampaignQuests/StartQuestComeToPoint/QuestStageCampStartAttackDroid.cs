using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCampStartAttackDroid : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCampStartAttackDroid()    
        :base(QuestsLib.CM_START_QUEST_ATTACK)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.CurrentCell.Sector;

        cell1 = FindAndMarkCell(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var dialog = DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), null);
        return () => dialog;
    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTagAttack(), DialogEnds);
    }



    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_START_QUEST_ATTACK);
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
        var power = 9;
        var config = ShipConfig.droid;
        var playerEnemy = new PlayerAIWithBattleEvent("Droids", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 2, ArmyCreatorLibrary.GetArmy(config), false,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmStartQuestAttackM1");
        list.Add("cmStartQuestAttackA1");    
        list.Add("cmStartQuestAttackM2");
        list.Add("cmStartQuestAttackA2");    
        list.Add("cmStartQuestAttackM3");
        list.Add("cmStartQuestAttackA3");    
        list.Add("cmStartQuestAttackM4");
        list.Add("cmStartQuestAttackA4");    
        list.Add("cmStartQuestAttackM5");
        list.Add("cmStartQuestAttackA5");   
        return list;
    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc,GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data  != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }
        int deltaMax = Int32.MaxValue;

        SectorCellContainer container = null;
        foreach (var sectorCellContainer in cells)
        {
            var deltaX = Mathf.Abs(sectorCellContainer.indX - playerCell.indX);
            var deltaZ = Mathf.Abs(sectorCellContainer.indZ - playerCell.indZ);
            var deltaSum = deltaZ + deltaX;
            if (deltaSum < deltaMax)
            {
                deltaMax = deltaSum;
                container = sectorCellContainer;
            }
        }
        var cell = container.Data as FreeActionGlobalMapCell;

        cell.SetQuestData(dialogFunc);
        return cell;
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
