using System;
using TMPro;
using UnityEngine;


public class ReputationElement : UIElementWithTooltip
{
    public TextMeshProUGUI Field;

    public TextMeshProUGUI StatusField;
    public TextMeshProUGUI EnemiesField;
    private string _tooltip;
    private Color _friendColor = new Color(0f, 1f, 85f / 255f, 1f);

    public void Init(ShipConfig config, ShipConfig? allies, EReputationAlliesRank alliesRank)
    {
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

        if (allies.HasValue && allies.Value == config)
        {

            Field.text = Namings.ShipConfig(config);
            StatusField.text = Namings.Format(Namings.Tag($"rep_allies_rank"), Namings.Tag($"alliesRank_{alliesRank.ToString()}"));
            Field.color = StatusField.color = _friendColor;

        }
        else
        {
            Field.text = Info(config);
            StatusField.text = Namings.Tag($"rep_{status.ToString()}");
            Color color = Color.white;
            switch (status)
            {
                case EReputationStatus.friend:
                    color = _friendColor;
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
//            StatusField.gameObject.SetActive(true);

        }
        var en = Namings.Format(Namings.Tag("ReputationElement"), enemiesString);
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
        return Namings.Format(Namings.Tag("ReputationData"), Namings.ShipConfig(mercenary), rep.ReputationFaction[mercenary]);
    }
}

