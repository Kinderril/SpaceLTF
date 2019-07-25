using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainShipUpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI CostField;
    public Button UpgradeButton;
    private PlayerParameter _parameter;

    public void Init(PlayerParameter pParameter)
    {
        _parameter = pParameter;
        _parameter.OnUpgrade += OnUpgrade;
        NameField.text = pParameter.Name;
        FieldsUpdate();
    }

    private void OnUpgrade(PlayerParameter obj)
    {
        FieldsUpdate();
    }

    private void FieldsUpdate()
    {
        LevelField.text = _parameter.Level.ToString();
        var isMax = _parameter.IsMaxLevel();
        if (isMax)
        {

            CostField.text = "Max";
        }
        else
        {
            CostField.text = _parameter.UpgradeCost().ToString();
        }
        UpgradeButton.interactable = !isMax;
    }

    public void OnClickUpgrade()
    {
        _parameter.TryUpgrade();
    }

    void OnDestroy()
    {
        Unsubscribe();
    }

    private void Unsubscribe()
    {
        _parameter.OnUpgrade -= OnUpgrade;
    }
}

