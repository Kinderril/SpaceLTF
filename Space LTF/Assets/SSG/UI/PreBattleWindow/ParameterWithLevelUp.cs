using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParameterWithLevelUp : MonoBehaviour
{
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI Field;

    private IPilotParameters _pilot;

    private LibraryPilotUpgradeType _type;
    public GameObject LevelUpButton;
    public UIElementWithTooltipCache TooltipCache;
    private int _level;

    public void SetData(string txt,int level,IPilotParameters pilot,LibraryPilotUpgradeType type)
    {
        _level = level;
        _pilot = pilot;
        LevelField.text = level.ToString();
        Field.text = txt;
        _type = type;
        LevelUpButton.SetActive(_pilot.CanUpgradeAnyParameter(0));
        UpdateTooltip();
    }

    public void OnLevelUpClicked()
    {
        if (_pilot.CanUpgradeAnyParameter(0))
        {
            _pilot.UpgradeLevel(true, _type,true);
            UpdateTooltip();
        }
    }

    public void UpdateTooltip()
    {
        var levelCoef = Library.PARAMETER_LEVEL_COEF;
        var val =  (_level - 1) * levelCoef; 
        TooltipCache.Cache = Namings.Format(Namings.Tag("BonusParamField"),Utils.FloatToChance(val));
    }

}

