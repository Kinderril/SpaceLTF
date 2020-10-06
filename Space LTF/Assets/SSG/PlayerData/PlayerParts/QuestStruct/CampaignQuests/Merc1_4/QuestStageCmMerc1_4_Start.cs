using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc1_4_Start : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmMerc1_4_Start()    
        :base(QuestsLib.CM_MERC_1_4_START)
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
        var msg10 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M10"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A10"), DialogEnds) }, true);     
        var msg9 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M9"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A9"), null, () => msg10) }, true);   
        var msg8 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M8"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A8"), null, () => msg9) }, true);   
        var msg7 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M7"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A7"), null, () => msg8) }, true);   
        var msg6 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M6"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("DC"), null, () => msg7) }, true);
        var msg5q = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M5"),
            new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Confirm"),null, () => msg6) ,
                new AnswerDialogData(Namings.Tag("Decline"),null, null) ,
            },true);    

        var msg4 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M4"),
            new List<AnswerDialogData>(){new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A4"),null, () => msg5q )},true);
        var msg3= new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M3"),
            new List<AnswerDialogData>(){new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A3"),null,()=> msg4) },true);       
        var msg2 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M2"),
            new List<AnswerDialogData>(){new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A2"),null, () => msg3) },true);
        var msg1 = new MessageDialogData(Namings.Tag("cmMerc1_4_dialog_0_M1"),
            new List<AnswerDialogData>(){new AnswerDialogData(Namings.Tag("cmMerc1_4_dialog_0_A1"),null,()=> msg2) },true);


        return msg1;
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_1_4_START);
        cell1.SetQuestData(null);
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
