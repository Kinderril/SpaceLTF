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
    public Button LevelUpButton;
    public UIElementWithTooltipCache Tooltip;
    public Image Icon;

    private PlayerParameter _parameter;

    public void Init(PlayerParameter parameter)
    {
        _parameter = parameter;
        //        NameField.text = parameter.Name;
        Icon.sprite = DataBaseController.Instance.DataStructPrefabs.GetParameterIcon(parameter.ParameterType);
        _parameter.OnUpgrade += OnUpgrade;
        OnUpgrade(_parameter);
    }
                           
    private void UpgradeTooltip()
    {
        string data  = "no data";
        switch (_parameter.ParameterType)
        {
            case PlayerParameterType.scout:
                var lvl = Namings.Tag("Level");
                var p0 = Namings.Format(Namings.Tag("ParameterTypeScouts"));
                var p1 = Namings.Format(Namings.Tag("ParameterTypeScouts1"));
                var p2 = Namings.Format(Namings.Tag("ParameterTypeScouts2"));
                var p3 = Namings.Format(Namings.Tag("ParameterTypeScouts3"));
                var p4 = Namings.Format(Namings.Tag("ParameterTypeScouts4"));
                data = $"{_parameter.Name}\n{p0}\n{lvl} 1: {p1}\n{lvl} 2: {p2}\n{lvl} 3: {p3}\n{lvl} 4: {p4}";      
                break;
            case PlayerParameterType.repair:
                var percent =  (1 + _parameter.Level) * Library.REPAIR_PERCENT_PERSTEP_PERLEVEL;
                var r0 = Namings.Format(Namings.Tag("ParameterTypeRepair"), Utils.FloatToChance(percent));
                data = $"{_parameter.Name}\n{r0}";
                break;
            case PlayerParameterType.chargesCount:
                var c0 = Namings.Format(Namings.Tag("ParameterTypeChanrgeCount"), (_parameter.Level + Library.BASE_CHARGES_COUNT));
                data = $"{_parameter.Name}\n{c0}";
                break;                                
            case PlayerParameterType.chargesSpeed:            
                var coef = (_parameter.Level - 1) * Library.CHARGE_SPEED_COEF_PER_LEVEL;
                var cs0 = Namings.Format(Namings.Tag("ParameterTypeChanrgeSpeed"), Utils.FloatToChance(coef));

                data = $"{_parameter.Name}\n{cs0}";
                break;
            case PlayerParameterType.engineParameter:
                var radius = CommanderSpells.COMMANDER_BLINK_BASE_DIST + _parameter.Level * CommanderSpells.COMMANDER_BLINK_LEVEL_DIST;
                var delay = CommanderSpells.COMMANDER_BLINK_BASE_PERIOD - _parameter.Level * CommanderSpells.COMMANDER_BLINK_LEVEL_PERIOD;
                var e0 = Namings.Format(Namings.Tag("ParameterTypeEngine"), radius, delay);

                data = $"{_parameter.Name}\n{e0}";
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
//        FieldMaxLevel.gameObject.SetActive(isMaxLvl);
        LevelUpButton.gameObject.SetActive(!isMaxLvl);
//        LvlUpCostField.gameObject.SetActive(!isMaxLvl);

//        LvlUpCostField.text = cost.ToString();
        LevelField.text = obj.Level.ToString();
        UpgradeTooltip();
    }

    public void Dispose()
    {
        _parameter.OnUpgrade -= OnUpgrade;
    }
}

