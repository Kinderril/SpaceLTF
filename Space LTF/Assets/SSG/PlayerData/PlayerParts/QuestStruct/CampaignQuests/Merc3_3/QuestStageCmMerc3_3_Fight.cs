using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc3_3_Fight : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index;
    public QuestStageCmMerc3_3_Fight(int index)    
        :base(QuestsLib.CmMerc3_3_Fight + index)
    {
        _index = index;
    }

    protected override bool StageActivate(Player player)
    {

        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x=>x is SectorFinalBattle);
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
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        cells.Sort(comparator);

        var index = _index - 1;
        SectorCellContainer container;
        FreeActionGlobalMapCell cell;
        if (index < cells.Count  )
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

    private MessageDialogData GetDialog()
    {
        if (_index == 1)
        {
            return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);

        }
        else
        {

            return DialogsLibrary.GetPairDialogByTag(GetDialogsTagAttack(), DialogEnds);
        }
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CmMerc3_3_Fight + _index);
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
        var power = cell1.Power * 1.1f;
        EBattleType tyep = EBattleType.defenceWaves;
#if UNITY_EDITOR
//        Debug.LogError("debug final battle type");
//        tyep = EBattleType.standart;
#endif

        var playerEnemy = new PlayerAIWithBattleEvent("Army", false, new BattleTypeData(tyep));
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6,
            armyData, true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc3_3_dialog_0_M1");   
        list.Add("cmMerc3_3_dialog_0_A1");  
        list.Add("cmMerc3_3_dialog_0_M2");   
        list.Add("cmMerc3_3_dialog_0_A2");  
        list.Add("cmMerc3_3_dialog_0_M3");   
        list.Add("cmMerc3_3_dialog_0_A3");  
        list.Add("cmMerc3_3_dialog_0_M4");   
        list.Add("cmMerc3_3_dialog_0_A4");   
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
