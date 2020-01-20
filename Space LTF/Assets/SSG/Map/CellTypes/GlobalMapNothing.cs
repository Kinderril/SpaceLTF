using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GlobalMapNothing : GlobalMapCell
{
    public GlobalMapNothing(int id, int iX, int iZ, SectorData secto, ShipConfig config) : base(id, iX, iZ, secto, config)
    {

    }

    public override string Desc()
    {
        return "Nothing";
    }

    public override void Take()
    {

    }

    protected override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("Nothing is here.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Yes",Take),
        });
        return mesData;
    }

    public override Color Color()
    {
        return UnityEngine.Color.black;
    }

    public override bool OneTimeUsed()
    {
        return true;
    }

    public override bool CanCellDestroy()
    {
        return false;
    }
}

