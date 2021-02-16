using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr3_3_FightFinal : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index;
    public QuestStageCmRdr3_3_FightFinal(int index)    
        :base(QuestsLib.QuestStageCmRdr3_3_FightFinal)
    {
        _index = index;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x is SectorFinalBattle);
        sectorId.UnHide();

        cell1 = FindAndMarkCell(sectorId, GetDialog, _index) as FreeActionGlobalMapCell;
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
        Fight();
    }

    public Player PlayerToDefeat2()
    {
        var power = Mathf.Clamp(cell1.Power * 1.2f,20f,9000f);
        ShipConfig config = ShipConfig.raiders;
        var btd = new BattleTypeData(4, 4, ShipConfig.raiders, ShipConfig.raiders);
        var playerEnemy = new PlayerAIWithBattleEvent("raiders", false, btd);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 6, 9,
            ArmyCreatorLibrary.GetArmy(config), true,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }

    private void CompleteQuest()
    {

        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr3_3_FightFinal);
    }
                
    private void Fight()
    {
        TextChangeEvent();
        cell1.SetQuestData(null);
        MainController.Instance.PreBattle(_player, PlayerToDefeat2(), false, false, CompleteQuest);

    }


    private List<string> GetDialogsTag()
    {
        return GetDialogsTagAttack();
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
