using UnityEngine;
using System.Collections;
using TMPro;

public class BattleEventTimerContainer : MonoBehaviour
{
    private BattleTypeEvent _battleType;
    public TextMeshProUGUI TimeField;
    public void Init(BattleTypeEvent battleType)
    {
        _battleType = battleType;
        if (_battleType != null && _battleType.HaveActiveTime)
        {
            TimeField.gameObject.SetActive(true);
            _battleType.OnTimeLeft += OnTimeLeft;
        }
        else
        {
            TimeField.gameObject.SetActive(false);
        }
    }

    private void OnTimeLeft(float obj,bool isLast,string msg)
    {
        if (isLast)
        {
            TimeField.gameObject.SetActive(false);
        }
        else
        {
            TimeField.text = $"{msg}:{obj.ToString("0.0")}";
        }

    }

    public void Dispose()
    {

        if (_battleType != null)
            _battleType.OnTimeLeft -= OnTimeLeft;
    }

}
