using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class MoneySlotUI : MonoBehaviour
{
    public ChangingCounter Field;

    public void Init(int count,string prefix = "")
    {
        Field.Init(count,1000,30, prefix);
    }
}

