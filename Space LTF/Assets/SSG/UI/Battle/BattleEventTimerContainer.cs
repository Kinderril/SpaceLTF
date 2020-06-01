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
        if (_battleType != null)
        {
            TimeField.gameObject.SetActive(true);
            if (_battleType.HaveActiveTime)
            {
                _battleType.OnTimeLeft += OnTimeLeft;
            }
            else
            {
                TimeField.text = _battleType.GetMsg();
            }
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
