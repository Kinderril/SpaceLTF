
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class EndGlobalCell : GlobalMapCell
{
    private FinalBattleData _data;

    public EndGlobalCell(int id, int intX, int intZ, SectorData secto) : base( id, intX, intZ, secto)
    {
        InfoOpen = true;
        Scouted();
    }
    

    public override string Desc()
    {
        return "Galaxy gate";
    }

    public override void Take()
    {

    }

    public override bool CanCellDestroy()
    {
        return false;
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        var getDialog = _data.GetDialog();
        return getDialog;
    }

    public override MessageDialogData GetDialog()
    {
//        list.Add(new AnswerDialogData("Ok", null));
//        var mesData = new MessageDialogData(String.Format("This is your main goal. You have {0}/{1} parts", mainQuest.mainElementsFound, PlayerQuestData.MaxMainElements), list);
        return _data.GetDialog();
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f/255f);
    }

    public override void ComeTo()
    {
        var questData = MainController.Instance.MainPlayer.QuestData;
        questData.ComeToLastPoint();
        questData.CheckIfOver();
        _data = questData.LastBattleData;
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

