using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StartGlobalCell : GlobalMapCell
{
    public StartGlobalCell(int id, int intX, int intZ, SectorData secto, ShipConfig config) : base(id, intX, intZ, secto, config)
    {
        InfoOpen = true;
        Complete();
    }


    public override string Desc()
    {
        return Namings.Tag("Start point");
    }

    public override bool IsPossibleToChange()
    {
        return true;
    }
    public override void Take()
    {

    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    protected override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("Nothing here.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Ok",null),
        });
        return mesData;
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

