using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class PlayerParameterUI : MonoBehaviour
{
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI NameField;

    public void Init(PlayerParameter parameter)
    {
        LevelField.text = parameter.Level.ToString();
        NameField.text = parameter.Name;
    }
}

