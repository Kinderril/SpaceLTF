using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr3_3_Fight2 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int _index;
    public QuestStageCmOcr3_3_Fight2(int index)    
        :base(QuestsLib.QuestStageCmOcr3_3_Fight2)
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr3_3_Fight2);
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
        configs.Remove(ShipConfig.ocrons);
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

    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_4_M1");
        list.Add("cmOcr3_3_dialog_4_A1");  
        list.Add("cmOcr3_3_dialog_4_M2");
        list.Add("cmOcr3_3_dialog_4_A2");  
        list.Add("cmOcr3_3_dialog_4_M3");
        list.Add("cmOcr3_3_dialog_4_A3");  
        list.Add("cmOcr3_3_dialog_4_M4");
        list.Add("cmOcr3_3_dialog_4_A4");  
        list.Add("cmOcr3_3_dialog_4_M5");
        list.Add("cmOcr3_3_dialog_4_A5");  
        list.Add("cmOcr3_3_dialog_4_M6");
        list.Add("cmOcr3_3_dialog_4_A6");
        return list;       
    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
//        return null;
        var list = new List<string>();
        list.Add("cmOcr3_3_dialog_5_M1");
        list.Add("cmOcr3_3_dialog_5_A1");     
        list.Add("cmOcr3_3_dialog_5_M2");
        list.Add("cmOcr3_3_dialog_5_A2");     
        list.Add("cmOcr3_3_dialog_5_M3");
        list.Add("cmOcr3_3_dialog_5_A3");     
        list.Add("cmOcr3_3_dialog_5_M4");
        list.Add("cmOcr3_3_dialog_5_A4");     
        list.Add("cmOcr3_3_dialog_5_M5");
        list.Add("cmOcr3_3_dialog_5_A5");     
        list.Add("cmOcr3_3_dialog_5_M6");
        list.Add("cmOcr3_3_dialog_5_A6");     
        list.Add("cmOcr3_3_dialog_5_M7");
        list.Add("cmOcr3_3_dialog_5_A7");     
        list.Add("cmOcr3_3_dialog_5_M8");
        list.Add("cmOcr3_3_dialog_5_A8");     
        list.Add("cmOcr3_3_dialog_5_M9");
        list.Add("cmOcr3_3_dialog_5_A9");     
        list.Add("cmOcr3_3_dialog_5_M10");
        list.Add("cmOcr3_3_dialog_5_A10");     
        list.Add("cmOcr3_3_dialog_5_M11");
        list.Add("cmOcr3_3_dialog_5_A11");
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
