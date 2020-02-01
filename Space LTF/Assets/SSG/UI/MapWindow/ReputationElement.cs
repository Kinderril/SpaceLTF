using System;
using TMPro;
using UnityEngine;


public class ReputationElement : UIElementWithTooltip
{
    public TextMeshProUGUI Field;

    public TextMeshProUGUI StatusField;
    public TextMeshProUGUI EnemiesField;
    private string _tooltip;

    public void Init(ShipConfig config)
    {
        Field.text = Info(config);
        var rep = MainController.Instance.MainPlayer.ReputationData;
        var enemiesList = rep.Enemies;
        var list = enemiesList[config];
        string enemiesString = "";
        for (int i = 0; i < list.Count; i++)
        {
            var enemy = list[i];
            var enemyName = Namings.ShipConfig(enemy);
            if (i > 0)
            {
                enemiesString = $"{enemiesString}, {enemyName}";
            }
            else
            {
                enemiesString = enemyName;
            }

        }

        var status = rep.GetStatus(config);
        StatusField.text = Namings.Tag($"rep_{status.ToString()}");
        Color color = Color.white;
        switch (status)
        {
            case EReputationStatus.friend:
                color = new Color(0f, 1f, 85f / 255f, 1f);
                break;
            case EReputationStatus.neutral:
                color = Color.white;
                break;
            case EReputationStatus.negative:
                color = new Color(1f, 115f / 255f, 0f, 1f);
                break;
            case EReputationStatus.enemy:
                color = new Color(1f, 215f / 255f, 0f, 1f);
                break;
        }

        StatusField.color = color;
        var en = String.Format(Namings.Tag("ReputationElement"), enemiesString);
        EnemiesField.text = en;
        _tooltip = Namings.Tag($"rep_adv_{status.ToString()}");
        if (config == ShipConfig.droid)
        {
            StatusField.gameObject.SetActive(false);
            EnemiesField.gameObject.SetActive(false);

        }
    }

    protected override string TextToTooltip()
    {
        return _tooltip;
    }

    private string Info(ShipConfig mercenary)
    {
        var rep = MainController.Instance.MainPlayer.ReputationData;
        return String.Format(Namings.Tag("ReputationData"), Namings.ShipConfig(mercenary), rep.ReputationFaction[mercenary]);
    }
}

