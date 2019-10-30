using UnityEngine;
using System.Collections;

public class CoreSpellUI : UIElementWithTooltip
{
    public int index = 0;

    public void OnClick()
    {
        WindowManager.Instance.ItemInfosController.Init(index);
    }

    protected override string TextToTooltip()
    {
        var isZero = index == 0;
        return isZero ? Namings.PriorityTarget : Namings.BaitPriorityTarget;
    }
}
