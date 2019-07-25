using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CoinUI : MonoBehaviour
{
//    public Image CoinImage;
    private CommandCoin _coin;
    private Action<CoinUI,bool> _usedCallback;
    public Image Slider;
    public TextMeshProUGUI Field;

    public void Init(CommandCoin coin, Action<CoinUI, bool> usedCallback)
    {
        _usedCallback = usedCallback;
        _coin = coin;
        _coin.OnUsed += OnUsed;
        OnUsed(coin, coin.Used);
    }

    void Update()
    {
        if (_coin.Used)
        {
            if (!Field.gameObject.activeSelf)
            {
                Field.gameObject.SetActive(true);
            }
            var remainTime = (int) _coin.RemainTime();
            Field.text = remainTime.ToString("0");
            Slider.fillAmount = 1f - _coin.Percent();
        }
        else
        {
            if (Field.gameObject.activeSelf)
            {
                Field.gameObject.SetActive(false);
            }
            Slider.fillAmount = _coin.BlockedPercent();
        }
    }

    private void OnUsed(CommandCoin arg1, bool arg2)
    {

        if (arg2)
        {
            Slider.fillAmount = 0f;
//            Slider.gameObject.SetActive(true);
        }
        else
        {
            Slider.fillAmount = 1f;
            //            Slider.gameObject.SetActive(false);
        }
        _usedCallback(this, arg2);
    }

    public void Dispose()
    {
        _coin.OnUsed -= OnUsed;
    }
}

