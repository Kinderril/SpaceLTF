using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class ArmyDataInfoUI : MonoBehaviour
{
    public TextMeshProUGUI Field;

    public void Init(string info)
    {
        Field.text = info;
    }
}

