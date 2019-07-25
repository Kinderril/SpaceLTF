using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifGlobalMapCell : GlobalMapCell
{
    public ModifGlobalMapCell(int id, int intX, int intZ) : base(id, intX, intZ)
    {
    }

    public override string Desc()
    {
        return "Modification station";
    }

    public override void Take()
    {
//        WindowManager.Instance.OpenWindow(MainState.modif);
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("This is modification base. You can try to modificate something.",
            new List<AnswerDialogData>
            {
                new AnswerDialogData("Yes", Take),
                new AnswerDialogData("No", null)
            });
        return mesData;
    }

    public override Color Color()
    {
        return new Color(153f/255f, 204f/255f, 255f/255f);
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