using UnityEngine;
using System.Collections;

public class CriticalDamageObject : UIElementWithTooltip
{
    protected override string TextToTooltip()
    {
        return Namings.CriticalDamage;
     }
}
