using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PilotBattleUI : MonoBehaviour
{
    public TextMeshProUGUI LevelField;
    public TextMeshProUGUI SpeedField;
    public TextMeshProUGUI TurnField;

    public void Init(IPilotParameters pilotParameters)
    {
        LevelField.text = Namings.Level +  ":" + pilotParameters.CurLevel.ToString();
        SpeedField.text = Namings.Tag("Speed") + ":" + pilotParameters.SpeedLevel.ToString();
        TurnField.text = Namings.Tag("TurnSpeed") + ":" + pilotParameters.TurnSpeedLevel.ToString();
    }

}

