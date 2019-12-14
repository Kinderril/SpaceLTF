using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PriorityTooltipInfo : UIElementWithTooltip
{
    private ECommanderPriority1 _priority1;
    private string inof = "";

    public void SetData(ECommanderPriority1 priority1)
    {
        _priority1 = priority1;
        inof = Namings.Tag($"ECommanderPriority1{_priority1.ToString()}");
    }

    protected override string TextToTooltip()
    {
        return inof;
    }
}

