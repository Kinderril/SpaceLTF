using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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


        NameField.text = isZero ? Namings.PriorityTarget : Namings.BaitPriorityTarget;
        DescField.text = isZero ? Namings.PriorityTargetDesc : Namings.BaitPriorityTargetDesc;


        var costDelay = isZero ? Library.PriorityTargetCostTime : Library.BaitPriorityTargetCostTime;
        var costCount = isZero ? Library.PriorityTargetCostCount : Library.BaitPriorityTargetCostCount;

        CostCountField.text = String.Format(Namings.ChargesCount, costCount); 
        CostDelayField.text = String.Format(Namings.ChargesDelay, costDelay); 
    }


}

