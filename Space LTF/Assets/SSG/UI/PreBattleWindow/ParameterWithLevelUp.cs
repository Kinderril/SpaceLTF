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

    public void SetData(string txt,int level,IPilotParameters pilot,LibraryPilotUpgradeType type)
    {
        _pilot = pilot;
        LevelField.text = level.ToString();
        Field.text = txt;
        _type = type;
        LevelUpButton.SetActive(_pilot.CanUpgradeAnyParameter(0));
    }

    public void OnLevelUpClicked()
    {
        if (_pilot.CanUpgradeAnyParameter(0))
        {
            _pilot.UpgradeLevel(true, _type,true);
        }
    }

}

