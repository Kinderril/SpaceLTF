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
    public UIElementWithTooltipCache Tooltip;

    private PlayerParameter _parameter;

    public void Init(PlayerParameter parameter)
    {
        _parameter = parameter;
        NameField.text = parameter.Name;
        _parameter.OnUpgrade += OnUpgrade;
        OnUpgrade(_parameter);
    }
                           
    private void UpgradeTooltip()
    {
        string data  = "no data";
        switch (_parameter.ParameterType)
        {
            case PlayerParameterType.scout:
                data = Namings.Format(Namings.Tag("ParameterTypeScouts"));
                break;
            case PlayerParameterType.repair:
                var percent =  (1 + _parameter.Level) * Library.REPAIR_PERCENT_PERSTEP_PERLEVEL;
                data = Namings.Format(Namings.Tag("ParameterTypeRepair"),Utils.FloatToChance(percent));
                break;
            case PlayerParameterType.chargesCount:
                data = Namings.Format(Namings.Tag("ParameterTypeChanrgeCount"), (_parameter.Level + Library.BASE_CHARGES_COUNT));
                break;                                
            case PlayerParameterType.chargesSpeed:            
                var coef = (_parameter.Level - 1) * Library.CHARGE_SPEED_COEF_PER_LEVEL;
                data = Namings.Format(Namings.Tag("ParameterTypeChanrgeSpeed"), Utils.FloatToChance(coef));

                break;
            case PlayerParameterType.engineParameter:
                var radius = CommanderSpells.COMMANDER_BLINK_BASE_DIST + _parameter.Level * CommanderSpells.COMMANDER_BLINK_LEVEL_DIST;
                var delay = CommanderSpells.COMMANDER_BLINK_BASE_PERIOD - _parameter.Level * CommanderSpells.COMMANDER_BLINK_LEVEL_PERIOD;
                data = Namings.Format(Namings.Tag("ParameterTypeEngine"), radius, delay);
                break;
        }

        Tooltip.Cache = data;
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
        UpgradeTooltip();
    }

    public void Dispose()
    {
        _parameter.OnUpgrade -= OnUpgrade;
    }
}

