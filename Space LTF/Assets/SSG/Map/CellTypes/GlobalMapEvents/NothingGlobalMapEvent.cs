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
        return Namings.Tag("Nothing");
    }

    public override MessageDialogData GetDialog()
    {
        return null;
    }

    public NothingGlobalMapEvent(ShipConfig config) : base(config)
    {
    }
}

