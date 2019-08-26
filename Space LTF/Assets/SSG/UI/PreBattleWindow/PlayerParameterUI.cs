using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerParameterUI : MonoBehaviour
{
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI NameField;
    public Button LevelUpButton;
    public TextMeshProUGUI FieldMaxLevel;
    public TextMeshProUGUI FieldLevel;
    private PlayerParameter _parameter;

    public void Init(PlayerParameter parameter)
    {
        _parameter = parameter;
        NameField.text = parameter.Name;
        _parameter.OnUpgrade += OnUpgrade;
        OnUpgrade(_parameter);
    }

    private void OnUpgrade(PlayerParameter obj)
    {
        LevelField.text = obj.Level.ToString();
    }

    public void Dispose()
    {

        _parameter.OnUpgrade += OnUpgrade;
    }
}

