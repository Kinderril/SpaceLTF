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
    private TempCoin _coin;
    public Image Slider;
    public TextMeshProUGUI Field;

    public void Init(TempCoin coin)
    {
        _coin = coin;
        Slider.fillAmount = 0f;
        _coin.OnStateChange += OnStateChange;
        Field.text = "";
    }

    private void OnStateChange(TempCoin obj)
    {
        switch (_coin.State)
        {
            case CointState.Ready:
                if (Field.gameObject.activeSelf)
                {
                    Field.gameObject.SetActive(false);
                }

                Slider.fillAmount = 0f;
                break;    
            case CointState.Block:
            case CointState.Recharging:
                if (!Field.gameObject.activeSelf)
                {
                    Field.gameObject.SetActive(true);
                }
                break;
        }
    }

    void Update()
    {
        switch (_coin.State)
        {
            case CointState.Block:
                Slider.fillAmount = _coin.Percent();
                Field.text = _coin.RemainVal.ToString("0.0");
                break;
            case CointState.Ready:
                break;
            case CointState.Recharging:
                Slider.fillAmount = _coin.Percent();
                Field.text = _coin.RemainVal.ToString("0.0");
                break;
        }
    }


    public void Dispose()
    {
        _coin.OnStateChange -= OnStateChange;
    }
}

