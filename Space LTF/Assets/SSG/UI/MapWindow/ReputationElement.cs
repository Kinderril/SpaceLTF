using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class ReputationElement : UIElementWithTooltip
{
    public TextMeshProUGUI Field;
    private string _tooltip;

    public void Init(ShipConfig config)
    {
        Field.text = Info(config);
        var enemiesList = MainController.Instance.MainPlayer.ReputationData.Enemies;
        var list = enemiesList[config];
        string enemiesString = "";
        for (int i = 0; i < list.Count; i++)
        {
            var enemy = list[i];
            var enemyName = Namings.ShipConfig(enemy);
            if (i == 0)
            {
                enemiesString = $"{enemiesString}, {enemyName}";
            }
            else
            {
                enemiesString = enemyName;
            }
        }

        _tooltip = String.Format(Namings.Tag("ReputationElement"), enemiesString);
    }

    protected override string TextToTooltip()
    {
        return _tooltip;
    }

    private string Info(ShipConfig mercenary)
    {
        var rep = MainController.Instance.MainPlayer.ReputationData;
        return String.Format(Namings.Reputation, Namings.ShipConfig(mercenary), rep.ReputationFaction[mercenary]);
    }
}

