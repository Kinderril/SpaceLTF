using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class NothingGlobalMapEvent : BaseGlobalMapEvent
{
    public override string Desc()
    {
        return "Nothing";
    }

    public override MessageDialogData GetDialog()
    {
        return null;
    }
}

