using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PreFinishWindow : MonoBehaviour
{
    private BattleController _battle;
    public TextMeshProUGUI Field;
    public Image BackgroundImage;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void Activate(BattleController battle, EndBattleType winner)
    {
        gameObject.SetActive(true);
        _battle = battle;
        Color color = new Color(255f / 255f, 153f / 255f, 51f / 255f, 1f);
        switch (winner)
        {
            case EndBattleType.win:
                color = Color.green;
                Field.text = "Win!";
                break;
            case EndBattleType.lose:
                Field.text = "Lose";
                color = Color.red;
                break;
            case EndBattleType.runAway:
                Field.text = "Run away complete";
                color = new Color(255f/255f, 153f / 255f, 51f / 255f,1f);
                break;
        }

        color.a = 0.8f;
        BackgroundImage.color = color;
    }

    public void OnClickFinish()
    {
        _battle.EndPart2Battle();
    }
}

