using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmOcr2_2_ComeTo3 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private Player _player;
    private int _count;
    public QuestStageCmOcr2_2_ComeTo3(int moneyCount)    
        :base(QuestsLib.QuestStageCmOcr2_2_ComeTo3)
    {
        _count = moneyCount;
    }

    protected override bool StageActivate(Player player)
    {
        _player = player;
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.krios);
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

        if (_player.MoneyData.HaveMoney(_count))
        {
            _player.MoneyData.RemoveMoney(_count);
            return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
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
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_ComeTo3);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_8_M1");
        list.Add("cmOcr2_2_dialog_8_A1");   
        list.Add("cmOcr2_2_dialog_8_M2");
        list.Add("cmOcr2_2_dialog_8_A2");   
        list.Add("cmOcr2_2_dialog_8_M3");
        list.Add("cmOcr2_2_dialog_8_A3");   
        list.Add("cmOcr2_2_dialog_8_M4");
        list.Add("DC");   
        list.Add("cmOcr2_2_dialog_8_M5");
        list.Add("cmOcr2_2_dialog_8_A5");   
        list.Add("cmOcr2_2_dialog_8_M6");
        list.Add("DC");   
        list.Add("cmOcr2_2_dialog_8_M7");
        list.Add("cmOcr2_2_dialog_8_A7");   
        list.Add("cmOcr2_2_dialog_8_M8");
        list.Add("cmOcr2_2_dialog_8_A8");   
        list.Add("cmOcr2_2_dialog_8_M9");
        list.Add("cmOcr2_2_dialog_8_A9");   
        list.Add("cmOcr2_2_dialog_8_M10");
        list.Add("cmOcr2_2_dialog_8_A10");   
        list.Add("cmOcr2_2_dialog_8_M11");
        list.Add("cmOcr2_2_dialog_8_A11"); 
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
