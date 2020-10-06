using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_3_FinalFight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    public QuestStageCmMerc3_3_FinalFight()    
        :base(QuestsLib.CmMerc3_3_FinalFight)
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

    public GlobalMapCell FindAndMarkCell(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data != playerCell && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        int maxID = 0;
        int maxID2 = 0;
        SectorCellContainer container = null;
        foreach (var sectorCellContainer in cells)
        {
            var freeAction = sectorCellContainer.Data as FreeActionGlobalMapCell;
            if (  freeAction.Id > maxID)
            {
                maxID = freeAction.Id;
//                container = sectorCellContainer;
            }
        }

        foreach (var sectorCellContainer in cells)    //PRE MAX CELL
        {
            var freeAction = sectorCellContainer.Data as FreeActionGlobalMapCell;
            if ( freeAction.Id > maxID2 && freeAction.Id != maxID)
            {
                maxID2 = freeAction.Id;
                container = sectorCellContainer;
            }
        }

//        Debug.LogError($"Last fight cell id:{maxID}");
        var cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;

    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTagAttack(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc3_3_FinalFight);
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
        List<ShipConfig> configs = Library.Configs();
        configs.Remove(ShipConfig.droid);
        configs.Remove(ShipConfig.mercenary);
        var a1 = configs.RandomElement();
        configs.Remove(a1);
        var b2 = configs.RandomElement();
        var armyData = ArmyCreatorLibrary.GetArmy(a1, b2);
        var power = cell1.Power * 1.2f;
        var playerEnemy = new PlayerAIWithBattleEvent("Army", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6,
            armyData, true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
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
