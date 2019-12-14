using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SideAttackTooltipInfo : UIElementWithTooltip
{
    private ESideAttack _priority1;
    private string inof = "";

    public void SetData(ESideAttack priority1)
    {
        _priority1 = priority1;
        inof = Namings.Tag($"ESideAttack{_priority1.ToString()}");
    }

    protected override string TextToTooltip()
    {
        return inof;
    }
}

