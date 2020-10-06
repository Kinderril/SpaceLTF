using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_3_Fight3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index;
    public QuestStageCmOcr3_3_Fight3(int index)    
        :base(QuestsLib.QuestStageCmOcr3_3_Fight3)
    {
        _index = index;
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

        var index = _index - 1;
        SectorCellContainer container;
        FreeActionGlobalMapCell cell;
        index = Mathf.Clamp(index, 1, cells.Count - 1);
        if (index < cells.Count)
        {
            container = cells[index];
            cell = container.Data as FreeActionGlobalMapCell;
            cell.SetQuestData(dialogFunc);
            return cell;
        }

        Debug.LogError($"Error final quest: _index :{_index}  cells.count:{cells.Count}");
        foreach (var sectorCellContainer in cells)
        {
            var freeAction = sectorCellContainer.Data as FreeActionGlobalMapCell;

            Debug.LogError($"All ids:{freeAction.Id}");
        }
        container = cells.RandomElement();
        cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;

    }
    private void DialogEnds()
    {
        Fight();
    }
    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        return null;
    }


    private void Fight()
    {
        TextChangeEvent();
        cell1.SetQuestData(null);
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_3_Fight3);
        MainController.Instance.PreBattle(_player, PlayerToDefeat(), false, false);

    }

    public Player PlayerToDefeat()
    {
        var armyData = ArmyCreatorLibrary.GetArmy(ShipConfig.raiders);
        var power = cell1.Power;
        var btd = new BattleTypeData(2,2,ShipConfig.krios,ShipConfig.raiders);
        var playerEnemy = new PlayerAIWithBattleEvent("Army", false, btd);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 5, 8,
            armyData, true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_6_M1");
        list.Add("cmOcr3_3_dialog_6_A1");    
        list.Add("cmOcr3_3_dialog_6_M2");
        list.Add("cmOcr3_3_dialog_6_A2");    
        list.Add("cmOcr3_3_dialog_6_M3");
        list.Add("cmOcr3_3_dialog_6_A3");    
        list.Add("cmOcr3_3_dialog_6_M4");
        list.Add("cmOcr3_3_dialog_6_A4");  
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
