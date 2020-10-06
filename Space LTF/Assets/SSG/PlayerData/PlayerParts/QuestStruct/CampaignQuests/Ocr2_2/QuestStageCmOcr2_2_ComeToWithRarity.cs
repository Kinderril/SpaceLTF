using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_2_ComeToWithRarity : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private Player _player;
    public QuestStageCmOcr2_2_ComeToWithRarity(EParameterItemRarity rarity)    
        :base(QuestsLib.QuestStageCmOcr2_2_ComeTo1)
    {

    }

    protected override bool StageActivate(Player player)
    {
        _player = player;
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);
        cell1 = FindAndMarkCellRandom(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
        if (_player == null)
        {
            return null;
        }

        foreach (var item in _player.Inventory.ParamItems)
        {
            if (item.Rarity == EParameterItemRarity.perfect)
            {
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
            }
        }

        return DialogsLibrary.GetPairDialogByTag(GetDialogsTagNull(), null); ;

    }

    private List<string> GetDialogsTagNull()
    {

        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_null_M1");
        list.Add("cmOcr2_2_dialog_null_A1");
        return list;

    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_ComeTo1);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_1_M1");
        list.Add("cmOcr2_2_dialog_1_A1"); 
        list.Add("cmOcr2_2_dialog_1_M2");
        list.Add("DC"); 
        list.Add("cmOcr2_2_dialog_1_M3");
        list.Add("cmOcr2_2_dialog_1_A3"); 
        list.Add("cmOcr2_2_dialog_1_M4");
        list.Add("cmOcr2_2_dialog_1_A4"); 
        list.Add("cmOcr2_2_dialog_1_M5");
        list.Add("cmOcr2_2_dialog_1_A5"); 
        list.Add("cmOcr2_2_dialog_1_M6");
        list.Add("cmOcr2_2_dialog_1_A6"); 
        list.Add("cmOcr2_2_dialog_1_M7");
        list.Add("cmOcr2_2_dialog_1_A7"); 
//        list.Add("cmOcr2_2_dialog_1_M8");
//        list.Add("cmOcr2_2_dialog_1_A8");
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
