using System;

public class CriticalDamageObject : UIElementWithTooltip
{
    protected override string TextToTooltip()
    {
        return Namings.TryFormat(Namings.Tag("CriticalDamage"), Library.CRITICAL_DAMAGES_TO_DEATH);
    }
}
