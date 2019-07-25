using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CreditControllerUI : MonoBehaviour
{
    public Text Field;
    private Commander _commander;

    public void Init(Commander commander)
    {
        _commander = commander;
//        commander.RewardController.OnCreditChange += OnCreditChange;
//        OnCreditChange(commander.RewardController.CurCredit);
    }
    

    private void OnCreditChange(int obj)
    {
        Field.text = obj.ToString();
    }

    public void Dispose()
    {
//        _commander.RewardController.OnCreditChange -= OnCreditChange;
    }
}

