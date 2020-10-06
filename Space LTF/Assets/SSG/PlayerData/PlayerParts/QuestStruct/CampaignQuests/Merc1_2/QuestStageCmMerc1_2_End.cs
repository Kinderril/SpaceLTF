using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_2_End : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc1_2_End()    
        :base(QuestsLib.CM_MERC_1_2_END)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);
        cell1 = FindAndMarkCellClosest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
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
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_1_2_END);
       var  ModulReward1 = Library.CreatSimpleModul(1);
        var ModulReward2 = Library.CreatSimpleModul(2);
        TakeModul(ModulReward1);
        TakeModul(ModulReward2);
        cell1.SetQuestData(null);
    }

    public void TakeModul(BaseModulInv modulReward)
    {
        var inv = MainController.Instance.MainPlayer.Inventory;
        if (inv.GetFreeSimpleSlot(out int slot))
        {
            inv.TryAddSimpleModul(modulReward, slot);
        }
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmMerc1_2_dialog_end_M1");
        list.Add("cmMerc1_2_dialog_end_A1");   
        list.Add("cmMerc1_2_dialog_end_M2");
        list.Add("dialogContinue");
        list.Add("cmMerc1_2_dialog_end_M2_2");
        list.Add("cmMerc1_2_dialog_end_A2");   
        list.Add("cmMerc1_2_dialog_end_M3");
        list.Add("dialogContinue");
        list.Add("cmMerc1_2_dialog_end_M3_2");
        list.Add("dialogContinue");
        list.Add("cmMerc1_2_dialog_end_M3_3");
        list.Add("cmMerc1_2_dialog_end_A3");   
        list.Add("cmMerc1_2_dialog_end_M4");
        list.Add("cmMerc1_2_dialog_end_A4");     
        list.Add("cmMerc1_2_dialog_end_M5");
        list.Add("cmMerc1_2_dialog_end_A5");   
        list.Add("cmMerc1_2_dialog_end_M6");
        list.Add("cmMerc1_2_dialog_end_A6");   
        list.Add("cmMerc1_2_dialog_end_M7");
        list.Add("dialogContinue");
        list.Add("cmMerc1_2_dialog_end_M7_2");
        list.Add("cmMerc1_2_dialog_end_A7"); 
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
        return $"{Namings.Tag("cmComeToPoint")}";
    }
}
