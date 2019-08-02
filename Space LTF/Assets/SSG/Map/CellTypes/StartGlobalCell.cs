using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class StartGlobalCell : GlobalMapCell
{
    public StartGlobalCell(int id, int intX, int intZ, SectorData secto) : base( id, intX, intZ, secto)
    {
        InfoOpen = true;
    }
    

    public override string Desc()
    {
        return "Start point";
    }

    public override void Take()
    {

    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("Nothing here.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Ok",null),
        });
        return mesData;
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f/255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

