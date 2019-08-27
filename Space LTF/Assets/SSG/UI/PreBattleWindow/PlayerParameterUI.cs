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
    public TextMeshProUGUI LvlUpCostField;

    private PlayerParameter _parameter;

    public void Init(PlayerParameter parameter)
    {
        _parameter = parameter;
        NameField.text = parameter.Name;
        _parameter.OnUpgrade += OnUpgrade;
        OnUpgrade(_parameter);
    }

    public void OnUpgradeClick()
    {
        _parameter.TryUpgrade();
    }

    private void OnUpgrade(PlayerParameter obj)
    {
        var cost = obj.UpgradeCost();
        var isMaxLvl = obj.IsMaxLevel();
        FieldMaxLevel.gameObject.SetActive(isMaxLvl);
        LevelUpButton.gameObject.SetActive(!isMaxLvl);
        LvlUpCostField.gameObject.SetActive(!isMaxLvl);

        LvlUpCostField.text = cost.ToString();
        LevelField.text = obj.Level.ToString();
    }

    public void Dispose()
    {
        _parameter.OnUpgrade += OnUpgrade;
    }
}

