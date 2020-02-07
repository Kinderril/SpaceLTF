using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UIElementWithTooltipCache : UIElementWithTooltip
{
    public string Cache;
    protected override string TextToTooltip()
    {
        return Cache;
    }
}

