using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class EndTutorGlobalCell : GlobalMapCell
{                                    
    public EndTutorGlobalCell(int id, int intX, int intZ, SectorData sector)
        : base(id, intX, intZ, sector,ShipConfig.droid)
    {
        InfoOpen = true;
        Scouted();
    }

    public override string Desc()
    {
        return Namings.Tag("Galaxy gate");
    }

    public override void Take()
    {

    }

    public override bool CanCellDestroy()
    {
        return false;
    }

    protected override MessageDialogData GetDialog()
    {
        var answers = new List<AnswerDialogData>();
        MessageDialogData mesData;
        answers.Add(new AnswerDialogData(Namings.Tag("Ok"), EndGameWin));
        mesData = new MessageDialogData(Namings.DialogTag("tutorEnds"), answers);
        return mesData;
    }
    private void EndGameWin()
    {
        MainController.Instance.BattleData.EndGame(true);
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override void ComeTo(GlobalMapCell from)
    {

    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

