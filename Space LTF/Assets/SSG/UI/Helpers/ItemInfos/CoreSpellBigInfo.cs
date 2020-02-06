using System;
using TMPro;


public class CoreSpellBigInfo : AbstractBaseInfoUI
{
    public TextMeshProUGUI DescField;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI CostCountField;
    public TextMeshProUGUI CostDelayField;


    public void Init(int index, Action callback)
    {
        var isZero = index == 0;
        base.Init(callback);


        NameField.text = isZero ? Namings.Tag("PriorityTarget") : Namings.Tag("BaitPriorityTarget");
        DescField.text = isZero ? Namings.Tag("PriorityTargetDesc") : Namings.Tag("BaitPriorityTargetDesc");


        var costDelay = isZero ? Library.PriorityTargetCostTime : Library.BaitPriorityTargetCostTime;
        var costCount = isZero ? Library.PriorityTargetCostCount : Library.BaitPriorityTargetCostCount;

        CostCountField.text = Namings.TryFormat(Namings.Tag("ChargesCount"), costCount);
        CostDelayField.text = Namings.TryFormat(Namings.Tag("ChargesDelay"), costDelay);
    }


}

