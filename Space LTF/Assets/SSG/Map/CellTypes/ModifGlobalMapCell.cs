using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifGlobalMapCell : GlobalMapCell
{
    public ModifGlobalMapCell(int id, int intX, int intZ, SectorData secto, ShipConfig config) : base(id, intX, intZ, secto, config)
    {
    }

    public override string Desc()
    {
        return Namings.Tag("modifCellName");
    }

    public override void Take()
    {
        //        WindowManager.Instance.OpenWindow(MainState.modif);
    }

    protected override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData(Namings.Tag("modifBaseStart"),
            new List<AnswerDialogData>
            {
                new AnswerDialogData(Namings.Tag("Yes"), Take),
                new AnswerDialogData(Namings.Tag("No"), null)
            });
        return mesData;
    }

    public override Color Color()
    {
        return new Color(153f / 255f, 204f / 255f, 255f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return false;
    }
    public override bool CanCellDestroy()
    {
        return true;
    }
}